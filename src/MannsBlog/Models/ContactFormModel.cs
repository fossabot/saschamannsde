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
    public class ContactFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [StringLength(4096, ErrorMessage = "Your message is too long. Please shorten it to max. 4096 chars.")]
        [MinLength(5)]
        [Required]
        public string Message { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "Name is too long.")]
        public string Name { get; set; } = "";

        [Required]
        public string Subject { get; set; } = "";

        [Required]
        public string Recaptcha { get; set; } = "";
    }
}