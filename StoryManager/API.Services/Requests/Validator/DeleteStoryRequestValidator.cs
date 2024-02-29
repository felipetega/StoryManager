using API.Services.Handler;
using FluentValidation;

namespace API.Services.Validators
{
    public class DeleteStoryRequestValidator : AbstractValidator<DeleteStoryRequest>
    {
        public DeleteStoryRequestValidator()
        {
            RuleFor(request => request.Id).GreaterThan(0).WithMessage("Id should be greater than 0");
        }
    }
}
