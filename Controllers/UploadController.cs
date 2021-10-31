using Asset.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Asset.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IAssetService _service;
        public UploadController(IAssetService service)
        {
            _service = service;
        }
        [HttpPost("Upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(IFormCollection data)
        {
            try
            {
                return Ok(await _service.SaveOrEditAsset(Request, data,"Save"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("getAllAsset")]
        public async Task<IActionResult> getAllAsset()
        {
            try
            {
                return Ok(await _service.getAssets());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string file)
        {
            try
            {
                var uploads = Path.Combine("Resources", "Files");
                var filePath = Path.Combine(uploads, file);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, GetContentType(filePath), file);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpDelete("deleteAsset/{assetId}")]
        public async Task<IActionResult> deleteAsset(string assetId )
        {
            try
            {
                return Ok(await _service.DeleteAsset(assetId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost("EditAsset/{Option}")]
        public async Task<IActionResult> EditAsset(string Option, IFormCollection data)
        {
            try
            {
                return Ok(await _service.SaveOrEditAsset(Request, data, Option));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet("getAssetById/{assetId}")]
        public async Task<IActionResult> getAssetById(string assetId)
        {
            try
            {
                return Ok(await _service.getAssetsById(assetId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
