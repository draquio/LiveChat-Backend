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

            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            if (!allowed.Contains(extension))
            {
                return BadRequest("Formato de imagen no soportado");
            }

            var uploadPath = Path.Combine(_env.WebRootPath, "uploads", "images");
            Directory.CreateDirectory(uploadPath);

            try
            {
                await _fileCleanupService.EnsureLimit(uploadPath, 100, 10);
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
    }
}
