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
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MannsBlog.Config
{
  public class AppSettings
  {

    public MannsDbSettings MannsDb { get; set; } = new();
    public ApplicationInsightsSettings ApplicationInsights { get; set; } = new();
    public ExceptionsSettings Exceptions { get; set; } = new();
    public DisqusSettings Disqus { get; set; } = new();
    public MailServiceSettings MailService { get; set; } = new();
    public MetaWeblogSettings MetaWeblog { get; set; } = new();
    public AdServiceSettings AdService { get; set; } = new();
    public GoogleSettings Google { get; set; } = new();
    public BlobStorageSettings BlobStorage { get; set; } = new();


    public class MannsDbSettings
    {
      public string OldConnectionString { get; set; } = "";
      public string ConnectionString { get; set; } = "";
      public bool TestData { get; set; }
    }

    public class ApplicationInsightsSettings
    {
      public string InstrumentationKey { get; set; } = "";
    }

    public class ExceptionsSettings
    {
      public bool TestEmailExceptions { get; set; }
    }

    public class DisqusSettings
    {
      public string BlogName { get; set; } = "";
    }

    public class MailServiceSettings
    {
      public string ApiKey { get; set; } = "";
      public string ApiUser { get; set; } = "";
      public string Receiver { get; set; } = "";
      public bool TestInDev { get; set; }
    }

    public class MetaWeblogSettings
    {
      public string StoragePath { get; set; } = "";
    }

    public class AdServiceSettings
    {
      public string Client { get; set; } = "";
      public string Slot { get; set; } = "";
    }

    public class GoogleSettings
    {
      public string Analytics { get; set; } = "";
      public string CaptchaSecret { get; set; } = "";
      public string CaptchaSiteId { get; set; } = "";
    }

    public class BlobStorageSettings
    {
      public string Account { get; set; } = "";
      public string Key { get; set; } = "";
      public string StorageUrl { get; set; } = "";
    }
  }
}
