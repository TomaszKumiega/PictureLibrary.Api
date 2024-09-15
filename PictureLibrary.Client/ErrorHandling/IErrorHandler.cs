namespace PictureLibrary.Client.ErrorHandling
{
    public interface IErrorHandler
    {
        void HandleErrorStatusCode(HttpResponseMessage response);
    }
}
