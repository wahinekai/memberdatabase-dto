// -----------------------------------------------------------------------
// <copyright file="Position.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.Text;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

    /// <summary>
    /// Object representing a position a user can hold
    /// </summary>
    public class Position : IValidatable, IUpdatable<Position>
    {
        /// <summary>
        /// Gets or sets the leadership position of the user, required
        /// </summary>
        public Enums.Position? Name { get; set; }

        /// <summary>
        /// Gets or sets the date the user started their particular position, required
        /// </summary>
        public DateTime? Started { get; set; }

        /// <summary>
        /// Gets or sets the date the user ended their particular position
        /// </summary>
        public DateTime? Ended { get; set; }

        /// <inheritdoc/>
        public void Validate()
        {
            // Name must not be null
            this.Name = Ensure.IsNotNull(() => this.Name);

            // Started must not be null
            this.Started = Ensure.IsNotNull(() => this.Started);
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

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Position Model");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"Name: {this.Name}");
            stringBuilder.AppendLine($"Date Started: {this.Started}");
            stringBuilder.AppendLine($"Date Ended: {this.Ended}");

            return stringBuilder.ToString();
        }

        /// <inheritdoc/>
        public void Update(Position position)
        {
            // Update Properties
            this.Name = position.Name ?? this.Name;
            this.Started = position.Started ?? this.Started;
            this.Ended = position.Ended ?? this.Ended;

            // Ensure validation
            this.Validate();
        }
    }
}
