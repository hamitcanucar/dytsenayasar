using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using dytsenayasar.DataAccess.Entities;
using dytsenayasar.Models.ControllerModels;
using dytsenayasar.Models.Settings;
using dytsenayasar.Services.Abstract;
using dytsenayasar.Util;
using ContentType = dytsenayasar.DataAccess.Entities.ContentTypes;

namespace dytsenayasar.Controllers
{
    [Route("file")]
    [ApiController]
    public class FileController : AController<FileController>
    {
        public const string FILE_URL = "{0}/file/{1}";
        public const string IMAGE_URL = "{0}/file/image/{1}";

        private readonly IFileManager _fileManager;
        private readonly IFileTypeChecker _fileTypeChecker;
        private readonly FileManagerSettings _fileManagerSettings;
        private readonly AppSettings _appSettings;
        private readonly IContentService _contentService;
        // private readonly IContentDeliveryService _contentDeliveryService;
        private readonly IUserService _userService;

        public FileController(IFileManager fileManager, IFileTypeChecker fileTypeChecker, IContentService contentService, IUserService userService,
            ILogger<FileController> logger, IOptions<FileManagerSettings> fileManagerSettings, IOptions<AppSettings> appSettings) : base(logger)
        {
            _fileManager = fileManager;
            _fileTypeChecker = fileTypeChecker;
            _contentService = contentService;
            // _contentDeliveryService = contentDeliveryService;
            _userService = userService;
            _fileManagerSettings = fileManagerSettings.Value;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var fileName = id.ToString();
            var userId = GetUserIdFromToken();
            bool fileIsAvailable;

            if (User.IsInRole(Role.ADMIN))
            {
                fileIsAvailable = true;
            }
            else
            {
                fileIsAvailable = await _contentService.CheckContentFileAvailableForUser(id, userId);
            }

            if (fileIsAvailable)
            {
                var result = _fileManager.OpenFileStream(fileName);

                if (result.Status == FileManagerStatus.Completed)
                {
                    return File(result.Stream, MediaTypeNames.Application.Octet);
                }

                if (result.Status != FileManagerStatus.FileNotFound)
                {
                    _logger.LogError("File({0}) does not available: {1}", fileName, result.Status.ToString());
                }
            }
            return NoContent();
        }

        [HttpGet]
        [Route("image/{id}")]
        [Authorize]
        public IActionResult GetImage(Guid id)
        {
            var fileName = id.ToString();
            var result = _fileManager.OpenImageStream(fileName);

            if (result.Status == FileManagerStatus.Completed)
            {
                return File(result.Stream, MediaTypeNames.Image.Jpeg);
            }

            if (result.Status != FileManagerStatus.FileNotFound)
            {
                _logger.LogError("Image({0}) does not available: {1}", fileName, result.Status.ToString());
            }
            return NoContent();
        }

        [HttpPost]
        [Route("content/{id}/files")]
        [Authorize]
        [DisableRequestSizeLimit]
        public async Task<GenericResponse<string>> UploadContentFiles([FromForm] IFormFile file)
        {
             if ( file == null)
            {
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_EMPTY),
                    Message = ErrorMessages.FILE_EMPTY
                };
            }

            Guid? fileId = Guid.NewGuid();
            var fileName = fileId.Value.ToString();

            var tasks = new List<Task<FileManagerResult>>();
            Stream fileStream = null;

            if (file != null)
            {
                fileStream = file.OpenReadStream();
            }

            if (fileStream != null)
            {
                tasks.Add(_fileManager.WriteFile(fileName, fileStream));
            }
            else
            {
                fileId = null;
            }

            var fileManagerResults = await Task.WhenAll(tasks);
            FileManagerResult fileResult = fileManagerResults.SingleOrDefault(x => x.Name == fileName)
                ?? new FileManagerResult { Status = FileManagerStatus.Completed };

            var result = ReturnUploadFileResult(fileName, fileResult.Status);

