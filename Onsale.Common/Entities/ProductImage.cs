using System;
using System.ComponentModel.DataAnnotations;

namespace Onsale.Common.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://onsaleweb2021.azurewebsites.net/images/noimage.png"
            : $"https://onsaleemilio.blob.core.windows.net/products/{ImageId}";

    }
}
