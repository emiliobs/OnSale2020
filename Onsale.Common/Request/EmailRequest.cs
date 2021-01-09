using System.ComponentModel.DataAnnotations;

namespace Onsale.Common.Request
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }

}
