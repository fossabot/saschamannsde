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
using System.ComponentModel.DataAnnotations;

namespace MannsBlog.Models
{
    /// <summary>
    /// Model for the Contact Form.
    /// </summary>
    public class ContactFormModel
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [StringLength(4096, ErrorMessage = "Your message is too long. Please shorten it to max. 4096 chars.")]
        [MinLength(5)]
        [Required]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [Required]
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the recaptcha.
        /// </summary>
        /// <value>
        /// The recaptcha.
        /// </value>
        public string Recaptcha { get; set; } = string.Empty;
    }
}