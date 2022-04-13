using FileUploader.Models;
using Microsoft.AspNetCore.Mvc;
using File = FileUploader.Models.File;

namespace FileUploader.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// �ϴ��ļ��ӿ�
        /// </summary>
        /// <param name="input">�ļ��ϴ��������</param>
        /// <returns></returns>
        /// <exception cref="NoUploadedFileException"></exception>
        [Route("upload")]
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<bool> ActionCreateAsync([FromForm] CreateFileInput input)
        {
            //�ж��Ƿ��ϴ����ļ�����
            if (input.File == null)
            {
                throw new NoUploadedFileException();
            }

            var fileName = input.File.FileName;

            await using var memoryStream = new System.IO.MemoryStream();

            //���ļ����ݸ��Ƶ�����
            await input.File.CopyToAsync(memoryStream);


            //�����ļ����ʵ��
            var file = new File
            {
                FileContainerName = input.FileContainerName,
                FileName = fileName,
                MimeType = input.File.ContentType,
                FileType = input.FileType,
                ParentId = input.ParentId,
                OwnerUserId = input.OwnerUserId,
                //��ȡ��������������ļ�����������
                Content = memoryStream.ToArray()
            };
         
            Console.WriteLine($"�ļ���:{file.FileName}", $"�ļ���С:{file.Content.Length} �ֽ�");
            return true;

        }


    }
}