using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Commands;

public abstract class CreateItem : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/items",
                async (Command command, IMediator mediator, CancellationToken cancellationToken) =>
                {
                    var result = await mediator.Send(command, cancellationToken);
                    return Results.Created($"/items/{result}", result);
                })
            .WithTags("Items")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public record Command(string Name):IRequest<Guid>;
    public class Handler(IAppLogger<Handler> logger, IRepositoryBase<Domain.Item> itemRepository)
        : IRequestHandler<Command, Guid>
    {
        private readonly IAppLogger<Handler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRepositoryBase<Domain.Item> _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository));


        public Task<Guid> Handle(Command request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating a new item with name: {Name}", request.Name);

            var item = new Domain.Item
            {
                Name = request.Name
                // Set other properties as needed
            };

             _itemRepository.CreateAsync(item);
            // await _itemRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Items created successfully with ID: {ItemId}", item.Id);

            return   Task.FromResult(item.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating the item with name: {Name}", request.Name);
            throw;
        }
    }

      
    }

}



