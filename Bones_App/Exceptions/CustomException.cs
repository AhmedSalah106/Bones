namespace Bones_App.Exceptions
{
    public class CustomException:Exception
    {
        public ErrorResponse ErrorResponse { get; } 
        public CustomException(List<string>errors,int StatusCode = 400)
            :base(string.Join("; ",errors))
        {
            ErrorResponse = new ErrorResponse()
            {
                Errors = errors,
                statusCode = StatusCode
            };
        }


        public CustomException(string error, int StatusCode = 400) 
            :this(new List<string>() { error}, StatusCode)
        {


        }

    }
}
