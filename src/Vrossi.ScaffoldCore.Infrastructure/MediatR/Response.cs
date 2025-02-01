using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vrossi.ScaffoldCore.Infrastructure.MediatR
{
    public sealed class Response<TDto> : Response
       where TDto : class
    {
        private Response(TDto data)
        {
            Data = data;
        }

        public TDto Data { get; }
        private Response(string errorMessage) : base(errorMessage) { }
        public static Response<TDto> Map(TDto data)
        {
            if (data == null)
                return Error("Not found");

            return Success(data);
        }
        public static Response<TDto> Success(TDto data) => new(data);
        public static new Response<TDto> Error(string errorMessage) => new(errorMessage);
        public static Response<TDto> NotFound() => new("Resource not found") { IsNotFound = true };
        public static Response<TDto> EntityNotFound(string errorMessage) => new(errorMessage) { IsNotFound = true };
        public static new Response<TDto> Error(Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Result returned Success, therefore is not possible to return Error");

            return Error(result.Error);
        }
    }
    public class Response
    {
        private const string NOT_FOUND_MESSAGE = "Resource not found";
        protected Response() { }

        public Response(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));

            _errors.Add(errorMessage);
        }

        protected readonly List<string> _errors = new();
        public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();
        public bool IsSuccess => !Errors.Any();
        public bool IsNotFound { get; protected set; } = false;
        public string ErrorMessage => string.Join(";", Errors);

        public static Response Success() => new();

        public static Response Error(string errorMessage) => new(errorMessage);

        public static Response Error(Result result) => Error(result.Error);

        public static Response NotFound(string errorMessage = "") =>
            new(string.IsNullOrEmpty(errorMessage) ? NOT_FOUND_MESSAGE : errorMessage)
            {
                IsNotFound = true
            };

        public void AddErrors(IEnumerable<string> errors)
        {
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    if (!string.IsNullOrEmpty(error))
                        _errors.Add(error);
                }
            }
        }

        public bool HasErrors => _errors.Any();
    }
}
