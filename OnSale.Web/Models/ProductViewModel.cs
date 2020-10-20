﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Onsale.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Models
{
    public class ProductViewModel : Product
    {
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Category")]
        [Required]
        public int CategoryId { get; set; }
        
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
