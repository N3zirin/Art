using Art.Abstractions.IRepositories;
using Art.Abstractions.IServices;
using Art.Abstractions.IUnitOfWorks;
using Art.DTOs;
using Art.DTOs.ArtistDTOs;
using Art.Entities;
using Art.Implementations.UnitOfWorks;
using Art.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Art.Implementations.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Artist> _artistRepository;

        public ArtistService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this._artistRepository = _unitOfWork.GetRepository<Artist>();
        }

        public async Task<ResponseModel<ArtistCreateDTO>> AddArtist(ArtistCreateDTO artistCreateDTO)
        {
            try
            {
                if (artistCreateDTO != null)
                {
                    await _artistRepository.AddAsync(new()
                    {
                        ArtistName = artistCreateDTO.ArtistName,
                    });

                    var affectedRows = await _unitOfWork.SaveChangesAsync();


                    if (affectedRows > 0)
                    {
                        return new ResponseModel<ArtistCreateDTO>
                        {
                            Data = artistCreateDTO,
                            StatusCode = 201
                        };
                    }
                    else
                    {
                        return new ResponseModel<ArtistCreateDTO>
                        {
                            Data = null,
                            StatusCode = 500
                        };
                    }
                }
                else
                {
                    return new ResponseModel<ArtistCreateDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<ArtistCreateDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<bool>> DeleteArtist(int Id)
        {
            try
            {
                Artist artist = await _artistRepository.GetByIdAsync(Id);
                if (artist != null)
                {
                    _artistRepository.Remove(artist);

                    var affectedRows = await _unitOfWork.SaveChangesAsync();

                    if (affectedRows > 0)
                    {
                        return new ResponseModel<bool>
                        {
                            Data = true,
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ResponseModel<bool>
                        {
                            Data = false,
                            StatusCode = 500
                        };
                    }
                }
                else
                {
                    return new ResponseModel<bool>
                    {
                        Data = false,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<List<ArtistGetDTO>>> GetAllArtists()
        {
            try
            {
                List<Artist> artistList = await _artistRepository.GetAll().ToListAsync();

                if (artistList.Count > 0)
                {
                    List<ArtistGetDTO> artists = _mapper.Map<List<ArtistGetDTO>>(artistList);
                    return new ResponseModel<List<ArtistGetDTO>>
                    {
                        Data = artists,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<List<ArtistGetDTO>>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<List<ArtistGetDTO>>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<ArtistGetDTO>> GetArtistByID(int id)
        {
            try
            {
                Artist artist = await _artistRepository.GetByIdAsync(id);
                if (artist != null)
                {
                    ArtistGetDTO artistDTO = _mapper.Map<ArtistGetDTO>(artist);
                    return new ResponseModel<ArtistGetDTO>
                    {
                        Data = artistDTO,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<ArtistGetDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<ArtistGetDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<ArtistUpdateDTO>> UpdateArtist(ArtistUpdateDTO artistUpdateDTO)
        {
            try
            {
                if (artistUpdateDTO != null)
                {
                    Artist artist = await _artistRepository.GetByIdAsync(artistUpdateDTO.Id);
                    if (artist != null)
                    {
                        artist.ArtistName = artistUpdateDTO.ArtistName;

                        _artistRepository.Update(artist);
                        var affectedRows = await _unitOfWork.SaveChangesAsync();

                        if (affectedRows > 0)
                        {
                            return new ResponseModel<ArtistUpdateDTO>
                            {
                                Data = artistUpdateDTO,
                                StatusCode = 200
                            };
                        }
                        else
                        {
                            return new ResponseModel<ArtistUpdateDTO>
                            {
                                Data = null,
                                StatusCode = 500
                            };
                        }
                    }
                    else
                    {
                        return new ResponseModel<ArtistUpdateDTO>
                        {
                            Data = null,
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new ResponseModel<ArtistUpdateDTO>
                    {
                        Data = artistUpdateDTO,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<ArtistUpdateDTO>
                {
                    Data = artistUpdateDTO,
                    StatusCode = 500
                };
            }
        }
    }
}
