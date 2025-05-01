using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Repositories.SharedRepo;

namespace Bones_App.Repositories.Implementation
{
    public class ImageRepository:Repository<Image> , IImageRepository
    {
        private readonly BonesContext context;

        public ImageRepository(BonesContext context):base(context) 
        {
            this.context = context;
        }
    }

}
