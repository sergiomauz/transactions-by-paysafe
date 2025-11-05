using System.Net;


namespace Application.Commons.Exceptions
{
    public class ContentValidationException : Exception
    {
        public string PropertyName { get; set; }
        public string CodeError { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ContentValidationException(string propertyName, string codeError, string messageError, HttpStatusCode statusCode) : base(messageError)
        {
            PropertyName = propertyName;
            CodeError = codeError;
            StatusCode = statusCode;
        }
    }
}
