namespace ShiftManagement.Web.Filters
{
    using AutoMapper;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class MappingResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction?.Value == null 
            || resultFromAction.StatusCode < 200
            || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }
            //var mapper = context.HttpContext.RequestServices.GetService(typeof(IMapper)) as IMapper; 
            //resultFromAction.Value = mapper.Map<T>(resultFromAction.Value);

            await next();
        }
    }    
}
