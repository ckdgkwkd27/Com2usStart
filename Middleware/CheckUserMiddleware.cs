using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace com2us_start.Middleware
{
    public class CheckUserMiddleware
    {
        ILogger Logger;
        private readonly RequestDelegate _requestDelegate;

        public CheckUserMiddleware(RequestDelegate requestDelegate, ILoggerFactory loggerFactory)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/Login") {
                StreamReader bodystream = new StreamReader(context.Request.Body, Encoding.UTF8);
                string body = bodystream.ReadToEndAsync().Result;

                var obj = (JObject)JsonConvert.DeserializeObject(body);
                var userID = (string)obj["ID"];
                if(string.IsNullOrEmpty(userID))
                {
                    return;
                }

                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body)); 
            }

            await _requestDelegate(context);
        }
    }
}
