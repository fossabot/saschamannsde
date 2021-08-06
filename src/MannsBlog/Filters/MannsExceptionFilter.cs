using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace MannsBlog.Filters
{
  public class MannsExceptionFilter : ExceptionFilterAttribute
  {
    private readonly IHostEnvironment _hostingEnvironment;

    public MannsExceptionFilter(IHostEnvironment hostingEnvironment)
    {
      _hostingEnvironment = hostingEnvironment;
    }

    public override void OnException(ExceptionContext context)
    {
      if (_hostingEnvironment.IsDevelopment())
      {
        return;
      }
      
      
    }
  }
}
