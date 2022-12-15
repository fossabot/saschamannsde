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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Helpers;
using MannsBlog.Models;
using MannsBlog.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WilderMinds.AzureImageStorageService;
using WilderMinds.MetaWeblog;

namespace MannsBlog.MetaWeblog
{
    /// <summary>
    /// Class for Metaweblog.
    /// </summary>
    /// <seealso cref="WilderMinds.MetaWeblog.IMetaWeblogProvider" />
    public class MannsWeblogProvider : IMetaWeblogProvider
    {
        private const string ContainerName = "img";
        private readonly UserManager<MannsUser> _userMgr;
        private readonly IAzureImageStorageService _imageService;
        private readonly ILogger<MannsWeblogProvider> _logger;
        private IMannsRepository _repo;
        private IHostEnvironment _appEnv;
        private IConfiguration _config;

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
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string? UserName { get; set; }

        /// <summary>
        /// Email Address.
        /// </summary>

        public string? Email { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MannsWeblogProvider"/> class.
        /// </summary>
        /// <param name="userMgr">The user MGR.</param>
        /// <param name="repo">The repo.</param>
        /// <param name="appEnv">The application env.</param>
        /// <param name="imageService">The image service.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="config">The Configuration.</param>
        public MannsWeblogProvider(
          UserManager<MannsUser> userMgr,
          IMannsRepository repo,
          IHostEnvironment appEnv,
          IAzureImageStorageService imageService,
          ILogger<MannsWeblogProvider> logger,
          IConfiguration config)
        {
            _repo = repo;
            _userMgr = userMgr;
            _appEnv = appEnv;
            _imageService = imageService;
            _logger = logger;
            _config = config;

            UserName = config.GetValue<string>("Blog:Username");
            HTTPSUrl = config.GetValue<string>("Blog:HTTPSUrl");
            HTTPUrl = config.GetValue<string>("Blog:HTTPUrl");
            UseHttps = config.GetValue<bool>("Blog:UseHttps");
            Email = config.GetValue<string>("Blog:Email");
            UserSurname = config.GetValue<string>("Blog:UserSurname");
            UserFirstname = config.GetValue<string>("Blog:UserFirstname");
        }

        /// <summary>
        /// Adds the post asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>New story ID.</returns>
        /// <exception cref="WilderMinds.MetaWeblog.MetaWeblogException">
        /// Failed to specify categories
        /// or
        /// Failed to save the post.
        /// </exception>
        public async Task<string> AddPostAsync(string blogid, string username, string password, Post post, bool publish)
        {
            await EnsureUser(username, password);

            if (post.categories == null)
            {
                throw new MetaWeblogException("Failed to specify categories");
            }

            var newStory = new BlogStory();
            try
            {
                newStory.Title = post.title;
                newStory.Body = post.description;
                newStory.DatePublished = post.dateCreated == DateTime.MinValue ? DateTime.UtcNow : post.dateCreated;
                if (post.categories != null)
                {
                    newStory.Categories = string.Join(",", post.categories);
                }

                newStory.IsPublished = publish;
                newStory.Slug = newStory.GetStoryUrl();
                newStory.UniqueId = newStory.Slug;
                newStory.FeatureImageUrl = post.wp_post_thumbnail;
                newStory.Abstract = newStory.GetSummary();

                _repo.AddStory(newStory);
                if (await _repo.SaveAllAsync())
                {
                    return newStory.Id.ToString();
                }
            }
            catch (Exception)
            {
                _logger.LogError("Failed to add new Post");
            }

            throw new MetaWeblogException("Failed to save the post.");
        }

        /// <summary>
        /// Edits the post asynchronous.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="post">The post.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>Bool what reflects the success.</returns>
        /// <exception cref="WilderMinds.MetaWeblog.MetaWeblogException">
        /// Failed to specify categories
        /// or
        /// Failed to edit the post.
        /// </exception>
        public async Task<bool> EditPostAsync(string postid, string username, string password, Post post, bool publish)
        {
            await EnsureUser(username, password);

            if (post.categories == null)
            {
                throw new MetaWeblogException("Failed to specify categories");
            }

            try
            {
                var story = await _repo.GetStory(int.Parse(postid));

                story.Title = post.title;
                story.Body = post.description;
                if (post.dateCreated == DateTime.MinValue)
                {
                    story.DatePublished = DateTime.UtcNow; // Only overwrite date if is empty
                }

                story.Categories = string.Join(",", post.categories);
                story.IsPublished = publish;
                if (string.IsNullOrWhiteSpace(story.Slug))
                {
                    story.Slug = story.GetStoryUrl(); // Only recalcuate Slug if absolutely necessary
                }

                story.FeatureImageUrl = post.wp_post_thumbnail;
                story.Abstract = story.GetSummary();

                if (await _repo.SaveAllAsync())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Failed to edit the post.");
            }

            throw new MetaWeblogException("Failed to edit the post.");
        }

        /// <summary>
        /// Gets the post asynchronous.
        /// </summary>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The new post.</returns>
        /// <exception cref="WilderMinds.MetaWeblog.MetaWeblogException">Failed to get the post.</exception>
        public async Task<Post> GetPostAsync(string postid, string username, string password)
        {
            await EnsureUser(username, password);

            try
            {
                var story = await _repo.GetStory(int.Parse(postid));
                var newPost = new Post()
                {
                    title = story.Title,
                    description = story.Body,
                    dateCreated = story.DatePublished,
                    categories = story.Categories.Split(','),
                    postid = story.Id,
                    userid = "saschamanns",
                    wp_slug = story.GetStoryUrl(),
                    wp_post_thumbnail = story.FeatureImageUrl,
                };

                return newPost;
            }
            catch (Exception)
            {
                throw new MetaWeblogException("Failed to get the post.");
            }
        }

        /// <summary>
        /// Creates new mediaobjectasync.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="mediaObject">The media object.</param>
        /// <returns>Objectinformation about the Media.</returns>
        /// <exception cref="WilderMinds.MetaWeblog.MetaWeblogException">Failed to upload Media Object.</exception>
        public async Task<MediaObjectInfo> NewMediaObjectAsync(string blogid, string username, string password, MediaObject mediaObject)
        {
            await EnsureUser(username, password);

            var bits = Convert.FromBase64String(mediaObject.bits);
            var result = await _imageService.StoreImage(ContainerName, mediaObject.name, bits);

            if (result.Success)
            {
                var url = result.ImageUrl;

                // Create the response
                MediaObjectInfo objectInfo = new MediaObjectInfo();
                objectInfo.url = url;

                return objectInfo;
            }

            throw new MetaWeblogException("Failed to upload Media Object");
        }

        /// <summary>
        /// Gets the categories asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>CategoryInfo Object.</returns>
        public async Task<CategoryInfo[]> GetCategoriesAsync(string blogid, string username, string password)
        {
            await EnsureUser(username, password);

            string myUrl;

            if (UseHttps)
            {
                myUrl = HTTPSUrl;
            }
            else
            {
                myUrl = HTTPSUrl;
            }

            return (await _repo.GetCategories())
              .Select(c => new CategoryInfo()
              {
                  categoryid = c,
                  title = c,
                  description = c,
                  htmlUrl = string.Concat(myUrl, "/tags/", c),
                  rssUrl = string.Empty,
              }).ToArray();

        }

        /// <summary>
        /// Gets the recent posts asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="numberOfPosts">The number of posts.</param>
        /// <returns>Recent Post.</returns>
        public async Task<Post[]> GetRecentPostsAsync(string blogid, string username, string password, int numberOfPosts)
        {
            await EnsureUser(username, password);

            string myUrl;

            if (UseHttps)
            {
                myUrl = HTTPSUrl;
            }
            else
            {
                myUrl = HTTPSUrl;
            }

            var result = (await _repo.GetStories(numberOfPosts)).Stories.Select(s =>
            {
                var summary = new HtmlDocument();
                summary.LoadHtml(s.GetSummary());

                return new Post()
                {
                    title = s.Title,
                    mt_excerpt = summary.DocumentNode.InnerText,
                    description = s.Title,
                    categories = s.Categories.Split(','),
                    dateCreated = s.DatePublished,
                    postid = s.Id,
                    permalink = string.Concat(myUrl, "/", s.GetStoryUrl()),
                    link = string.Concat(myUrl, "/", s.GetStoryUrl()),
                    wp_slug = s.Slug,
                    userid = "saschamanns",
                    wp_post_thumbnail = s.FeatureImageUrl,
                };
            }).ToArray();

            return result;
        }

        /// <summary>
        /// Deletes the post asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="postid">The postid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>Bool with explains, if the deletion was successful.</returns>
        public async Task<bool> DeletePostAsync(string key, string postid, string username, string password, bool publish)
        {
            await EnsureUser(username, password);

            try
            {
                var result = _repo.DeleteStory(postid);
                return await _repo.SaveAllAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the users blogs asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>BlogInfo Object.</returns>
        public async Task<BlogInfo[]> GetUsersBlogsAsync(string key, string username, string password)
        {
            await EnsureUser(username, password);

            var blog = new BlogInfo()
            {
                blogid = BlogId,
                blogName = BlogName,
                url = "/",
            };

            return new BlogInfo[] { blog };
        }

        /// <summary>
        /// Gets the user information asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>UserInfo object.</returns>
        public async Task<UserInfo> GetUserInfoAsync(string key, string username, string password)
        {
            await EnsureUser(username, password);

            string myUrl;

            if (UseHttps)
            {
                myUrl = HTTPSUrl;
            }
            else
            {
                myUrl = HTTPSUrl;
            }

            return new UserInfo()
            {
                email = Email,
                lastname = UserSurname,
                firstname = UserFirstname,
                userid = username,
                url = myUrl,
            };
        }

        /// <summary>
        /// Adds the category asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="category">The category.</param>
        /// <returns>Just integer 1.</returns>
        private async Task EnsureUser(string username, string password)
        {
            var user = await _userMgr.FindByNameAsync(username);
            if (user != null)
            {
                if (await _userMgr.CheckPasswordAsync(user, password))
                {
                    return;
                }
            }

            throw new MetaWeblogException("Authentication failed.");
        }

        /// <summary>
        /// Ensures the directory.
        /// </summary>
        /// <param name="dir">The dir.</param>
        private void EnsureDirectory(DirectoryInfo dir)
        {
            if (dir.Parent != null)
            {
                EnsureDirectory(dir.Parent);
            }

            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        /// <summary>
        /// Adds the category asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="category">The category.</param>
        /// <returns>Just integer 1.</returns>
        public async Task<int> AddCategoryAsync(string key, string username, string password, NewCategory category)
        {
            await EnsureUser(username, password);

            // We don't store these, just query them from the list of stories so don't do anything
            return 1;
        }

        /// <summary>
        /// Gets the page asynchronous. WordPress support, don't care so just implementing the interface.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="pageid">The pageid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<Page> GetPageAsync(string blogid, string pageid, string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the pages asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="numPages">The number pages.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<Page[]> GetPagesAsync(string blogid, string username, string password, int numPages)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the authors asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<Author[]> GetAuthorsAsync(string blogid, string username, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the page asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="page">The page.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<string> AddPageAsync(string blogid, string username, string password, Page page, bool publish)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Edits the page asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="pageid">The pageid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="page">The page.</param>
        /// <param name="publish">if set to <c>true</c> [publish].</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<bool> EditPageAsync(string blogid, string pageid, string username, string password, Page page, bool publish)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the page asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="pageid">The pageid.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<bool> DeletePageAsync(string blogid, string username, string password, string pageid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the tags asynchronous.
        /// </summary>
        /// <param name="blogid">The blogid.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="System.NotImplementedException">Not Implemented.</exception>
        public Task<Tag[]> GetTagsAsync(string blogid, string username, string password)
        {
            throw new NotImplementedException();
        }

    }
}