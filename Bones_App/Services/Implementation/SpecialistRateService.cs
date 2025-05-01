using Bones_App.DTOs;
using Bones_App.Models;
using Bones_App.Repositories.Interfaces;
using Bones_App.Services.Interfaces;
using Bones_App.Services.SharedService;

namespace Bones_App.Services.Implementation
{
    public class SpecialistRateService:Service<SpecialistRate>,ISpecialistRateService
    {
        private readonly ISpecialistRateRepository specialistRateRepository;
        public SpecialistRateService(ISpecialistRateRepository specialistRateRepository):base(specialistRateRepository)
        {
            this.specialistRateRepository = specialistRateRepository;   
        }

        public SpecialistRate ConvertToSpecialistRate(SpecialistRateDTO specialistRateDTO)
        {
            SpecialistRate specialistRate = new SpecialistRate()
            {
                SpecialistId = specialistRateDTO.SpecialistId,
                Comment = specialistRateDTO.Comment,
                PatientId = specialistRateDTO.PatientId,
                RatingValue = specialistRateDTO.RatingValue,
                RatedAt = specialistRateDTO.RatedAt
            };

            return specialistRate;
        }

        public SpecialistRateDTO ConvertToSpecialistRateDTO(SpecialistRate specialistRate)
        {
            SpecialistRateDTO specialistRateDTO = new SpecialistRateDTO()
            {
                SpecialistId = specialistRate.SpecialistId,
                Comment = specialistRate.Comment,
                PatientId = specialistRate.PatientId,
                RatingValue = specialistRate.RatingValue,
                RatedAt = specialistRate.RatedAt   
            };
            return specialistRateDTO;
        }

        public SpecialistRate GetPatientRatingForSpecialist(int SpecialistId, int PatientId)
        {
            return specialistRateRepository.GetPatientRatingForSpecialist(SpecialistId, PatientId);
        }

        public SpecialistRate GetBySpecialistId(int SpecialistId)
        {
            return specialistRateRepository.GetBySpecialistId(SpecialistId);
        }
        public int GetAverageRating(int SpecialistId)
        {
            var ratings = GetAll().Where(rate=>rate.SpecialistId== SpecialistId).ToList();

            if (ratings==null || ratings.Count == 0)
                return 0;

            int Avg =(int) ratings.Average(rate=>rate.RatingValue);
            return Avg;
        }

        public List<SpecialistRateResponseDTO> GetTopTenSpecialists()
        {
            List<SpecialistRateResponseDTO> topTen = GetAll().GroupBy(g => g.SpecialistId).Select(group => new SpecialistRateResponseDTO
            {
                
                SpecialistId = group.Key,
                AvgRate = GetAverageRating(group.Key),
                RatingCount = group.Count()

            }).OrderByDescending(O=>O.AvgRate)
            .ThenByDescending(O=>O.RatingCount)
            .ToList();

            return topTen;
        }
    }
}
