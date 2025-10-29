using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Filters
{
    /// <summary>
    /// Filtro global para manejo de excepciones en la API
    /// </summary>
    public class BaseExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<BaseExceptionFilter> _logger;

        public BaseExceptionFilter(ILogger<BaseExceptionFilter> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Exception caught in BaseExceptionFilter: {Message}",
                context.Exception.Message);

            switch (context.Exception)
            {
                case InvalidEntityDataException invalidEntityEx:
                    _logger.LogWarning("Invalid entity data: {Errors}", invalidEntityEx.GetErrorMessages());
                    context.Result = new BadRequestObjectResult(new
                    {
                        success = false,
                        message = invalidEntityEx.GetErrorMessages(),
                        code = "INVALID_ENTITY_DATA",
                        statusCode = 400
                    });
                    context.ExceptionHandled = true;
                    break;

                case EntityDoesExistException entityExistsEx:
                    _logger.LogWarning("Entity already exists: {Message}", entityExistsEx.Message);
                    context.Result = new BadRequestObjectResult(new
                    {
                        success = false,
                        message = entityExistsEx.Message,
                        code = "ENTITY_ALREADY_EXISTS",
                        statusCode = 400
                    });
                    context.ExceptionHandled = true;
                    break;

                case EntityNotFoundException notFoundEx:
                    _logger.LogWarning("Entity not found: {Message}", notFoundEx.Message);
                    context.Result = new NotFoundObjectResult(new
                    {
                        success = false,
                        message = notFoundEx.Message,
                        code = "ENTITY_NOT_FOUND",
                        statusCode = 404
                    });
                    context.ExceptionHandled = true;
                    break;

                case BussinessException businessEx:
                    _logger.LogWarning("Business rule violation: {Message}", businessEx.Message);
                    context.Result = new BadRequestObjectResult(new
                    {
                        success = false,
                        message = businessEx.Message,
                        code = "BUSINESS_RULE_VIOLATION",
                        statusCode = 400
                    });
                    context.ExceptionHandled = true;
                    break;

                default:
                    _logger.LogError(context.Exception, "Unhandled exception: {Message}",
                        context.Exception.Message);
                    context.Result = new ObjectResult(new
                    {
                        success = false,
                        message = "Ocurrió un error interno en el servidor",
                        code = "INTERNAL_SERVER_ERROR",
                        statusCode = 500
                    })
                    {
                        StatusCode = 500
                    };
                    context.ExceptionHandled = true;
                    break;
            }
        }
    }
}