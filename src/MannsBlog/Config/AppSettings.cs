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

namespace MannsBlog.Config
{
    /// <summary>
    /// App Settings Class.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Gets or sets the ad service.
        /// </summary>
        /// <value>
        /// The ad service.
        /// </value>
        public AdServiceSettings AdService { get; set; } = new();

        /// <summary>
        /// Gets or sets the application insights.
        /// </summary>
        /// <value>
        /// The application insights.
        /// </value>
        public ApplicationInsightsSettings ApplicationInsights { get; set; } = new();

        /// <summary>
        /// Gets or sets the BLOB storage.
        /// </summary>
        /// <value>
        /// The BLOB storage.
        /// </value>
        public BlobStorageSettings BlobStorage { get; set; } = new();

        /// <summary>
        /// Gets or sets the blog.
        /// </summary>
        /// <value>
        /// The blog.
        /// </value>
        public BlogSettings Blog { get; set; } = new();

        /// <summary>
        /// Gets or sets the disqus.
        /// </summary>
        /// <value>
        /// The disqus.
        /// </value>
        public DisqusSettings Disqus { get; set; } = new();

        /// <summary>
        /// Gets or sets the exceptions.
        /// </summary>
        /// <value>
        /// The exceptions.
        /// </value>
        public ExceptionsSettings Exceptions { get; set; } = new();

        /// <summary>
        /// Gets or sets the google.
        /// </summary>
        /// <value>
        /// The google.
        /// </value>
        public GoogleSettings Google { get; set; } = new();

        /// <summary>
        /// Gets or sets the mail service.
        /// </summary>
        /// <value>
        /// The mail service.
        /// </value>
        public MailServiceSettings MailService { get; set; } = new();

        /// <summary>
        /// Gets or sets the manns database.
        /// </summary>
        /// <value>
        /// The manns database.
        /// </value>
        public MannsDbSettings MannsDb { get; set; } = new();

        /// <summary>
        /// Gets or sets the meta weblog.
        /// </summary>
        /// <value>
        /// The meta weblog.
        /// </value>
        public MetaWeblogSettings MetaWeblog { get; set; } = new();

        /// <summary>
        /// Gets or sets the outlook.
        /// </summary>
        /// <value>
        /// The outlook.
        /// </value>
        public OutlookSettings Outlook { get; set; } = new();

        /// <summary>
        /// Gets or sets the syncfusion.
        /// </summary>
        /// <value>
        /// The syncfusion.
        /// </value>
        public SyncfusionSettings Syncfusion { get; set; } = new();

        /// <summary>
        /// Settings for the Ad Service.
        /// </summary>
        public class AdServiceSettings
        {
            /// <summary>
            /// Gets or sets the client.
            /// </summary>
            /// <value>
            /// The client.
            /// </value>
            public string Client { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the slot.
            /// </summary>
            /// <value>
            /// The slot.
            /// </value>
            public string Slot { get; set; } = string.Empty;
        }

        /// <summary>
        /// Settings for ApplicationInsights.
        /// </summary>
        public class ApplicationInsightsSettings
        {
            /// <summary>
            /// Gets or sets the instrumentation key.
            /// </summary>
            /// <value>
            /// The instrumentation key.
            /// </value>
            public string ConnectionString { get; set; } = string.Empty;
        }

