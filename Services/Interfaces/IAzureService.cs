using AzureBlobStorageTest.Contracts;
namespace AzureBlobStorageTest.Services;

public interface IAzureService
{
    IAsyncEnumerable<BlobURLResponseDTO> GetImagesAsync(CancellationToken cancellationToken);
    Task<bool> UploadImageAsync(IFormFile file, CancellationToken cancellationToken);
    Task<bool> DeleteImageAsync(string blobName, CancellationToken cancellationToken);
}
