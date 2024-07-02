using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Features.Items.Domain;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Commands;
    public class UpdateItem : IEndpoint
    {
        public static void MapEndpoint(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapPut("/api/v{version:apiVersion}/items/{id:Guid}",
                async (Guid id, Command command, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    if (id != command.Id)
                    {
                        return Results.BadRequest();
                    }

                    await mediator.Send(command, cancellationToken);
                    return Results.NoContent();
                })
                .WithTags("Items")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithApiVersionSet(ApiVersionsConfig.VersionSet)
                .MapToApiVersion(ApiVersionsConfig.GetVersion(1, 0));

        }

        public record Command(Guid Id, string Name) : IRequest;

        public class Handler(IAppLogger<Handler> logger, IRepositoryBase<Item> itemRepository)
            : IRequestHandler<Command>
        {
            private readonly IAppLogger<Handler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            private readonly IRepositoryBase<Item> _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    _logger.LogInformation("Updating item with ID: {ItemId}", request.Id);

                    var item = await _itemRepository.GetByIdAsync( request.Id, cancellationToken);

                    if (item == null)
                    {
                        _logger.LogWarning("Items with ID: {ItemId} not found", request.Id);
                        throw new KeyNotFoundException($"Items with ID {request.Id} not found.");
                    }

                    item.Name = request.Name;
                    // Update other properties as needed

                    await _itemRepository.UpdateAsync(item);

                    _logger.LogInformation("Items with ID: {ItemId} updated successfully", request.Id);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating the item with ID: {ItemId}", request.Id);
                    throw;
                }
            }
        }
    }

