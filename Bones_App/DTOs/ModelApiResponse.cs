namespace Bones_App.DTOs
{
    public class ModelApiResponse
    {
        public string Error_Message { get; set; }
        public bool Is_Success { get; set; }
        public string Status_Code { get; set; }
        public ModelApiData Data { get; set; }

    }
}
