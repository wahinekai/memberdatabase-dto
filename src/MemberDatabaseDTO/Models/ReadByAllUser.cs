// -----------------------------------------------------------------------
// <copyright file="ReadByAllUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;
    using WahineKai.MemberDatabase.Dto.Enums;

    /// <summary>
    /// Extension of users that can be read by all
    /// </summary>
    public class ReadByAllUser : ReadByAllWriteByUser, IValidatable
    {
        /// <summary>
        /// Gets or sets list of user positions
        /// </summary>
        public IList<Position> Positions { get; set; } = new List<Position>();

        /// <summary>
        /// Gets or sets the user's chapter, required.  Must belong to set of supported chapters in settings
        /// </summary>
        public Chapter Chapter { get; set; } = Chapter.WahineKaiInternational;

        /// <inheritdoc/>
        public new void Validate()
        {
            // Validate base
            base.Validate();

            // Validate positions
            foreach (var position in this.Positions)
            {
                position.Validate();
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
            stringBuilder.AppendLine("ReadByAll Section");
            stringBuilder.AppendLine($"Valid?: {valid}");
            stringBuilder.AppendLine($"Chapter: {this.Chapter}");
            stringBuilder.AppendLine("Positions: ");

            foreach (var position in this.Positions)
            {
                stringBuilder.Append(position.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
