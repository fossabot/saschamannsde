using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MannsBlog.Models
{
  public class SiteVerifyResult
  {
    public bool Success { get; set; }
    public string ChallengeTs { get; set; } = "";
    public string Hostname { get; set; } = "";
    public List<string> ErrorCodes { get; set; } = new();
  }
}
