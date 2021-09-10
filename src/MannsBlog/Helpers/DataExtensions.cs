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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MannsBlog.Data;

namespace MannsBlog.Helpers
{
    public static class DataExtensions
    {

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
                if (x == maxparagraphs) break;
            }
            return bldr.ToString();

        }

        public static string GetStoryUrl(this BlogStory story)
        {
            return string.Format("{0:0000}/{1:00}/{2:00}/{3}", story.DatePublished.Year, story.DatePublished.Month, story.DatePublished.Day, GetUrlSafeTitle(story));
        }

        //public static Uri GetStoryFullUri(this BlogStory story, HttpRequest request)
        //{
        //  return new Uri(new Uri(request.GetDisplayUrl()), story.GetStoryUrl());
        //}

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
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            String wodia = String.Empty;

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    wodia += c;
            }
            return wodia;
        }
    }
}
