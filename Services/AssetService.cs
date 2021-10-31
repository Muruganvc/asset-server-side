using Asset.Interfaces;
using Asset.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Asset.Services
{
    public class AssetService : IAssetService
    {
        private readonly AssetContext _context;
        public AssetService(AssetContext context)
        {
            _context = context;
        }
        public async Task<int> SaveOrEditAsset(HttpRequest request, IFormCollection data,string Option)
        {
            TblAsset asset = new TblAsset();
            string assetId = string.Empty;
            string fileName = string.Empty;
            string type = string.Empty;
            if(request.Form.Files.Count > 0)
            {
                var file = request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Files");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    type = file.ContentType;
                }
            }
            if (data.TryGetValue("userObj", out var userObj))
            {
                var assetObject = JsonConvert.DeserializeObject<dynamic>(userObj);
                asset.Description = assetObject["description"].Value;
                asset.CreatedBy = assetObject["userName"].Value;
                asset.Country = assetObject["country"].Value;
                asset.Email = assetObject["email"].Value;
                asset.FileName = fileName;
                asset.MimeType = type;
                Guid obj = Guid.NewGuid();
                asset.AssetId = obj;
                if (Option != "Save")
                {
                    assetId = assetObject["assetId"].Value;
                }
            }
            if (asset != null)
            {
                if(Option == "Save")
                {
                    _context.Add(asset);
                }
                else
                {
                    TblAsset edit = _context.TblAssets.Where(w => w.AssetId == Guid.Parse(assetId)).SingleOrDefault();
                    if (edit != null)
                    {
                        edit.Country = asset.Country;
                        edit.Description = asset.Description;
                        edit.Email = asset.Email;
                        edit.CreatedBy = asset.CreatedBy;
                        edit.FileName = fileName;
                        edit.MimeType = type;
                    }
                }
                return await _context.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<IList<TblAsset>> getAssets() => await _context.TblAssets.ToListAsync();

        public async Task<TblAsset> getAssetsById(string assetId)
        {
            return await _context.TblAssets.Where(w => w.AssetId == Guid.Parse(assetId.Trim())).SingleOrDefaultAsync(); ;
        }

        public async Task<int> DeleteAsset(string assetId)
        {
            TblAsset asset = _context.TblAssets.Where(w => w.AssetId == Guid.Parse(assetId)).SingleOrDefault();
            if (asset != null)
            {
                _context.Remove(asset);
                var folderName = Path.Combine("Resources", "Files");
                File.Delete(folderName + @"\" + asset.FileName);
            }
            return await _context.SaveChangesAsync();
        }
    }
}
