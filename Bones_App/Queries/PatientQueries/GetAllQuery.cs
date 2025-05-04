using Bones_App.DTOs;
using MediatR;

namespace Bones_App.Queries.PatientQuries
{
    public class GetAllQuery:IRequest<List<PatientResponseDTO>>
    {

    }
}
