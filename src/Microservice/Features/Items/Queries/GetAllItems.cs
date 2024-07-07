using System.Diagnostics;
using MediatR;
using Microservice.Core.EndPoints;
using Microservice.Core.Logging;
using Microservice.Features.Items.Commands;
using Microservice.Features.Items.Domain;
using Microservice.Persistence.Repositories;

namespace Microservice.Features.Items.Queries;

public class GetAllItems : IEndpoint, IRequest<IEnumerable<Item>>
{
    //  add parameters or filters here if needed

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        Debug.Assert(ApiVersionsConfig.VersionSet != null, "ApiVersionsConfig.VersionSet != null");
        endpoints.MapGet("/api/items",
                    async (IMediator mediator, CancellationToken cancellationToken) =>
                    {
                        var result = await mediator.Send(new GetAllItems(), cancellationToken);
                        return Results.Ok(result);
                    })
                .WithTags("Items")
                .Produces<IEnumerable<Item>>()
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithApiVersionSet(ApiVersionsConfig.VersionSet)
                .MapToApiVersion(ApiVersionsConfig.GetVersion(1, 0));
    }

    public class Handler(IAppLogger<CreateItem.Handler> logger, 
        IRepositoryBase<Item> itemRepository)
        :IRequestHandler<GetAllItems, IEnumerable<Item>>

    {
        public async Task<IEnumerable<Item>> Handle(GetAllItems request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Fetching all items...");

                var registrations = await itemRepository.GetAllAsync(cancellationToken);

                var enumerable = registrations.ToList();
                logger.LogInformation("Fetched {Count} items", enumerable.Count());

                return enumerable;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching items");
                throw;
            }
        }


    }
}