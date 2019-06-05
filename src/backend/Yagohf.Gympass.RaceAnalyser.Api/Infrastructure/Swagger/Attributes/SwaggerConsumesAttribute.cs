using System;
using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Api.Infrastructure.Swagger.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerConsumesAttribute : Attribute
    {
        public SwaggerConsumesAttribute(params string[] contentTypes)
        {
            this.ContentTypes = contentTypes;
        }

        public IEnumerable<string> ContentTypes { get; }
    }
}
