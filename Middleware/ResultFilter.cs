using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace com2us_start.Middleware;

public class ResultFilterChangeResponse : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        ObjectResult objRet = (ObjectResult) context.Result;
        objRet.Value = "OnResult Executed Success";
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        
    }
}