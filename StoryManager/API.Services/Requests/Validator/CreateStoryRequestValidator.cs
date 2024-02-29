using API.Services.Requests;
using FluentValidation;

namespace API.Services.Handler
{
    public class CreateStoryRequestValidator : AbstractValidator<CreateStoryRequest>
    {
        public CreateStoryRequestValidator()
        {
            RuleFor(request => request.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(request => request.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(request => request.Department).NotEmpty().WithMessage("Department cannot be empty");
        }
    }
}
