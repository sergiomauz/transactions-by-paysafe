namespace Application.ErrorCatalog
{
    public class ErrorTuple
    {
        public string ErrorCode { get; set; }
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorTuple(string errorCode, string propertyName, string errorMessage)
        {
            ErrorCode = errorCode;
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }
}
