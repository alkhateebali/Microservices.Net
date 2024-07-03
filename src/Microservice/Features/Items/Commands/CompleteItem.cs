using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Exceptions;
using Microservice.Features.Items.Domain;
using Microservice.Features.Items.Queries;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Commands;

public  class CompleteItem(Guid id):IEndpoint, IRequest<Unit>

{
    private Guid Id { get; set; } = id;

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/items/{id:Guid}/complete", async (Guid id, IMediator mediator, CancellationToken cancellationToken) =>
            {
                var result = await mediator.Send(new GetItem(id), cancellationToken);
                return Results.NoContent();
            })
            .WithTags("Items")
            .Produces<Item>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithApiVersionSet(ApiVersionsConfig.VersionSet ?? throw new InvalidOperationException())
            .MapToApiVersion(ApiVersionsConfig.GetVersion(1, 0));

    }
    public class Handler(IRepositoryBase<Item> itemRepository) : IRequestHandler<CompleteItem, Unit>
    {
        public async Task<Unit> Handle(CompleteItem request, CancellationToken cancellationToken)
        {
            var item= await itemRepository.GetByIdAsync(request.Id, cancellationToken);
            if (item == null) throw new NotFoundException(nameof(Item), request.Id);
            item.Complete();
            return Unit.Value;
        }
    }
}