// -----------------------------------------------------------------------
// <copyright file="AdminUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.Text;
    using Newtonsoft.Json;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Model of a user with all fields - to be worked on by admin
    /// </summary>
    public class AdminUser : ReadByAllUser, IValidatable
    {
        /// <summary>
        /// Gets or sets a value indicating whether a user is an admin user
        /// </summary>
        public bool Admin { get; set; } = false;

        /// <summary>
        /// Gets or sets a member's PayPal Name, not required
        /// </summary>
        public string? PayPalName { get; set; }

        /// <summary>
        /// Gets or sets user phone number, not required
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's street address, not required
        /// </summary>
        public string? StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the user's birth date - not required.
        /// </summary>
        public DateTime? Birthdate { get; set; }

        /// <summary>
        /// Gets the age of the user
        /// </summary>
        public int? Age { get => this.CalculateAge(); }

        /// <summary>
        /// Gets or sets the membership status of the user.
        /// </summary>
        public MemberStatus Status { get; set; } = MemberStatus.Pending;

        /// <summary>
        /// Gets or sets the date the user joined the Wahine Kais, required
        /// </summary>
        public DateTime? JoinedDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user needs to renew their membership
        /// </summary>
        public DateTime? RenewalDate { get; set; }

        /// <summary>
        /// Gets or sets the date the user terminated their membership with the Wahine Kais
        /// </summary>
        public DateTime? TerminatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in the facebook chapter group
        /// </summary>
        public ChapterEnteredStatus EnteredInFacebookChapter { get; set; } = ChapterEnteredStatus.Entered;

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in facebook WKI
        /// </summary>
        public WkiEnteredStatus EnteredInFacebookWki { get; set; } = WkiEnteredStatus.NotEntered;

        /// <summary>
        /// Gets or sets a value indicating whether a user needs a new member bag
        /// </summary>
        public bool NeedsNewMemberBag { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a user has won a surfboard
        /// </summary>
        public bool WonSurfboard { get; set; } = false;

        /// <summary>
        /// Gets or sets the date a user has won a surfboard - null if the user hasn't won
        /// </summary>
        public DateTime? DateSurfboardWon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a member has opted out of social media
        /// </summary>
        public bool SocialMediaOptOut { get; set; } = false;

        /// <summary>
        /// Gets or sets TimeStamp Property from Cosmos DB
        /// </summary>
        [JsonProperty(PropertyName = "_ts")]
        public long? TimeStamp { get; set; }

        /// <summary>
        /// Clone the old user, and replace it with the parameters of the updated user
        /// </summary>
        /// <param name="oldUser">READONLY old user to update</param>
        /// <param name="updatedUser">READONLY updated user to replace the parameters of the old user</param>
        /// <returns>A new user with the parameters of the old user replaced by the new user.</returns>
        public static AdminUser Replace(AdminUser oldUser, AdminUser updatedUser)
        {
            // Create a deep copy of oldUser
            var replacedUser = oldUser.Clone();

            // Update updatable parameters
            replacedUser.Admin = updatedUser.Admin;
            replacedUser.Email = updatedUser.Email;
            replacedUser.FirstName = updatedUser.FirstName ?? oldUser.FirstName;
            replacedUser.LastName = updatedUser.LastName ?? oldUser.LastName;
            replacedUser.FacebookName = updatedUser.FacebookName ?? oldUser.FacebookName;
            replacedUser.PayPalName = updatedUser.PayPalName ?? oldUser.PayPalName;
            replacedUser.PhoneNumber = updatedUser.PhoneNumber ?? oldUser.PhoneNumber;
            replacedUser.StreetAddress = updatedUser.StreetAddress ?? oldUser.StreetAddress;
            replacedUser.City = updatedUser.City ?? oldUser.City;
            replacedUser.Region = updatedUser.Region ?? oldUser.Region;
            replacedUser.Country = updatedUser.Country ?? oldUser.Country;
            replacedUser.Occupation = updatedUser.Occupation ?? oldUser.Occupation;
            replacedUser.Chapter = updatedUser.Chapter;
            replacedUser.Birthdate = updatedUser.Birthdate ?? oldUser.Birthdate;
            replacedUser.Level = updatedUser.Level ?? oldUser.Level;
            replacedUser.StartedSurfing = updatedUser.StartedSurfing ?? oldUser.StartedSurfing;
            replacedUser.Boards = updatedUser.Boards ?? oldUser.Boards;
            replacedUser.SurfSpots = updatedUser.SurfSpots ?? oldUser.SurfSpots;
            replacedUser.PhotoUrl = updatedUser.PhotoUrl ?? oldUser.PhotoUrl;
            replacedUser.Biography = updatedUser.Biography ?? oldUser.Biography;
            replacedUser.Status = updatedUser.Status;
            replacedUser.JoinedDate = updatedUser.JoinedDate ?? oldUser.JoinedDate;
            replacedUser.RenewalDate = updatedUser.RenewalDate ?? oldUser.RenewalDate;
            replacedUser.TerminatedDate = updatedUser.TerminatedDate ?? oldUser.TerminatedDate;
            replacedUser.Positions = updatedUser.Positions ?? oldUser.Positions;
            replacedUser.EnteredInFacebookChapter = updatedUser.EnteredInFacebookChapter;
            replacedUser.EnteredInFacebookWki = updatedUser.EnteredInFacebookWki;
            replacedUser.NeedsNewMemberBag = updatedUser.NeedsNewMemberBag;
            replacedUser.WonSurfboard = updatedUser.WonSurfboard;
            replacedUser.DateSurfboardWon = updatedUser.DateSurfboardWon ?? oldUser.DateSurfboardWon;
            replacedUser.PostalCode = updatedUser.PostalCode ?? oldUser.PostalCode;
            replacedUser.SocialMediaOptOut = updatedUser.SocialMediaOptOut;

            // Validate and return replaced user
            replacedUser.Validate();
            return replacedUser;
        }

        /// <inheritdoc/>
        public new void Validate()
        {
            base.Validate();

            // Joined/Renewal/Terminated date validation
            // Depends on the status of the member
            switch (this.Status)
            {
                case MemberStatus.Pending:
                    // Nothing is required
                    break;
                case MemberStatus.ActivePaying:
                    // Joined & Renewal Date Required, terminated date null
                    this.JoinedDate = Ensure.IsNotNull(() => this.JoinedDate);
                    this.RenewalDate = Ensure.IsNotNull(() => this.RenewalDate);
                    this.TerminatedDate = null;
                    break;
                case MemberStatus.ActiveNonPaying or MemberStatus.LifetimeMember:
                    // Joined required, renewal & terminated null
                    this.JoinedDate = Ensure.IsNotNull(() => this.JoinedDate);
                    this.RenewalDate = null;
                    this.TerminatedDate = null;
                    break;
                case MemberStatus.Terminated:
                    // Joined & Terminated requried, renewal null
                    this.JoinedDate = Ensure.IsNotNull(() => this.JoinedDate);
                    this.RenewalDate = null;
                    this.TerminatedDate = Ensure.IsNotNull(() => this.JoinedDate);
                    break;
            }

            // If user has won a surfboard, must have the date won
            if (this.WonSurfboard == true)
            {
                Ensure.IsNotNull(() => this.DateSurfboardWon);
            }
        }

        /// <summary>
        /// Creates a deep copy of the object
        /// </summary>
        /// <returns>The new user cloned from this user</returns>
        public AdminUser Clone()
        {
            return new AdminUser
            {
                Id = this.Id,
                Admin = this.Admin,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Status = this.Status,
                FacebookName = this.FacebookName,
                PayPalName = this.PayPalName,
                Email = this.Email,
                PhoneNumber = this.PhoneNumber,
                StreetAddress = this.StreetAddress,
                City = this.City,
                Region = this.Region,
                Country = this.Country,
                Occupation = this.Occupation,
                Chapter = this.Chapter,
                Birthdate = this.Birthdate,
                Level = this.Level,
                StartedSurfing = this.StartedSurfing,
                Boards = this.Boards,
                SurfSpots = this.SurfSpots,
                PhotoUrl = this.PhotoUrl,
                Biography = this.Biography,
                JoinedDate = this.JoinedDate,
                RenewalDate = this.RenewalDate,
                TerminatedDate = this.TerminatedDate,
                Positions = this.Positions,
                EnteredInFacebookChapter = this.EnteredInFacebookChapter,
                EnteredInFacebookWki = this.EnteredInFacebookWki,
                NeedsNewMemberBag = this.NeedsNewMemberBag,
                WonSurfboard = this.WonSurfboard,
                DateSurfboardWon = this.DateSurfboardWon,
                PostalCode = this.PostalCode,
                SocialMediaOptOut = this.SocialMediaOptOut,
            };
        }

        /// <summary>
        /// Override of base ToString Method
        /// </summary>
        /// <returns>A printable string representing this document</returns>
        public override string ToString()
        {
            bool valid = true;
            try
            {
                this.Validate();
            }
            catch (Exception)
            {
                valid = false;
            }

            var stringBuilder = new StringBuilder(base.ToString());
            stringBuilder.AppendLine("AdminUser Section");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"Admin: {this.Admin}");
            stringBuilder.AppendLine($"Status: {this.Status}");
            stringBuilder.AppendLine($"PayPalName: {this.PayPalName}");
            stringBuilder.AppendLine($"PhoneNumber: {this.PhoneNumber}");
            stringBuilder.AppendLine($"StreetAddress: {this.StreetAddress}");
            stringBuilder.AppendLine($"Birthdate: {this.Birthdate}");
            stringBuilder.AppendLine($"Age: {this.Age}");
            stringBuilder.AppendLine($"JoinedDate: {this.JoinedDate}");
            stringBuilder.AppendLine($"RenewalDate: {this.RenewalDate}");
            stringBuilder.AppendLine($"TerminatedDate: {this.TerminatedDate}");
            stringBuilder.AppendLine($"EnteredInFacebookChapter: {this.EnteredInFacebookChapter}");
            stringBuilder.AppendLine($"EnteredInFacebookWki: {this.EnteredInFacebookWki}");
            stringBuilder.AppendLine($"NeedsNewMemberBag: {this.NeedsNewMemberBag}");
            stringBuilder.AppendLine($"WonSurfboard: {this.WonSurfboard}");
            stringBuilder.AppendLine($"Admin: {this.Admin}");
            stringBuilder.AppendLine($"DateSurfboardWon: {this.DateSurfboardWon}");
            stringBuilder.AppendLine($"SocialMediaOptOut: {this.SocialMediaOptOut}");
            stringBuilder.AppendLine($"TimeStamp: {this.TimeStamp}");

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Calculates the age of the user from the birthdate
        /// </summary>
        /// <returns>The age in years of the user</returns>
        private int? CalculateAge()
        {
            // Returns null if no birthdate given
            if (this.Birthdate == null)
            {
                return null;
            }

            var age = DateTime.Today.Year - this.Birthdate.Value.Year;

            // Account for leap years
            if (this.Birthdate?.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
    }
}
