using System.Collections.Generic;

namespace MannsBlog.Data
{
  public class BlogResult
  {
    public IEnumerable<BlogStory> Stories = new List<BlogStory>();
    public int TotalResults;
    public int TotalPages;
    public int CurrentPage;
  }
}