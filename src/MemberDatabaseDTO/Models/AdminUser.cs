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
    public class AdminUser : ReadByAllUser, IValidatable, IUpdatable<AdminUser>
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
        public EnteredStatus EnteredInFacebookChapter { get; set; } = EnteredStatus.Entered;

        /// <summary>
        /// Gets or sets a value indicating whether the user has been entered in facebook WKI
        /// </summary>
        public EnteredStatus EnteredInFacebookWki { get; set; } = EnteredStatus.Entered;

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

        /// <inheritdoc/>
        public void Update(AdminUser user)
        {
            // Update base
            base.Update(user);

            // Update Properties
            this.Positions = user.Positions ?? this.Positions;
            this.Admin = user.Admin;
            this.PayPalName = user.PayPalName ?? this.PayPalName;
            this.PhoneNumber = user.PhoneNumber ?? this.PhoneNumber;
            this.StreetAddress = user.StreetAddress ?? this.StreetAddress;
            this.Birthdate = user.Birthdate ?? this.Birthdate;
            this.Status = user.Status;
            this.JoinedDate = user.JoinedDate ?? this.JoinedDate;
            this.RenewalDate = user.RenewalDate ?? this.RenewalDate;
            this.TerminatedDate = user.TerminatedDate ?? this.TerminatedDate;
            this.EnteredInFacebookChapter = user.EnteredInFacebookChapter;
            this.EnteredInFacebookWki = user.EnteredInFacebookWki;
            this.NeedsNewMemberBag = user.NeedsNewMemberBag;
            this.DateSurfboardWon = user.DateSurfboardWon ?? this.DateSurfboardWon;
            this.WonSurfboard = user.WonSurfboard;
            this.SocialMediaOptOut = user.SocialMediaOptOut;

            // Ensure validation
            this.Validate();
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
