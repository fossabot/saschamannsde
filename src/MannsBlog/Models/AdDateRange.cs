// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

namespace MannsBlog.Models
{
    /// <summary>
    /// Ad Date Range Service.
    /// </summary>
    public class AdDateRange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdDateRange"/> class.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="ads">The ads.</param>
        /// <exception cref="System.InvalidOperationException">Invalid Ads</exception>
        public AdDateRange(string startDate, string endDate, params string[] ads)
        {
            if (!DateTime.TryParse(startDate, out Start) || !DateTime.TryParse(endDate, out End) || ads == null || ads.Length == 0)
            {
                throw new InvalidOperationException("Invalid Ads");
            }

            Ads = ads;
        }

        public readonly DateTime Start;
        public readonly DateTime End;
        public readonly string[] Ads;
    }
}
