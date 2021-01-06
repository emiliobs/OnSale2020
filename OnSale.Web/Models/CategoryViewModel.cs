using Microsoft.AspNetCore.Http;
using Onsale.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Web.Models
{
    public class CategoryViewModel : Category
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
