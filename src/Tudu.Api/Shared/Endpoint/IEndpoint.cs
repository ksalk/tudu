namespace Tudu.Api.Shared.Endpoint
{
    public interface IEndpoint
    {
        IEndpointRouteBuilder Map(IEndpointRouteBuilder routeBuilder);
    }
}
