using BingoX.ExceptionProviders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using BingoX.Helper;

namespace BingoX.AspNetCore.Extensions
{
    public class StandardExceptionFilterAttribute : ExceptionFilterAttribute
    {


        public override void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;
            var descriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            var apitype = descriptor.ControllerTypeInfo.GetAttribute<ApiControllerAttribute>();
            var fault = FaultExceptionProvider.Get(ex)?? FaultExceptionProvider.Unhandled;
            if (apitype == null)
            {

                context.HttpContext.Items.Add("Exception", fault.GetMessage(ex));
                context.HttpContext.Items.Add("ExceptionType", ex);
            }
            else
            {
                context.ExceptionHandled = true;
                switch (ex)
                {
                    case UnauthorizedException _:
                        context.Result = new UnauthorizedResult();

                        return;
                    case ForbiddenException _:
                        context.Result = new ForbidResult();

                        return;
                    case NotFoundEntityException _:
                        context.Result = new NotFoundResult();

                        return;
                    default:
                        {

                            context.Result = new JsonResult(new { Message = fault.GetMessage(ex) })
                            {
                                StatusCode = 400
                            };
                            break;
                        }
                }
            }



        }

    }
}