            return result;
        }

        [HttpPost]
        [Route("content/{id}/images")]
        [Authorize]
        [DisableRequestSizeLimit]
        public async Task<GenericResponse<string>> UploadContentImages(Guid id, [FromForm] IFormFile image)
        {
            if (image == null)
            {
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_EMPTY),
                    Message = ErrorMessages.FILE_EMPTY
                };
            }
            Content content;

            var userId = GetUserIdFromToken();
            content = await _contentService.GetUserContent(id, userId);
         
            if (content == null)
            {
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_CONTENT_NOT_FOUND),
                    Message = ErrorMessages.FILE_CONTENT_NOT_FOUND
                };
            }

            Guid? imageId = Guid.NewGuid();
            var imageName = imageId.Value.ToString();

            var tasks = new List<Task<FileManagerResult>>();
            Stream imgStream = null;

            if (image != null)
            {
                imgStream = image.OpenReadStream();
                if (!await CheckImageType(imgStream)) return CreateWrongImageError();
            }

            if (imgStream != null)
            {
                tasks.Add(_fileManager.WriteImage(imageName, imgStream));
            }
            else
            {
                imageId = null;
            }

            var fileManagerResults = await Task.WhenAll(tasks);
            FileManagerResult imgResult = fileManagerResults.SingleOrDefault(x => x.Name == imageName)
                ?? new FileManagerResult { Status = FileManagerStatus.Completed };

            var result = ReturnUploadImageResult(imageName, imgResult.Status);

            if (result.Success)
            {
                if (image != null && content.Image.HasValue)
                {
                    _ = _fileManager.DeleteImage(content.Image.Value.ToString());
                }

                if (result.Success)
                {
                    _ = _fileManager.CreateImage(imageName);
                }
                else
                {
                    _ = _fileManager.DeleteImage(imageName);
                }
            }
            return result;
        }
        
        [HttpPost]
        [Route("user/image")]
        [Authorize]
        [DisableRequestSizeLimit]
        public Task<GenericResponse<string>> UploadProfileImage([FromForm] IFormFile image)
        {
            return UploadProfileImage(GetUserIdFromToken(), image);
        }
        
        [NonAction]
        private async Task<GenericResponse<string>> UploadProfileImage(Guid userId, IFormFile image)
        {
            var result = await UploadImage(image);
            if (!result.Success) return result;

            var oldImg = await _userService.UpdateImage(userId, new Guid(result.Data));

            if (oldImg == Guid.Empty)
            {
                _ = _fileManager.DeleteImage(result.Data);
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.USER_NOT_FOUND),
                    Message = ErrorMessages.USER_NOT_FOUND
                };
            }
            else
            {
                DeleteOldImage(oldImg);
            }

            return new GenericResponse<string> { Success = true };
        }

        [NonAction]
        private async Task<GenericResponse<string>> UploadImage(IFormFile image)
        {
            if (image == null)
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_EMPTY),
                    Message = ErrorMessages.FILE_EMPTY
                };

            var stream = image.OpenReadStream();
            if (!await CheckImageType(stream)) return CreateWrongImageError();

            var imgId = Guid.NewGuid();
            var imgName = imgId.ToString();
            var imgResult = await _fileManager.WriteImage(imgName, stream);

            if (imgResult.Status != FileManagerStatus.Completed)
            {
                return returnImageWriteError(imgResult.Status, imgName);
            }

            return new GenericResponse<string> { Success = true, Data = imgName };
        }

        [NonAction]
        private void DeleteOldImage(Guid? imgId)
        {
            if (imgId.HasValue) _ = _fileManager.DeleteImage(imgId.Value.ToString());
        }

        [NonAction]
        private GenericResponse<string> ReturnUploadFileResult(
                    String fileName, FileManagerStatus fileStatus)
        {
            if (fileStatus == FileManagerStatus.Completed)
            {
                return new GenericResponse<string>
                {
                    Success = true
                };
            }
            else if (fileStatus != FileManagerStatus.Completed)
            {
                _ = _fileManager.DeleteFile(fileName);
                return returnFileWriteError(fileStatus, fileName);
            }
            else
            {
                return returnFileWriteError(fileStatus, fileName);
            }

        }

        private GenericResponse<string> ReturnUploadImageResult(String imageName, FileManagerStatus imgStatus)
        {
            if (imgStatus == FileManagerStatus.Completed)
            {
                return new GenericResponse<string>
                {
                    Success = true
                };
            }
            else if (imgStatus != FileManagerStatus.Completed)
            {
                _ = _fileManager.DeleteImage(imageName);
                return returnImageWriteError(imgStatus, imageName);
            }
            else
            {
                return returnFileWriteError(imgStatus, imageName);
            }

        }

        [NonAction]
        private GenericResponse<string> returnFileWriteError(FileManagerStatus fileStatus, string fileName)
        {
            if (fileStatus == FileManagerStatus.TooBigFile)
            {
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_TOO_BIG),
                    Message = String.Format(ErrorMessages.FILE_TOO_BIG, "file", _fileManagerSettings.MaxFileSizeInMB)
                };
            }
            else
            {
                _logger.LogError("File({0}) create failed: {1}", fileName, fileStatus.ToString());
                Response.StatusCode = 500;
                return null;

            }
        }

        [NonAction]
        private GenericResponse<string> returnImageWriteError(FileManagerStatus imgStatus, string imageName)
        {
            if (imgStatus == FileManagerStatus.TooBigFile)
            {
                return new GenericResponse<string>
                {
                    Code = nameof(ErrorMessages.FILE_TOO_BIG),
                    Message = String.Format(ErrorMessages.FILE_TOO_BIG, "image", _fileManagerSettings.MaxImageSizeInMB)
                };
            }
            else
            {
                _logger.LogError("Image({0}) create failed: {1}", imageName, imgStatus.ToString());
                Response.StatusCode = 500;
                return null;
            }
        }

        [NonAction]
        private Task<bool> CheckFileType(Stream stream, ContentType contentType)
        {
            return _fileTypeChecker.IsFileTypeCorrect(stream, contentType.ToFileType());
        }

        [NonAction]
        private Task<bool> CheckImageType(Stream stream)
        {
            return _fileTypeChecker.IsFileTypeCorrect(stream, FileType.Image);
        }

        [NonAction]
        private GenericResponse<string> CreateWrongImageError()
        {
            return new GenericResponse<string>
            {
                Code = nameof(ErrorMessages.FILE_TYPE_WRONG),
                Message = string.Format(ErrorMessages.FILE_TYPE_WRONG, "Png, Jpg, Bmp, Gif")
            };
        }

        [NonAction]
        private GenericResponse<string> CreateWrongFileError(ContentType contentType)
        {
            var fileDesc = contentType == ContentType.Any ? "null" : contentType.ToString();

            return new GenericResponse<string>
            {
                Code = nameof(ErrorMessages.FILE_TYPE_WRONG),
                Message = string.Format(ErrorMessages.FILE_TYPE_WRONG, fileDesc)
            };
        }
    }
}