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

namespace MannsBlog.Controllers.Web
{
    //[Route("hwpod")]
    //public class PodcastController : Controller
    //{
    //  private readonly PodcastEpisodesProvider _podcastProvider;

    //  public PodcastController(PodcastEpisodesProvider podcastProvider)
    //  {
    //    _podcastProvider = podcastProvider;
    //  }

    //  [HttpGet("")]
    //  public IActionResult Index()
    //  {
    //    var episodes = _podcastProvider.Get();
    //    var latest = _podcastProvider.Get().Where(e => e.Status == PodcastEpisodeStatus.Live &&
    //                                     e.PublishedDate.AddHours(14) <= DateTime.UtcNow)
    //                         .OrderByDescending(e => e.EpisodeNumber)
    //                         .First();

    //    return View(Tuple.Create<PodcastEpisode, IEnumerable<PodcastEpisode>>(latest, episodes));
    //  }

    //  [HttpGet("{id:int}/{tag}")]
    //  public IActionResult Episode(int id, string tag)
    //  {
    //    var episode = _podcastProvider
    //        .Get()
    //        .FirstOrDefault(e => e.Status == PodcastEpisodeStatus.Live && 
    //                             e.EpisodeNumber == id);

    //    if (episode == null) return RedirectToAction("Index");

    //    return View(episode);
    //  }
    //}
}
