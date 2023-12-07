using Art.DTOs.ArtistDTOs;
using Art.DTOs.PictureDTOs;
using Art.Entities;
using AutoMapper;

namespace Art.Mappings
{   
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Picture, PictureGetDTO>()
                  .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.ArtistName))
                  .ReverseMap();

            CreateMap<Artist, ArtistGetDTO>().ReverseMap();
        }
    }
}
