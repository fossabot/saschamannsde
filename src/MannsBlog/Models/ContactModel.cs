﻿namespace MannsBlog.Models
{
  public class ContactModel
  {
    public string Email { get; set; } = "";
    public string Msg { get; set; } = "";
    public string Name { get; set; } = "";
    public string Subject { get; set; } = "";
    public string Recaptcha { get; set; } = "";
  }
}