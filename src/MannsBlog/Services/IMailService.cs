using System.Threading.Tasks;

namespace MannsBlog.Services
{
  public interface IMailService
  {
    Task<bool> SendMailAsync(string template, string name, string email, string subject, string msg);
  }
}