using Art.Abstractions.IRepositories;
using Art.Abstractions.IServices;
using Art.Abstractions.IUnitOfWorks;
using Art.DTOs;
using Art.DTOs.PictureDTOs;
using Art.Entities;
using Art.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Art.Implementations.Services
{
    public class PictureService : IPictureService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Picture> _pictureRepository;
        private readonly IRepository<Artist> _artistRepository;
        public PictureService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this._pictureRepository = _unitOfWork.GetRepository<Picture>();
            this._artistRepository = _unitOfWork.GetRepository<Artist>();
        }

        public async Task<ResponseModel<PictureCreateDTO>> AddPicture(PictureCreateDTO pictureCreateDTO)
        {
            try
            {
                if (pictureCreateDTO != null)
                {
                    await _pictureRepository.AddAsync(new()
                    {
                        Name = pictureCreateDTO.PictureName,
                        ArtistId = pictureCreateDTO.ArtistID

                    });
                    var affectedRows = await _unitOfWork.SaveChangesAsync();
                    if (affectedRows > 0)
                    {
                        return new ResponseModel<PictureCreateDTO>
                        {
                            Data = pictureCreateDTO,
                            StatusCode = 201
                        };
                    }
                    else
                    {
                        return new ResponseModel<PictureCreateDTO>
                        {
                            Data = null,
                            StatusCode = 500
                        };
                    }
                }
                else
                {
                    return new ResponseModel<PictureCreateDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<PictureCreateDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<bool>> ChangeArtist(ChangeArtistDTO model)
        {
            var picData = await _pictureRepository.GetByIdAsync(model.PictureId);
            if (picData == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

            var aSchool = await _artistRepository.GetByIdAsync(model.NewArtistID);

            if (aSchool == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }
            picData.ArtistId = model.NewArtistID;

            _pictureRepository.Update(picData);


            var rowAffect = await _unitOfWork.SaveChangesAsync();

            if (rowAffect > 0)
            {
                return new ResponseModel<bool>
                {
                    Data = true,
                    StatusCode = 200,
                };
            }
            else
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500,
                };
            }
        }

        public async Task<ResponseModel<bool>> ChangeArtist(int pictureId, int newArtistId)
        {
            var picData = await _pictureRepository.GetByIdAsync(pictureId);
            if (picData == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
            var artistData = await _artistRepository.GetByIdAsync(newArtistId);
            if (artistData == null)
            {
                return new ResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400
                };
            }
            picData.ArtistId = newArtistId;
            _pictureRepository.Update(picData);
            int rowAffected = await _unitOfWork.SaveChangesAsync();
            if (rowAffected > 0)
            {
                return new ResponseModel<bool>
                {
                    Data = true,
                    StatusCode = 200
                };
            }
            else
            {
                return new ResponseModel<bool> { Data = false, StatusCode = 500 };
            }
        }

        public async Task<ResponseModel<bool>> DeletePicture(int id)
        {
            try
            {
                Picture picture = await _pictureRepository.GetByIdAsync(id);
                if (picture != null)
                {
                    _pictureRepository.Remove(picture);

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

        public async Task<ResponseModel<List<PictureGetDTO>>> GetAllPicture()
        {
            try
            {
                List<Picture> pictureList = await _pictureRepository.GetAll().Include(a => a.Artist).ToListAsync();

                if (pictureList.Count > 0)
                {
                    List<PictureGetDTO> pics = _mapper.Map<List<PictureGetDTO>>(pictureList);
                    return new ResponseModel<List<PictureGetDTO>>
                    {
                        Data = pics,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<List<PictureGetDTO>>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<List<PictureGetDTO>>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<List<PictureGetDTO>>> GetAllPictureByArtistId(int ArtistId)
        {
            try
            {
                var pics = await _pictureRepository.GetAll().Where(a => a.ArtistId == ArtistId).ToListAsync();
                if (pics != null)
                {
                    var picsDTO = _mapper.Map<List<PictureGetDTO>>(pics);
                    return new ResponseModel<List<PictureGetDTO>>
                    {
                        Data = picsDTO,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<List<PictureGetDTO>>
                    {
                        Data = null,
                        StatusCode = 404
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);
                return new ResponseModel<List<PictureGetDTO>> { Data = null, StatusCode = 500 };
            }
        }

        public async Task<ResponseModel<PictureGetDTO>> GetPictureById(int id)
        {
            try
            {

                Picture picture = await _pictureRepository.GetByIdAsync(id);
                if (picture != null)
                {
                    await _artistRepository.GetByIdAsync(picture.ArtistId);
                    PictureGetDTO studentGetDTO = _mapper.Map<PictureGetDTO>(picture);
                    return new ResponseModel<PictureGetDTO>
                    {
                        Data = studentGetDTO,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ResponseModel<PictureGetDTO>
                    {
                        Data = null,
                        StatusCode = 400
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + ex.InnerException);

                return new ResponseModel<PictureGetDTO>
                {
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public async Task<ResponseModel<bool>> UpdatePicture(PictureUpdateDTO pictureUpdateDTO, int id)
        {
            try
            {
                if (pictureUpdateDTO != null)
                {
                    Picture picture = await _pictureRepository.GetByIdAsync(id);

                    if (picture != null)
                    {
                        picture.Name = pictureUpdateDTO.PictureName;

                        _pictureRepository.Update(picture);
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

        public async Task<Picture> GetPictureId(int id)
        {
            var data = await _pictureRepository.GetByIdAsync(id);
            _unitOfWork.SaveChangesAsync();
            return data;
        }

    }
}
