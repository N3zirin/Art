using Art.Abstractions.IServices;
using Art.Data;
using Art.DTOs;
using Art.DTOs.ArtistDTOs;
using Art.Entities;
using Art.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Art.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly ILogger<ArtistsController> _logger;
        private readonly IArtistService _artistService;
        public ArtistsController(ILogger<ArtistsController> logger, IArtistService artistService)
        {
            _logger = logger;
            _artistService = artistService;
        }

        [HttpGet("[action]"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllArtist()
        {
            var data = await _artistService.GetAllArtists();
            _logger.LogWarning($"GetAllArtist method executed: {data}");
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetArtistByID(int id)
        {
            var data = await _artistService.GetArtistByID(id);
            _logger.LogWarning($"GetArtistByID method executed: {data}");
            return StatusCode(data.StatusCode, data);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddArtist(ArtistCreateDTO artistCreateDTO)
        {
            var data = await _artistService.AddArtist(artistCreateDTO);
            _logger.LogWarning($"AddArtist method executed: {data}");
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteArtist(int Id)
        {
            var data = await _artistService.DeleteArtist(Id);
            _logger.LogWarning($"DeleteArtist method executed: {data}");
            return StatusCode(data.StatusCode, data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateArtist(ArtistUpdateDTO artistUpdateDTO)
        {
            var data = await _artistService.UpdateArtist(artistUpdateDTO);
            _logger.LogWarning($"UpdateArtist method executed: {data}");
            return StatusCode(data.StatusCode, data);
        }
    }
}
