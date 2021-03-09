// -----------------------------------------------------------------------
// <copyright file="ReadByAllWriteByUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using StatesAndProvinces;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// User type with widest permissions (read by all, write by user, all by admin).
    /// </summary>
    public class ReadByAllWriteByUser : UserBase, IValidatable, IUpdatable<ReadByAllWriteByUser>
    {
        /// <summary>
        /// Gets or sets user first name, required
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets user last name, not required (in the case where people don't have/don't want to share last names)
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets a member's facebook name, not required
        /// </summary>
        public string? FacebookName { get; set; }

        /// <summary>
        /// Gets or sets the user's city, not required
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the user's state or province, not required.  Must belong to states in supported countries in settings.
        /// </summary>
        public string? Region { get; set; }

        /// <summary>
        /// Gets or sets the user's postal code, not required
        /// </summary>
        public int? PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the user's country, not required.  Must belong to set of supported countries in settings.
        /// </summary>
        public Country? Country { get; set; }

        /// <summary>
        /// Gets or sets the user's occupation, not required
        /// </summary>
        public string? Occupation { get; set; }

        /// <summary>
        /// Gets or sets surfer level
        /// </summary>
        public Level? Level { get; set; }

        /// <summary>
        /// Gets or sets URL of a user's profile photo
        /// </summary>
        public string? PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets a user's biography
        /// </summary>
        public string? Biography { get; set; }

        /// <summary>
        /// Gets or sets the date a member started surfing
        /// </summary>
        public DateTime? StartedSurfing { get; set; }

        /// <summary>
        /// Gets or sets a member's boards
        /// </summary>
        public ICollection<string> Boards { get; set; } = new Collection<string>();

        /// <summary>
        /// Gets or sets a member's usual surf spots
        /// </summary>
        public ICollection<string> SurfSpots { get; set; } = new Collection<string>();

        /// <inheritdoc/>
        public new void Validate()
        {
            // Validate base
            base.Validate();

            // User must have a name
            this.FirstName = Ensure.IsNotNullOrWhitespace(() => this.FirstName);

            // Country & state validation
            if (this.Region != null)
            {
                // Country cannot be null if Region is
                Ensure.IsNotNull(() => this.Country);

                #pragma warning disable CS8629 // Nullable value checked for above

                // Cast country to CountrySelection to check for states
                var regions = Factory.Make((CountrySelection)this.Country);

                #pragma warning restore CS8629

                // Map to region names
                var regionNames = regions.Select(region => region.Name);

                // Check to see if region name contains the requested region
                if (!regionNames.Contains(this.Region))
                {
                    throw new ArgumentException($"{this.Region} is not a supported region");
                }
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
            stringBuilder.AppendLine("ReadByAllWriteByUser Section");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"First Name: {this.FirstName}");
            stringBuilder.AppendLine($"Last Name: {this.LastName}");
            stringBuilder.AppendLine($"Facebook Name: {this.FacebookName}");
            stringBuilder.AppendLine($"City: {this.City}");
            stringBuilder.AppendLine($"Region: {this.Region}");
            stringBuilder.AppendLine($"Postal Code: {this.PostalCode}");
            stringBuilder.AppendLine($"Country: {this.Country}");
            stringBuilder.AppendLine($"Occupation: {this.Occupation}");
            stringBuilder.AppendLine($"Level: {this.Level}");
            stringBuilder.AppendLine($"Photo Url: {this.PhotoUrl}");
            stringBuilder.AppendLine($"Biography: {this.Biography}");
            stringBuilder.AppendLine($"Started Surfing: {this.StartedSurfing}");
            stringBuilder.AppendLine("Boards:");

            foreach (var board in this.Boards)
            {
                stringBuilder.AppendLine(board);
            }

            stringBuilder.AppendLine("Surf Spots:");

            foreach (var surfSpot in this.SurfSpots)
            {
                stringBuilder.AppendLine(surfSpot);
            }

            return stringBuilder.ToString();
        }

        /// <inheritdoc/>
        public void Update(ReadByAllWriteByUser user)
        {
            // Update base
            base.Update(user);

            // Update Properties
            this.FirstName = user.FirstName ?? this.FirstName;
            this.LastName = user.LastName ?? this.LastName;
            this.FacebookName = user.FacebookName ?? this.FacebookName;
            this.City = user.City ?? this.City;
            this.Region = user.Region ?? this.Region;
            this.PostalCode = user.PostalCode ?? this.PostalCode;
            this.Country = user.Country ?? this.Country;
            this.Occupation = user.Occupation ?? this.Occupation;
            this.Level = user.Level ?? this.Level;
            this.PhotoUrl = user.PhotoUrl ?? this.PhotoUrl;
            this.Biography = user.Biography ?? this.Biography;
            this.StartedSurfing = user.StartedSurfing ?? this.StartedSurfing;
            this.Boards = user.Boards ?? this.Boards;
            this.SurfSpots = user.SurfSpots ?? this.SurfSpots;

            // Ensure validation
            this.Validate();
        }
    }
}
