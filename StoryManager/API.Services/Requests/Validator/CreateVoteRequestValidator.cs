using FluentValidation;

namespace API.Services.Handler
{
    public class CreateVoteRequestValidator : AbstractValidator<CreateVoteRequest>
    {
        public CreateVoteRequestValidator()
        {
            RuleFor(request => request.StoryId).GreaterThan(0).WithMessage("StoryId should be greater than 0");
            RuleFor(request => request.UserId).GreaterThan(0).WithMessage("UserId should be greater than 0");
        }
    }
}
