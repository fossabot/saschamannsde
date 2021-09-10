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
  public class PodcastEpisodesProvider : DataProvider<PodcastEpisode>
  {
    public PodcastEpisodesProvider(IHostEnvironment env)
      : base(env, "episodeList.json")
    {
    }

    public override IEnumerable<PodcastEpisode> Get()
    {
      return base.Get().OrderByDescending(a => a.PublishedDate).ToList();
    }
  }

  public class PodcastEpisode
  {
    public string[] Blurb { get; set; } = new string[0];
    public int EpisodeNumber { get; set; }
    public string[] GuestBio { get; set; } = new string[0];
    public string GuestName { get; set; } = "";
    public string GuestHeadshot { get; set; } = "";
    public string GuestFirstMachineImage { get; set; } = "";
    public string GuestFirstMachineLink { get; set; } = "";
    public string GuestFirstMachineName { get; set; } = "";
    public ICollection<GuestLink> GuestLinks { get; set; } = new List<GuestLink>();
    public string PodcastName { get; set; } = "";
    public string AudioLink { get; set; } = "";
    public DateTime PublishedDate { get; set; }
    public string YouTubeLink { get; set; } = "";
    public PodcastEpisodeStatus Status { get; set; }
  }

  public enum PodcastEpisodeStatus
  {
    Planned,
    Live,
    Staged
  }

  public class GuestLink
  {
    public string Name { get; set; } = "";
    public string Link { get; set; } = "";
  }
}
