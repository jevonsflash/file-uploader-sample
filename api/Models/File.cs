using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileUploader.Models
{
    [Serializable]
    public class File
    {
        [Required]
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public FileType FileType { get; set; }

        public byte[] Content { get; set; }

        public Guid? ParentId { get; set; }

        public long? OwnerUserId { get; set; }

        public string FileContainerName { get; set; }


    }
}