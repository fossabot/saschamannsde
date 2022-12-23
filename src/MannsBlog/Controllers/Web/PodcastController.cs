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
