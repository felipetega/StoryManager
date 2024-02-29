using API.Services.Requests;
using FluentValidation;

namespace API.Services.Handler
{
    public class UpdateStoryRequestValidator : AbstractValidator<UpdateStoryRequest>
    {
        public UpdateStoryRequestValidator()
        {
            RuleFor(request => request.Id).GreaterThan(0).WithMessage("Id should be greater than 0");
            RuleFor(request => request.Title).NotEmpty().WithMessage("Title cannot be empty");
            RuleFor(request => request.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(request => request.Department).NotEmpty().WithMessage("Department cannot be empty");
        }
    }
}
