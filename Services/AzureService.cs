using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobStorageTest.Contracts;
using AzureBlobStorageTest.Extensions;
using FileTypeChecker;
using FileTypeChecker.Abstracts;
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

    public async IAsyncEnumerable<BlobURLResponseDTO> GetImagesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var blobItem in _containerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            var blobClient = _containerClient.GetBlobClient(blobItem.Name);
            var blobUrl = blobClient.Uri.ToString();
            yield return new BlobURLResponseDTO(blobUrl);
        }
    }

    public async Task<bool> UploadImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        var blobClient = _containerClient.GetBlobClient(file.FileName.RemoveWhiteSpace());

        string contentType = await GetFileContentTypeAsync(file.OpenReadStream());

        // Verificar se o tipo de conteúdo é uma imagem
        if (!IsImageContentType(contentType))
        {
            throw new InvalidOperationException($"O arquivo '{file.FileName}' não é uma imagem válida.");
        }

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

    public async Task<bool> DeleteImageAsync(string imageName, CancellationToken cancellationToken)
    {
        await _containerClient.DeleteBlobIfExistsAsync(imageName, cancellationToken: cancellationToken);

        return true;
    }

    private async Task<string> GetFileContentTypeAsync(Stream fileStream)
    {
        fileStream.Position = 0; 
        byte[] fileHeader = new byte[560]; 
        await fileStream.ReadAsync(fileHeader, 0, fileHeader.Length);
        fileStream.Position = 0; 

        IFileType fileType = FileTypeValidator.GetFileType(fileHeader);

        // Retornar o MIME type ou um default se não conseguir identificar
        return fileType?.Extension.ToUpperInvariant() switch
        {
            "JPG" => "image/jpeg",
            "JPEG" => "image/jpeg",
            "PNG" => "image/png",
            "GIF" => "image/gif",
            "BMP" => "image/bmp",
            "TIFF" => "image/tiff",
            "WEBP" => "image/webp",
            _ => "application/octet-stream"
        };
    }

    private bool IsImageContentType(string contentType)
    {
        // Lista de tipos MIME válidos para imagens
        var imageMimeTypes = new HashSet<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/tiff",
            "image/webp"
        };

        return imageMimeTypes.Contains(contentType);
    }
}
