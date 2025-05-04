using Bones_App.DTOs;
using MediatR;

namespace Bones_App.Commands.ImageCommand
{
    public class RetrieveImagesCommand:IRequest<Response<List<ImageResponseDTO>>>
    {
        public RetrieveImageDTO retrieveImage {  get; set; }
        public RetrieveImagesCommand(RetrieveImageDTO retrieveImage)
        {
            this.retrieveImage = retrieveImage;
        }
    }
}
