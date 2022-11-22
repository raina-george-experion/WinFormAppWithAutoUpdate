using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MyWebApplication
{
    public class MimeMultipart : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(
                        HttpStatusCode.UnsupportedMediaType)
                );
            }
        }
    }
}
