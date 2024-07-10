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
    public IAsyncEnumerable<BlobURLResponseDTO> GetAllFile(CancellationToken cancellationToken)
    {
        try
        {
            return _azureService.GetFilesAsync(cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureService.UploadFileAsync(file, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("delete/{fileName}")]
    public async Task<IActionResult> DeleteFile(string fileName, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _azureService.DeleteFileAsync(fileName, cancellationToken);

            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