        /// <summary>
        /// Settings for the Blob Storage.
        /// </summary>
        public class BlobStorageSettings
        {
            /// <summary>
            /// Gets or sets the account.
            /// </summary>
            /// <value>
            /// The account.
            /// </value>
            public string Account { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>
            /// The key.
            /// </value>
            public string Key { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the storage URL.
            /// </summary>
            /// <value>
            /// The storage URL.
            /// </value>
            public string StorageUrl { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets a value indicating whether [test in dev].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [test in dev]; otherwise, <c>false</c>.
            /// </value>
            public bool TestInDev { get; set; }
        }

        /// <summary>
        /// Blog Settings.
        /// </summary>
        public class BlogSettings
        {
            /// <summary>
            /// Gets or sets the name of the user.
            /// </summary>
            /// <value>
            /// The name of the user.
            /// </value>
            public string? UserName { get; set; }

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            public string? Password { get; set; }

            /// <summary>
            /// Gets or sets the email.
            /// </summary>
            /// <value>
            /// The email.
            /// </value>
            public string? Email { get; set; }

            /// <summary>
            /// Gets or sets the use sendgrid.
            /// </summary>
            /// <value>
            /// The use sendgrid.
            /// </value>
            public bool? UseSendgrid { get; set; }

            /// <summary>
            /// Gets or sets the HTTP URL.
            /// </summary>
            /// <value>
            /// The HTTP URL.
            /// </value>
            public string? HTTPUrl { get; set; }

            /// <summary>
            /// Gets or sets the HTTPS URL.
            /// </summary>
            /// <value>
            /// The HTTPS URL.
            /// </value>
            public string? HTTPSUrl { get; set; }

            /// <summary>
            /// Use HTTPS? If false, we using http instead of https.
            /// </summary>
            public bool UseHttps { get; set; }

            /// <summary>
            /// Gets or sets the blog identifier.
            /// </summary>
            /// <value>
            /// The blog identifier.
            /// </value>
            public string BlogId { get; set; }

            /// <summary>
            /// Gets or sets the name of the blog.
            /// </summary>
            /// <value>
            /// The name of the blog.
            /// </value>
            public string BlogName { get; set; }

            /// <summary>
            /// Gets or sets the user firstname.
            /// </summary>
            /// <value>
            /// The user firstname.
            /// </value>
            public string UserFirstname { get; set; }

            /// <summary>
            /// Gets or sets the user surname.
            /// </summary>
            /// <value>
            /// The user surname.
            /// </value>
            public string UserSurname { get; set; }

            /// <summary>
            /// Gets or sets the blog description.
            /// </summary>
            /// <value>
            /// The blog description.
            /// </value>
            public string BlogDescription { get; set; }

            /// <summary>
            /// Gets or sets the blog keywords.
            /// </summary>
            /// <value>
            /// The blog keywords.
            /// </value>
            public string BlogKeywords { get; set; }
        }

        /// <summary>
        /// Disqus Comments Settings.
        /// </summary>
        public class DisqusSettings
        {
            /// <summary>
            /// Gets or sets the name of the blog.
            /// </summary>
            /// <value>
            /// The name of the blog.
            /// </value>
            public string BlogName { get; set; } = string.Empty;
        }

        /// <summary>
        /// Exception Settings.
        /// </summary>
        public class ExceptionsSettings
        {
            /// <summary>
            /// Gets or sets a value indicating whether [test email exceptions].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [test email exceptions]; otherwise, <c>false</c>.
            /// </value>
            public bool TestEmailExceptions { get; set; }
        }

        /// <summary>
        /// Settings for Google Stuff.
        /// </summary>
        public class GoogleSettings
        {
            /// <summary>
            /// Gets or sets the analytics.
            /// </summary>
            /// <value>
            /// The analytics.
            /// </value>
            public string Analytics { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the captcha secret.
            /// </summary>
            /// <value>
            /// The captcha secret.
            /// </value>
            public string CaptchaSecret { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the captcha site identifier.
            /// </summary>
            /// <value>
            /// The captcha site identifier.
            /// </value>
            public string CaptchaSiteId { get; set; } = string.Empty;
        }

        /// <summary>
        /// Settings for Mail Service.
        /// </summary>
        public class MailServiceSettings
        {
            /// <summary>
            /// Gets or sets the API key.
            /// </summary>
            /// <value>
            /// The API key.
            /// </value>
            public string ApiKey { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the API user.
            /// </summary>
            /// <value>
            /// The API user.
            /// </value>
            public string ApiUser { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the receiver.
            /// </summary>
            /// <value>
            /// The receiver.
            /// </value>
            public string Receiver { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets a value indicating whether [test in dev].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [test in dev]; otherwise, <c>false</c>.
            /// </value>
            public bool TestInDev { get; set; }
        }

        /// <summary>
        /// Database Settings.
        /// </summary>
        public class MannsDbSettings
        {
            /// <summary>
            /// Gets or sets the connection string.
            /// </summary>
            /// <value>
            /// The connection string.
            /// </value>
            public string ConnectionString { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets a value indicating whether [test data].
            /// </summary>
            /// <value>
            ///   <c>true</c> if [test data]; otherwise, <c>false</c>.
            /// </value>
            public bool TestData { get; set; }
        }

        /// <summary>
        /// Metaweblog Settings.
        /// </summary>
        public class MetaWeblogSettings
        {
            /// <summary>
            /// Gets or sets the storage path.
            /// </summary>
            /// <value>
            /// The storage path.
            /// </value>
            public string StoragePath { get; set; } = string.Empty;
        }

        /// <summary>
        /// Settings for Outloog Mailservice.
        /// </summary>
        public class OutlookSettings
        {
            /// <summary>
            /// Gets or sets the mailaddress.
            /// </summary>
            /// <value>
            /// The mailaddress.
            /// </value>
            public string Mailaddress { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the SMTP server.
            /// </summary>
            /// <value>
            /// The SMTP server.
            /// </value>
            public string SMTPServer { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the SMTP port.
            /// </summary>
            /// <value>
            /// The SMTP port.
            /// </value>
            public int SMTPPort { get; set; } = 0;

            /// <summary>
            /// Gets or sets the crypt.
            /// </summary>
            /// <value>
            /// The crypt.
            /// </value>
            public string Crypt { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the password.
            /// </summary>
            /// <value>
            /// The password.
            /// </value>
            public string Password { get; set; } = string.Empty;
        }

        /// <summary>
        /// Settings for Syncfusion.
        /// </summary>
        public class SyncfusionSettings
        {
            /// <summary>
            /// Gets or sets the license.
            /// </summary>
            /// <value>
            /// The license.
            /// </value>
            public string License { get; set; } = string.Empty;

            /// <summary>
            /// Gets or sets the version.
            /// </summary>
            /// <value>
            /// The version.
            /// </value>
            public string Version { get; set; } = string.Empty;
        }
    }
}
