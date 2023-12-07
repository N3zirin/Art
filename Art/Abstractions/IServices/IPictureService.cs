using Art.DTOs;
using Art.DTOs.PictureDTOs;
using Art.Models;

namespace Art.Abstractions.IServices
{
    public interface IPictureService
    {
        Task<ResponseModel<PictureCreateDTO>> AddPicture(PictureCreateDTO pictureCreateDTO);
        Task<ResponseModel<bool>> DeletePicture(int id);
        Task<ResponseModel<bool>> UpdatePicture(PictureUpdateDTO pictureUpdateDTO, int id);


        Task<ResponseModel<List<PictureGetDTO>>> GetAllPicture();
        Task<ResponseModel<PictureGetDTO>> GetPictureById(int id);
        Task<ResponseModel<List<PictureGetDTO>>> GetAllPictureByArtistId(int ArtistId);

        //change artist
        Task<ResponseModel<bool>> ChangeArtist(int pictureId, int newArtistId);
        Task<ResponseModel<bool>> ChangeArtist(ChangeArtistDTO model);

        //  Task<Picture> GetStudentId(int id);

    }
}
