using System.Net;

namespace ApiProyects.Models
{
    public class ApiResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsSuccessfull { get; set; } = true;

        public List<string> ErrorMessages { get; set;}

        public object Result { get; set; }
    }
}
