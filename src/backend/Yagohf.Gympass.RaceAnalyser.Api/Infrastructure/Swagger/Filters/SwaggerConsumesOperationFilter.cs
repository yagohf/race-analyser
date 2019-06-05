using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Swagger.Attributes;

namespace Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Swagger.Filters
{
    public class SwaggerConsumesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attribute = context.ApiDescription.ActionAttributes().SingleOrDefault(x => x.GetType() == typeof(SwaggerConsumesAttribute));
            if (attribute == null)
            {
                return;
            }
            else
            {
                operation.Consumes.Clear();
                operation.Consumes = (attribute as SwaggerConsumesAttribute).ContentTypes.ToList();
            }
        }
    }
}
