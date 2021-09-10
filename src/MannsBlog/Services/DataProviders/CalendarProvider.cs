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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
  public class CalendarProvider : DataProvider<CalendarEntry>
  {
    public CalendarProvider(IHostEnvironment env)
      : base(env, "calendar.json")
    {
    }

    public override IEnumerable<CalendarEntry> Get()
    {
      return base.Get().OrderBy(a => a.EventDate).ToList();
    }
  }

  public class CalendarEntry
  {
    public string EventName { get; set; } = "";
    public DateTime EventDate { get; set; }
    public int Length { get; set; }
    public string Link { get; set; } = "";
    public string Location { get; set; } = "";
    public string Note { get; set; } = "";
    public string Logo { get; set; } = "";
    public bool ReverseLogo { get; set; } = false;

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
