using Onsale.Common.Response;

namespace OnSale.Web.Helpers
{
    public interface IEmailHelper
    {
        Response SendMail(string to, string subject, string body);
    }
}
