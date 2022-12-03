using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Profile.Storage.Api.Files.Models;
using Profile.Storage.Domain.Storage;

namespace Profile.Storage.Api.Files
{
    [Route("/storage/api/files")]
    public class FileManagementController : ControllerBase
    {
        private readonly IFileStorageService _storageService;
        private readonly ILogger<FileManagementController> _logger;

        public FileManagementController(ILogger<FileManagementController> logger, IFileStorageService storageService)
        {
            _storageService = storageService;
            _logger = logger;
        }

        /// <summary>
        /// Provide the way to save files. 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SaveFile([FromForm(Name = "file")] IEnumerable<IFormFile> files, CancellationToken token)
        {
            var formFiles = files.ToList();
            var file = formFiles.First();
            
            await using var readStream = file.OpenReadStream();
            
            var fileMetadata = new FileMetadata()
            {
                Id = Guid.NewGuid(),
                Name = file.FileName,
                ContentType = file.ContentType,
                StorageType = StorageType.AmazonS3,
                DateLoaded = DateTime.UtcNow,
                FileStatus = ExistenceStatus.Exists
            };
            
            await _storageService.SaveFileAsync(fileMetadata, readStream, token);
            
            var response = new FileResponse()
            {
                Id = fileMetadata.Id,
                Name = fileMetadata.Name,
                SizeBytes = file.Length,
            };
            
            return await Task.FromResult(Ok(response));
        }

        /// <summary>
        /// Get the list of files.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetModels([FromRoute] Guid id, CancellationToken token)
        {
            var (fileStream, fileMetadata) = await _storageService.GetFileAsync(id, token);
            return await Task.FromResult(File(fileStream, fileMetadata.ContentType, fileMetadata.Name));
        }
        
        /// <summary>
        /// Deletes file by it's id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFile([FromRoute] Guid id, CancellationToken token)
        {
            await _storageService.DeleteFileByIdAsync(id, token);
            return await Task.FromResult(Ok());
        }
    }
}