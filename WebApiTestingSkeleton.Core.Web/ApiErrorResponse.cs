using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace WebApiTestingSkeleton.Core.Web
{
    public class ApiErrorResponse
    {
        [JsonConstructor]
        public ApiErrorResponse(IEnumerable<string> errors)
        {
            Errors = ImmutableArray.CreateRange(errors ?? Enumerable.Empty<string>());
        }

        public IReadOnlyCollection<string> Errors { get; }
    }

    public class ApiErrorResponseFactory
    {
        public static ApiErrorResponse Build(ModelStateDictionary modelState)
        {
            var errors = modelState
                .SelectMany(entry => entry.Value.Errors)
                .Select(error => error.ErrorMessage);
            var result = new ApiErrorResponse(errors);
            return result;
        }
    }
}
