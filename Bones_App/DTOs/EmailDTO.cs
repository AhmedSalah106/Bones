namespace Bones_App.DTOs
{
    public class EmailDTO
    {
        public string To {  get; set; }
        public string Subject {  get; set; }
        public string Body { get; set; }
        public DateTime DateSent { get; set; }
        public int SpecialistId {  get; set; }
    }
}
