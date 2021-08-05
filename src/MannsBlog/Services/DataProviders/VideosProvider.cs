using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Services.DataProviders
{
  public class VideosProvider : DataProvider<Video>
  {
    public VideosProvider(IHostEnvironment env) 
      : base(env, "videos.json")
    {
    }

    public override IEnumerable<Video> Get()
    {
        string culture = CultureInfo.CurrentCulture.Name;
        return base.Get().Where(t => t.Language == culture).OrderByDescending(p => p.DatePublished).ToList();
    }
  }

  public enum VideoType
  {
    Unknown = 0,
    YouTube,
    Channel9,
    Vimeo
  }

  public class Video
  {
    public string? Title { get; set; }
    public int Id { get; set; }
    public string? Description { get; set; }
    public string? VideoCode { get; set; }
    public VideoType VideoType { get; set; }
    public DateTime DatePublished { get; set; }
    public string? Language { get; set; }
  }
}
