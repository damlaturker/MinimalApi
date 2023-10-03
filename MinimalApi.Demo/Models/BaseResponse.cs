using System.Net;

namespace MinimalApi.Demo.Models
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool IsSuccess { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
