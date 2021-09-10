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

namespace MannsBlog.Data
{
  public enum EpisodeStatus
  {
    Planned,
    Live,
    Staged
  }

  public class Episode
  {
    public string[] Blurb { get; set; } = new string[0];
    public int EpisodeNumber { get; set; }
    public string GuestName { get; set; } = "";
    public string[] GuestBio { get; set; } = new string[0];
    public string GuestHeadshot { get; set; } = "";
    public string GuestFirstMachine { get; set; } = "";
    public string GuestFirstMachineName { get; set; } = "";
    public string GuestFirstMachineLink { get; set; } = "";
    public EpisodeLink[] GuestLinks { get; set; } = new EpisodeLink[0];
    public string PodcastName { get; set; } = "";
    public string AudioLink { get; set; } = "";
    public DateTime PublishedDate { get; set; }
    public EpisodeStatus Status { get; set; }
    public TimeSpan Length { get; set; }
    public string YouTubeLink { get; set; } = "";

    public string Slug()
    {
      return string.Concat(EpisodeNumber, "_", GuestName.Replace(" ", "_"));
    }
  }
}