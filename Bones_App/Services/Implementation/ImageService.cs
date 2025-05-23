﻿using Bones_App.DTOs;
using Bones_App.Exceptions;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class ImageService:Service<Image> , IImageService
    {

        private readonly IImageRepository imageRepository;
        public ImageService(IImageRepository imageRepository):base(imageRepository)
        {
            this.imageRepository = imageRepository;
        }
         
        public List<ImageResponseDTO> GetAllUserImagesDTO(string Id)
        {

            List<ImageResponseDTO> AllImagesDTO
                = GetAll().Where(img=>img.UserId==Id).Select(img => new ImageResponseDTO 
                    { Id = img.Id , ImageUrl = img.ImageURL , UploadedAt = img.UploadedAt }).ToList();

            return AllImagesDTO;
        }
            
        public ImageResponseDTO GetImageResponseDTO(Image image)
        {
            ImageResponseDTO ImageDTO = new ImageResponseDTO()
            {
                Id = image.Id,
                UploadedAt = image.UploadedAt,
                ImageUrl = image.ImageURL
            };

            return ImageDTO;
        }


        public Response<List<ImageResponseDTO>> RetrieveImage(string Id)
        {
            
            

            
                
            List<ImageResponseDTO> Images = GetAllUserImagesDTO(Id);

            if (Images == null || Images.Count == 0)
            {
                throw new CustomException("User has not uploaded any images Yet!");
            }

            Response<List<ImageResponseDTO>> response = new Response<List<ImageResponseDTO>>()
            {
                Data = Images,
                Success = true,
                Message = "Successfully Retrieve all Images"
            };

            return response;
            
        }

    }
}
