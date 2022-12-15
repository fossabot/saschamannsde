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
