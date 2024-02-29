using System.Threading;
using System.Threading.Tasks;
using API.Services.DTOs;
using API.Services.Services.Interfaces;
using MediatR;

namespace API.Services.Handler
{
    public class CreateVoteHandler : IRequestHandler<CreateVoteRequest, bool>
    {
        private readonly IVoteService _voteService;

        public CreateVoteHandler(IVoteService voteService)
        {
            _voteService = voteService;
        }

        public async Task<bool> Handle(CreateVoteRequest request, CancellationToken cancellationToken)
        {
            // Fail Fast Validations
            if (request.UserId <= 0 || request.StoryId <= 0 || request.VoteValue is string)
            {
                return false;
            }

            VoteDTO voteDTO = new VoteDTO
            {
                StoryId = request.StoryId,
                UserId = request.UserId,
                VoteValue = request.VoteValue,
            };

            return await _voteService.Create(voteDTO);
        }
    }
}
