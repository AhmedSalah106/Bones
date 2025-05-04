using Bones_App.DTOs;
using MediatR;

namespace Bones_App.Queries.SpecialistQueries
{
    public class GetAllQuery:IRequest<List<SpecialistResponseDTO>>
    {
    }
}
