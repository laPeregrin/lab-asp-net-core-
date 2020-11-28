using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Reflection;

namespace WebView.ActionFilters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            Log(GetCurrentName((Action<ActionExecutingContext>)OnActionExecuting), context.RouteData);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Log(GetCurrentName((Action<ActionExecutedContext>)OnActionExecuted), context.RouteData);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Log(GetCurrentName((Action<ResultExecutingContext>)OnResultExecuting), context.RouteData);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Log(GetCurrentName((Action<ResultExecutedContext>)OnResultExecuted), context.RouteData);
        }



        public void Log(string MethodName, RouteData routedData)
        {
            var controllerName = routedData.Values["controller"];
            var actionName = routedData.Values["action"];
            var message = string.Format("{0} controller:{1} action:{2}", MethodName, controllerName, actionName);
            Debug.WriteLine(message);
        }
        public string GetCurrentName<T>(Action<T> action) where T : FilterContext
        {
            string name = action?.GetMethodInfo().Name;
            return name;
        }
       
    }
}