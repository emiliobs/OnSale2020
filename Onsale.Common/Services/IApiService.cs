using Onsale.Common.Responses;
using System.Threading.Tasks;

namespace Onsale.Common.Services
{
    public interface IApiService
    {
        Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller);
    }
}
