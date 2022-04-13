namespace FileUploader.Models
{
    public class NoUploadedFileException : Exception
    {
        public NoUploadedFileException() : base("NoUploadedFile")
        {

        }
    }
}