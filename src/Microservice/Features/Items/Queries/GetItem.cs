using System.Text.Json;
using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
using Microservice.Persistence.Repositories;
#if redis
using Microsoft.Extensions.Caching.Distributed;
#endif
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
    public class Handler(IAppLogger<CreateItem.Handler> logger
        ,IRepositoryBase<Item> itemRepository
#if redis
        ,IDistributedCache distributedCache
#endif
        ) : IRequestHandler<GetItem, Item?>
    {
        public async Task<Item?> Handle(GetItem request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling GetItem query for ID: {ItemId}", request.Id);

#if redis
                var cacheKey = $"item-{request.Id}";
                var cachedItem = await distributedCache.GetStringAsync(cacheKey, cancellationToken);

                if (!string.IsNullOrEmpty(cachedItem))
                {
                    logger.LogInformation("Item with ID: {ItemId} retrieved from cache", request.Id);
                    return JsonSerializer.Deserialize<Item>(cachedItem);
                }
#endif
            var item = await itemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (item is null)
            {
                logger.LogWarning("Item with ID: {ItemId} was not found", request.Id);
                return null;
            }

#if redis
                logger.LogInformation("Caching item with ID: {ItemId}", request.Id);
                cachedItem = JsonSerializer.Serialize(item);
                await distributedCache.SetStringAsync($"item-{request.Id}", cachedItem, cancellationToken);
            
#endif
            logger.LogInformation("Item with ID: {ItemId} retrieved successfully", request.Id);

            return item;
        }
    }
    }
