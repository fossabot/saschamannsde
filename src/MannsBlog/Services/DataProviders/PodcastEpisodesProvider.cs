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
