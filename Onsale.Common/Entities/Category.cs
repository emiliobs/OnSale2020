using System;
using System.ComponentModel.DataAnnotations;

namespace Onsale.Common.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }


        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://onsaleweb2021.azurewebsites.net/images/noimage.jpg"
            : $"https://onsaleemilio.blob.core.windows.net/categories/{ImageId}";
    }

}
