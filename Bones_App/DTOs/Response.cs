
namespace Bones_App.DTOs
{
    public class Response<T>
    {
        public bool Success {  get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Response(T Data, string Message = null) 
        {
            Success = true;
            this.Message = Message;
            this.Data = Data;
        }
        public Response(string Message) 
        {
            Success = false;
            this.Message = Message;
        }

        public Response()
        {

        }

       
    }
}
