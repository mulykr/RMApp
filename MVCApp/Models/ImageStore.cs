using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MVCApp.Models
{
    public class ImageStore
    {
        private readonly CloudBlobClient _blobClient;
        private readonly string _baseUri = ConfigurationManager.AppSettings["baseUri"];
        private readonly string _containerName = ConfigurationManager.AppSettings["containerName"];
        public ImageStore()
        {
            var credentials = new StorageCredentials(ConfigurationManager.AppSettings["accountName"],
                ConfigurationManager.AppSettings["keyValue"]);
            _blobClient = new CloudBlobClient(new Uri(_baseUri), credentials);
        }
        public async Task<string> SaveImage(Stream imageStream)
        {
            var imageId = Guid.NewGuid().ToString();
            var container = _blobClient.GetContainerReference(_containerName);
            var blob = container.GetBlockBlobReference(imageId);
            await blob.UploadFromStreamAsync(imageStream);
            return imageId;
        }

        public async Task RemoveFile(Uri uri)
        {
            var container = _blobClient.GetContainerReference(_containerName);
            var blob = container.GetBlockBlobReference(uri.LocalPath.Remove(0, 8));
            await blob.DeleteIfExistsAsync();
        }

        public async Task<List<Uri>> GetBlobsUri()
        {
            var container = _blobClient.GetContainerReference(_containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            BlobContinuationToken configToken = null;
            List<IListBlobItem> blobItems = new List<IListBlobItem>();
            do
            {
                var response = await container.ListBlobsSegmentedAsync(configToken);
                configToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            }
            while (configToken != null);

            return new List<Uri>(blobItems.Select(item => item.Uri));
        }

        public async Task<IListBlobItem> GetBlob(Uri uri)
        {
            var container = _blobClient.GetContainerReference(_containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container.GetBlockBlobReference(uri.LocalPath.Remove(0, 8));
        }

        public async Task<IListBlobItem> GetBlob(string id)
        {
            var container = _blobClient.GetContainerReference(_containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container.GetBlockBlobReference(id);
        }

        public string UriFor(string imageId)
        {
            return $"{_baseUri}/{_containerName}/{imageId}";
        }
    }
}