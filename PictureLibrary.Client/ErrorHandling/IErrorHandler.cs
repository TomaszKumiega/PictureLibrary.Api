namespace PictureLibrary.Client.ErrorHandling
{
    internal interface IErrorHandler
    {
        void HandleErrorStatusCode(HttpResponseMessage response);
    }
}
