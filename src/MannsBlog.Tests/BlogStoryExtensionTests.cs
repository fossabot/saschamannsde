﻿// Copyright (C) 2021 Sascha Manns <Sascha.Manns@outlook.de>
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
using MannsBlog.EntityFramework.Entities;
using MannsBlog.Helpers;
using Xunit;

namespace MannsBlog.Tests
{
    public class BlogStoryExtensionTests
    {
        BlogStory _story = new BlogStory()
        {
            DatePublished = new DateTime(2018, 5, 5),
            Title = "This is a test post",
            Body = @"<p>One</p><p>Two</p><p>Three</p><p>Four</p><p>Five</p>"
        };

        BlogStory _unsafeStory = new BlogStory()
        {
            DatePublished = new DateTime(2018, 5, 5),
            Title = @"Are favorite's part of this title, or is it wierd?",
            Body = @"<p><a href=""http://devintersection.com""><img title=""Print"" style=""border-top: 0px; border-right: 0px; background-image: none; border-bottom: 0px; float: right; padding-top: 0px; padding-left: 0px; border-left: 0px; display: inline; padding-right: 0px"" border=""0"" alt=""Print"" src=""http://Mannsmuth.com/images/DevIntersection_3.png"" width=""240"" align=""right"" height=""81""></a>I want to thank all the great attendees I met at this week’s DEVIntersection (Fall 2015) conference in Vegas! Richard Campbell and company put on a great show!</p> <p>I had the opportunity to do three talks and two of them went well (if you were at my Bootstrap talk, you know what I’m talking about). In any case, I wanted to share the slide and code with the attendees so here it is:</p> <h3>API Design</h3> <blockquote> <p><a href=""http://Mannsmuth.com/downloads/devintersection-fall2015-APIDesign.pdf"" target=""_blank"">Slides</a></p></blockquote> <h3>Looking Ahead to Bootstrap 4</h3> <blockquote> <p><a href=""http://Mannsmuth.com/downloads/devintersection-fall2015-BS4-slides.pdf"" target=""_blank"">Slides</a></p> <p><a href=""http://Mannsmuth.com/downloads/devintersection-fall2015-BS4-demo.zip"" target=""_blank"">Demos</a></p></blockquote> <p> <h3>Entities or&nbsp; View Models in ASP.NET Development</h3></p> <blockquote> <p><a href=""http://Mannsmuth.com/downloads/devintersection-fall2015-ViewModels.pdf"" target=""_blank"">Slides</a></p> <p><a href=""http://Mannsmuth.com/downloads/devintersection-fall2015-ViewModels-demo.zip"" target=""_blank"">Demos</a></p></blockquote> <p>Let me know if you attended the talk and how you liked them!</p>"
        };

        [Fact]
        public void TestSafeUrlTitle()
        {
            Assert.Contains("Are-favorite-s-part-of-this-title-or-is-it-wierd", _unsafeStory.GetUrlSafeTitle(), StringComparison.CurrentCultureIgnoreCase);
        }

        [Fact]
        public void TestStoryUrl()
        {
            Assert.Contains("2018/05/05/This-is-a-test-post", _story.GetStoryUrl(), StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains("Are-favorite-s-part-of-this-title-or-is-it-wierd", _unsafeStory.GetStoryUrl(), StringComparison.CurrentCultureIgnoreCase);
            Assert.Contains(_story.GetUrlSafeTitle(), _story.GetStoryUrl(), StringComparison.CurrentCultureIgnoreCase);
        }

        [Fact]
        public void TestSummary()
        {
            Assert.DoesNotContain("Five", _story.GetSummary());
            Assert.DoesNotContain("Let me know", _unsafeStory.GetSummary());
        }

    }
}
