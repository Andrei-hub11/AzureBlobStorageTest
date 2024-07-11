using AzureBlobStorageTest.Contracts;
using AzureBlobStorageTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorageTest.Controllers;

[Route("api/v1/azure")]
[ApiController]
public class AzureController : ControllerBase
{
    private readonly IAzureService _azureService;

    public AzureController(IAzureService azureService)
    {
        _azureService = azureService;
    }

    [HttpGet]
    public IAsyncEnumerable<BlobURLResponseDTO> GetImages(CancellationToken cancellationToken)
    {
        try
        {
            return _azureService.GetImagesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureService.UploadImageAsync(file, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteImage(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureService.DeleteImageAsync(fileName, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
