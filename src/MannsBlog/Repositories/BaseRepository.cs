// <copyright file="BaseRepository.cs" company="Sascha Manns">
// @author: Sascha Manns, Sascha.Manns@outlook.de
// @copyright: 2022, Sascha Manns, https://saschamanns.de
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
// </copyright>

namespace MannsBlog.Repositories
{
    /// <summary>
    /// Base Repository.
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Calculates the pages.
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Calculated Pagecount.</returns>
        protected int CalculatePages(int totalCount, int pageSize)
        {
            return ((int)(totalCount / pageSize)) + ((totalCount % pageSize) > 0 ? 1 : 0);
        }
    }
}