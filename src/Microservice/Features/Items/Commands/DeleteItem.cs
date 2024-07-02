using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Exceptions;
using Microservice.Core.Logging;
using Microservice.Features.Items.Domain;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Commands;
public class DeleteItem :IEndpoint
    {
      
        public static void MapEndpoint(IEndpointRouteBuilder endpoints)
            {
                endpoints.MapDelete("/api/v{version:apiVersion}/items/{id:Guid}",
                        async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
                        {
                            await mediator.Send(new Command(id), cancellationToken);
                            return Results.NoContent();
                        })
                    .WithTags("Items")
                    .Produces(StatusCodes.Status204NoContent)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithApiVersionSet(ApiVersionsConfig.VersionSet)
                    .MapToApiVersion(ApiVersionsConfig.GetVersion(1, 0));

            }
     
        
        public record Command(Guid Id) :  IRequest;

        public class Handler(IAppLogger<Handler> logger, IRepositoryBase<Item> itemRepository)
            : IRequestHandler<Command>
        {
            private readonly IAppLogger<Handler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            private readonly IRepositoryBase<Item> _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Deleting item with ID: {ItemId}", request.Id);

                    var item = await _itemRepository.GetByIdAsync(request.Id, cancellationToken);

                    if (item == null)
                    {
                        _logger.LogWarning("Items with ID: {ItemId} not found", request.Id);
                        throw new NotFoundException($"Items with ID {request.Id} not found.");
                    }

                    await _itemRepository.DeleteAsync(item);

                    _logger.LogInformation("Items with ID: {ItemId} deleted successfully", request.Id);

                   
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while deleting the item with ID: {ItemId}", request.Id);
                    throw;
                }
            }
        }
    }

   

