using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Queries;

public class GetItem(Guid id) : IEndpoint, IRequest<Item>
{
    public Guid Id { get; set; } = id;

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/items/{id:Guid}", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetItem(id), cancellationToken);
                return Results.Ok(result);
            })
            .WithTags("Items")
            .Produces<Item>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
    public class Handler(IAppLogger<CreateItem.Handler> logger,IRepositoryBase<Item> itemRepository) : IRequestHandler<GetItem, Item?>
    {
        public async Task<Item?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetItem query for ID: {ItemId}", request.Id);

            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);

            if (item == null)
            {
                logger.LogWarning("Item with ID: {ItemId} was not found", request.Id);
            }
            else
            {
                logger.LogInformation("Item with ID: {ItemId} retrieved successfully", request.Id);
            }

            return item;
        }
    }
}