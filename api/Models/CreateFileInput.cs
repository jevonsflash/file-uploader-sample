using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FileUploader.Models
{
    public class CreateFileInput : IValidatableObject
    {
        public string FileContainerName { get; set; }

        public FileType FileType { get; set; }

        public Guid? ParentId { get; set; }

        public long? OwnerUserId { get; set; }

        public IFormFile File { get; set; }

        public bool GenerateUniqueFileName { get; set; }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrWhiteSpace(FileContainerName))
            {
                yield return new ValidationResult("FileContainerName should not be empty!",
                    new[] { nameof(FileContainerName) });
            }

            if (!Enum.IsDefined(typeof(FileType), FileType))
            {
                yield return new ValidationResult("FileType is invalid!",
                    new[] { nameof(FileType) });
            }
        }
    }
}