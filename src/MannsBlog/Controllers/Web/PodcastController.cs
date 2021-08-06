using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MannsBlog.Data;
using MannsBlog.Services.DataProviders;

namespace MannsBlog.Controllers.Web
{
  [Route("hwpod")]
  public class PodcastController : Controller
  {
    private readonly PodcastEpisodesProvider _podcastProvider;

    public PodcastController(PodcastEpisodesProvider podcastProvider)
    {
      _podcastProvider = podcastProvider;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
      var episodes = _podcastProvider.Get();
      var latest = _podcastProvider.Get().Where(e => e.Status == PodcastEpisodeStatus.Live &&
                                       e.PublishedDate.AddHours(14) <= DateTime.UtcNow)
                           .OrderByDescending(e => e.EpisodeNumber)
                           .First();

      return View(Tuple.Create<PodcastEpisode, IEnumerable<PodcastEpisode>>(latest, episodes));
    }

    [HttpGet("{id:int}/{tag}")]
    public IActionResult Episode(int id, string tag)
    {
      var episode = _podcastProvider
          .Get()
          .FirstOrDefault(e => e.Status == PodcastEpisodeStatus.Live && 
                               e.EpisodeNumber == id);

      if (episode == null) return RedirectToAction("Index");

      return View(episode);
    }
  }
}
