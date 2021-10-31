using Asset.models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asset.Interfaces
{
    public interface IAssetService
    {
        Task<int> SaveOrEditAsset(HttpRequest request, IFormCollection data, string Option);
        Task<IList<TblAsset>> getAssets();
        Task<int> DeleteAsset(string assetId);
        Task<TblAsset> getAssetsById(string assetId);
    }
}
