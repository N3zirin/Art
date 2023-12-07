using Art.DTOs;
using Art.DTOs.ArtistDTOs;
using Art.Models;

namespace Art.Abstractions.IServices
{
    public interface IArtistService //response model uzerinden uygun dtolarla methods threads tasklar yaziriq: async olsun ve
                                    //burda ilkin halini yazaq deye
    {
            public Task<ResponseModel<List<ArtistGetDTO>>> GetAllArtists();
            public Task<ResponseModel<ArtistGetDTO>> GetArtistByID(int id);

            public Task<ResponseModel<ArtistUpdateDTO>> UpdateArtist(ArtistUpdateDTO artistUpdateDTO);

            public Task<ResponseModel<bool>> DeleteArtist(int Id);

            public Task<ResponseModel<ArtistCreateDTO>> AddArtist(ArtistCreateDTO artistCreateDTO);
        }
    }

