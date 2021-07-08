using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace aspnet_demo_server.Filters
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new StatusCodeResult(500);
                context.ExceptionHandled = true;
                Log.Error(context.Exception, "{@Exception}", context.Exception.Message);
            }
        }
    }
}
