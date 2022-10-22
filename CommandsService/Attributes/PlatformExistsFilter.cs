using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class PlatformExistsFilter : IActionFilter
{
    private readonly ICommandRepo _repository;
    public PlatformExistsFilter(ICommandRepo repo)
    {
        this._repository = repo;

    }

    public void OnActionExecuted(ActionExecutedContext context)
    {

    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var platformId = Convert.ToInt32(
            context.ActionArguments["platformId"]);
        if (!_repository.PlatformExists(platformId))
            context.Result = new NotFoundResult();
    }
}