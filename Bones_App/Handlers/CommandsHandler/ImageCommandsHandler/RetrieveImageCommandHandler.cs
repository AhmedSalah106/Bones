using Bones_App.Commands.ImageCommand;
using Bones_App.DTOs;
using Bones_App.Helpers;
using MediatR;

namespace Bones_App.Handlers.CommandsHandler.ImageCommandsHandler
{
    public class RetrieveImageCommandHandler : IRequestHandler<RetrieveImagesCommand,Response<List<ImageResponseDTO>>>
    {
        private readonly IUnitOfWork unitOfWork;
        public RetrieveImageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public  Task<Response<List<ImageResponseDTO>>> Handle(RetrieveImagesCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult( unitOfWork.ImageService.RetrieveImage(request.retrieveImage));
        }
    }
}
