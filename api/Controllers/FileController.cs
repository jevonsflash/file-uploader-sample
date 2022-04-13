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
        /// 上传文件接口
        /// </summary>
        /// <param name="input">文件上传传输对象</param>
        /// <returns></returns>
        /// <exception cref="NoUploadedFileException"></exception>
        [Route("upload")]
        [HttpPost]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<bool> ActionCreateAsync([FromForm] CreateFileInput input)
        {
            //判断是否上传了文件内容
            if (input.File == null)
            {
                throw new NoUploadedFileException();
            }

            var fileName = input.File.FileName;

            await using var memoryStream = new System.IO.MemoryStream();

            //将文件内容复制到流中
            await input.File.CopyToAsync(memoryStream);


            //创建文件类的实体
            var file = new File
            {
                FileContainerName = input.FileContainerName,
                FileName = fileName,
                MimeType = input.File.ContentType,
                FileType = input.FileType,
                ParentId = input.ParentId,
                OwnerUserId = input.OwnerUserId,
                //读取流来获得完整的文件二进制内容
                Content = memoryStream.ToArray()
            };
         
            Console.WriteLine($"文件名:{file.FileName}", $"文件大小:{file.Content.Length} 字节");
            return true;

        }


    }
}