using Art.Abstractions.IServices;
using Art.DTOs;
using Art.DTOs.PictureDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Art.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PictureController : ControllerBase
    {
        private readonly ILogger<PictureController> _logger;
        private readonly IPictureService _pictureService;
        public PictureController(ILogger<PictureController> logger, IPictureService pictureService)
        {
            _logger = logger;
            _pictureService = pictureService;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPictures()
        {
            var data = await _pictureService.GetAllPicture();
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPictureID(int id)
        {
            var data = await _pictureService.GetPictureById(id);
            return StatusCode(data.StatusCode, data);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> AddPicture(PictureCreateDTO pictureCreateDTO)
        {
            var data = await _pictureService.AddPicture(pictureCreateDTO);
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var data = await _pictureService.DeletePicture(id);
            return StatusCode(data.StatusCode, data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePicture(PictureUpdateDTO pictureUpdateDTO, int id)
        {
            var data = await _pictureService.UpdatePicture(pictureUpdateDTO, id);
            return StatusCode(data.StatusCode, data);
        }
        [HttpGet("[action]/{artistId}")]
        public async Task<IActionResult> GetAllPictureByArtistId(int artistId)
        {
            var data = await _pictureService.GetAllPictureByArtistId(artistId);
            return StatusCode(data.StatusCode, data);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> ChangeArtist(int picId, int newArtistId)
        {
            var data = await _pictureService.ChangeArtist(picId, newArtistId);
            return StatusCode(data.StatusCode, data);
        }
    }
}
