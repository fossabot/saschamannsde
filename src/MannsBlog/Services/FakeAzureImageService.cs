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
using System.IO;
using System.Threading.Tasks;
using WilderMinds.AzureImageStorageService;

namespace MannsBlog.Services
{
    /// <summary>
    /// Fake Service for Azure Blob Services.
    /// </summary>
    /// <seealso cref="WilderMinds.AzureImageStorageService.IAzureImageStorageService" />
    public class FakeAzureImageService : IAzureImageStorageService
    {
        /// <summary>
        /// Stores the image.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="storageImagePath">The storage image path.</param>
        /// <param name="imageStream">The image stream.</param>
        /// <returns>True ImageResponse.</returns>
        public Task<ImageResponse> StoreImage(string containerName, string storageImagePath, Stream imageStream)
        {
            return Task.FromResult(new ImageResponse() { Success = true });
        }

        /// <summary>
        /// Stores the image.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="storeImagePath">The store image path.</param>
        /// <param name="imageData">The image data.</param>
        /// <returns>True Image Response.</returns>
        public Task<ImageResponse> StoreImage(string containerName, string storeImagePath, byte[] imageData)
        {
            return Task.FromResult(new ImageResponse() { Success = true });
        }
    }
}
