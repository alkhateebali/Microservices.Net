﻿using Asp.Versioning.Builder;

namespace Microservice.Core.EndPoints;

public interface IEndpoint
{
    static abstract void MapEndpoint(IEndpointRouteBuilder endpoints);

}