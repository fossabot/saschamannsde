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

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MannsBlog.Services
{
    /// <summary>
    /// Interface for the MailService.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends the mail sendgrid asynchronous.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="attachment">The Attachment.</param>
        /// <returns>True or false depending on sending email success.</returns>
        Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg, [Optional] IFormFile attachment);
    }
}
