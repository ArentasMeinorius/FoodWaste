using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodWaste.ActionFilters
{
    public class LogMethod : IActionFilter
    {
        private readonly ILogger<LogMethod> _logger;
        public LogMethod(ILogger<LogMethod> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Completed: Page opened: {0}", context.ActionDescriptor.DisplayName);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Start: Opening page: {0}", context.ActionDescriptor.DisplayName);
        }
    }
}
