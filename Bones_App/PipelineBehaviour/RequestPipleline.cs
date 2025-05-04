using FluentValidation;
using MediatR;

namespace Bones_App.PipelineBehaviour
{

    public class RequestPipeline<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validator;

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var Context = new ValidationContext<TRequest>(request);
            var Failure = validator.Select(val => val.Validate(Context)).SelectMany(error => error.Errors).Where(errors => errors != null).ToList();
            if (Failure.Any())
            {
                throw new ValidationException(Failure);
            }

            return next();
        }
    }
}
