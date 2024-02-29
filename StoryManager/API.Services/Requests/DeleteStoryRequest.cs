using FluentValidation;
using MediatR;

namespace API.Services.Handler
{
    public class DeleteStoryRequest : IRequest<bool>
    {
        public int Id { get; set; }
    }
}
