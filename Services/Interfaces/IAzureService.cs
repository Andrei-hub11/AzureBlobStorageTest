using AzureBlobStorageTest.Contracts;
namespace AzureBlobStorageTest.Services;

public interface IAzureService
{
    IAsyncEnumerable<BlobURLResponseDTO> GetFilesAsync(CancellationToken cancellationToken);
    Task<bool> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);
    Task<bool> DeleteFileAsync(string blobName, CancellationToken cancellationToken);
}
