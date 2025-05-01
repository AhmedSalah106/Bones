using System.ComponentModel.DataAnnotations;

namespace Bones_App.DTOs
{
    public class AcceptSpecialistDTo
    {
        public int Id { get; set; }

        [EmailAddress]
        public string Email {  get; set; }
    }
}
