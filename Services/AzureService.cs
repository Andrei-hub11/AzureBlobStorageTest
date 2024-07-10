using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageTest.Contracts;
using AzureBlobStorageTest.Extensions;
using System.Runtime.CompilerServices;

namespace AzureBlobStorageTest.Services;

public class AzureService : IAzureService
{
    private readonly BlobContainerClient _containerClient;

    public AzureService(BlobServiceClient blobServiceClient)
    {
        _containerClient = blobServiceClient.GetBlobContainerClient("azure-files");
        _containerClient.CreateIfNotExists();
    }

    public async IAsyncEnumerable<BlobURLResponseDTO> GetFilesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var blobItem in _containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            var blobClient = _containerClient.GetBlobClient(blobItem.Name);
            var blobUrl = blobClient.Uri.ToString();
            yield return new BlobURLResponseDTO(blobUrl);
        }
    }

    public async Task<bool> UploadFileAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var blobClient = _containerClient.GetBlobClient(file.FileName.RemoveWhiteSpace());

        BlobHttpHeaders httpHeaders = new BlobHttpHeaders
        {
            ContentType = "image/png"
        };

        BlobUploadOptions uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = httpHeaders
        };

        await blobClient.UploadAsync(file.OpenReadStream(), uploadOptions, cancellationToken);

        return true;
    }

    public async Task<bool> DeleteFileAsync(string imageName, CancellationToken cancellationToken)
    {
        await _containerClient.DeleteBlobIfExistsAsync(imageName, cancellationToken: cancellationToken);

        return true;
    }
}
