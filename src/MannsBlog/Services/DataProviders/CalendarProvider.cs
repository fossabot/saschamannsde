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
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
    /// <summary>
    /// Provider for Calendar data source.
    /// </summary>
    /// <seealso cref="MannsBlog.Services.DataProviders.DataProvider&lt;Saschamannsde.Services.DataProviders.CalendarEntry&gt;" />
    public class CalendarProvider : DataProvider<CalendarEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarProvider"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public CalendarProvider(IHostEnvironment env)
          : base(env, "calendar.json")
        {
        }

        /// <summary>
        /// Gets the calendar items.
        /// </summary>
        /// <returns>List of CalendarEntries.</returns>
        public override IEnumerable<CalendarEntry> Get()
        {
            return base.Get().OrderBy(a => a.EventDate).ToList();
        }
    }

    /// <summary>
    /// Calender Entry class.
    /// </summary>
    public class CalendarEntry
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        /// <value>
        /// The event date.
        /// </value>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>
        /// The link.
        /// </value>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string Logo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether [reverse logo].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reverse logo]; otherwise, <c>false</c>.
        /// </value>
        public bool ReverseLogo { get; set; } = false;

        /// <summary>
        /// Gets the formatted date.
        /// </summary>
        /// <value>
        /// The formatted date.
        /// </value>
        public string FormattedDate
        {
            get
            {
                if (Length > 1)
                {
                    var endDate = EventDate.AddDays(Length - 1);
                    if (endDate.Month == EventDate.Month)
                    {
                        return $"{EventDate:MMM} {EventDate.Day}-{endDate.Day}, {EventDate.Year}";
                    }
                    else
                    {
                        if (endDate.Year == EventDate.Year)
                        {
                            return string.Format("{0:MMM} {2}-{1:MMM} {3}, {4}", EventDate, endDate, EventDate.Day, endDate.Day, EventDate.Year);
                        }
                        else
                        {
                            return $"{EventDate:MMM d, YYYY}-{endDate:MMM d, YYYY}";
                        }
                    }
                }
                else
                {
                    return EventDate.ToString("MMM d, yyyy");
                }
            }
        }
    }
}
