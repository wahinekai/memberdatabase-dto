// -----------------------------------------------------------------------
// <copyright file="ReadByAllUser.cs" company="Wahine Kai">
// Copyright (c) Wahine Kai. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace WahineKai.MemberDatabase.Dto.Models
{
    using System.Collections.Generic;
    using WahineKai.Common;
    using WahineKai.Common.Contracts;

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
        public Enums.Chapter? Chapter { get; set; }

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

            // Every user belongs to a chapter
            this.Chapter = Ensure.IsNotNull(() => this.Chapter);
        }
    }
}
