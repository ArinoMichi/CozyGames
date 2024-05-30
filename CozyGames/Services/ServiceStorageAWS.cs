using Amazon.S3.Model;
using Amazon.S3;
using CozyGames.Models;

namespace CozyGames.Services
{
    public class ServiceStorageAWS
    {
        private IAmazonS3 client;
        private string BucketName;

        public ServiceStorageAWS(KeysModel keys, IAmazonS3 client)
        {
            this.BucketName = keys.BucketName;
            this.client = client;
        }

        // MÉTODO PARA SUBIR LAS IMAGENES DONDE NECESITAMOS
        // EL NOMBRE DE LA IMAGEN, EL STREAM, EL BUCKET NAME
        public async Task<bool> UploadFileAsync(string fileName, Stream stream)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.BucketName
            };
            PutObjectResponse response = await this.client.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
