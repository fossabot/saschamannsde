// MIT License
//
// Copyright (c) 2022 Sascha Manns
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
