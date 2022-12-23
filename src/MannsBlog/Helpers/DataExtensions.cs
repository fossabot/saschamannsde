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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using MannsBlog.EntityFramework.Entities;

namespace MannsBlog.Helpers
{
    /// <summary>
    /// Extensionclass for Data.
    /// </summary>
    public static class DataExtensions
    {
        /// <summary>
        /// Gets the summary.
        /// </summary>
        /// <param name="me">A given Blogstory.</param>
        /// <returns>Blogs Summary.</returns>
        public static string GetSummary(this BlogStory me)
        {
            var maxparagraphs = 2;
            var regex = new Regex("(<p[^>]*>.*?</p>)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var result = regex.Matches(me.Body);
            StringBuilder bldr = new StringBuilder();
            var x = 0;
            foreach (Match m in result)
            {
                x++;
                bldr.Append(m.Value);
                if (x == maxparagraphs)
                {
                    break;
                }
            }

            return bldr.ToString();

        }

        /// <summary>
        /// Gets the story URL.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <returns>Current Date for Slug.</returns>
        public static string GetStoryUrl(this BlogStory story)
        {
            return string.Format("{0:0000}/{1:00}/{2:00}/{3}", story.DatePublished.Year, story.DatePublished.Month, story.DatePublished.Day, GetUrlSafeTitle(story));
        }

        /// <summary>
        /// Gets the URL safe title.
        /// </summary>
        /// <param name="story">The story.</param>
        /// <returns>Save URL.</returns>

        public static string GetUrlSafeTitle(this BlogStory story)
        {
            // Characters to replace with underscore
            char[] replacements = @" ""'?*.,+&:;\/#".ToCharArray();

            string[] splits = story.Title.Split(replacements, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder bldr = new StringBuilder();
            foreach (string s in splits)
            {
                string rd = RemoveDiacritics(s);
                bldr.Append(rd);
                bldr.Append("-");
            }

            return bldr.ToString(0, bldr.Length - 1);
        }

        // https://stackoverflow.com/questions/7470997/replace-german-characters-umlauts-accents-with-english-equivalents
        private static string RemoveDiacritics(string s)
        {
            string normalizedString = s.Normalize(NormalizationForm.FormD);
            string wodia = string.Empty;

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    wodia += c;
                }
            }

            return wodia;
        }
    }
}
