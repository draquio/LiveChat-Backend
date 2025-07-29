using LiveChat.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LiveChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IFileCleanupService _fileCleanupService;

        public UploadController(IWebHostEnvironment env, IFileCleanupService fileCleanupService)
        {
            _env = env;
            _fileCleanupService = fileCleanupService;
        }

        [HttpPost("images")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0) 
            {
                return BadRequest("No se subió ningún archivo");
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowed.Contains(extension))
            {
                return BadRequest("Formato de imagen no soportado");
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "images");
            Directory.CreateDirectory(uploadPath);

            try
            {
                await _fileCleanupService.EnsureLimit(uploadPath, 10, 5);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se pudo eliminar archivos antiguos: {ex.Message}");
            }

            var fileName = $"img_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/images/{fileName}";
            return Ok(new { url = publicUrl });

        }

        [HttpPost("audios")]
        public async Task<IActionResult> UploadAudios(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se subió ningún audio.");
            }
            string extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedExtensions = new[] { ".mp3", ".wav", ".ogg", ".webm", ".m4a" };
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Formato de audio no soportado.");
            }

            string uploadPath = Path.Combine(_env.WebRootPath, "uploads", "audios");
            Directory.CreateDirectory(uploadPath);

            try
            {
                await _fileCleanupService.EnsureLimit(uploadPath, 10, 5);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se pudo limpiar audios antiguos: {ex.Message}");
            }
            string fileName = $"audio_{Guid.NewGuid():N}{extension}";
            string filePath = Path.Combine(uploadPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            string publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/audios/{fileName}";
            return Ok(new { url = publicUrl });
        }

        [HttpPost("documents")]
        public async Task<IActionResult> UploadDocuments(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No se subió ningún archivo");
            }

            string extension = Path.GetExtension(file.FileName).ToLower();
            string[] allowed = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" };

            if (!allowed.Contains(extension))
            {
                return BadRequest("Formato de documento no soportado");
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "documents");
            Directory.CreateDirectory(uploadPath);

            try
            {
                await _fileCleanupService.EnsureLimit(uploadPath, 10, 5);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"No se pudo limpiar documentos antiguos: {ex.Message}");
            }

            var fileName = $"doc_{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            var publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/documents/{fileName}";
            return Ok(new { url = publicUrl });
        }
    }
}
