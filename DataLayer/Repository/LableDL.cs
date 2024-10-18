using AutoMapper;
using DataLayer.Constants.DBConnection;
using DataLayer.Constants.Exceptions;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.DTO.Label;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace DataLayer.Repository
{
    public class LableDL : ILabel
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public LableDL(DataContext dataContext,IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ResponseBody<bool>> AddLabelToNoteAsync(int noteId, int labelId)
        {
            try
            {
                var note = await _dataContext.Note.FirstOrDefaultAsync(n => n.Id == noteId);
                var label = await _dataContext.Label.FirstOrDefaultAsync(l => l.Id == labelId);

                if (note == null || label == null)
                {
                    return new ResponseBody<bool>
                    {
                        Data = false,
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Note or Label not found"
                    };
                }

                note.Labels.Add(label);
                await _dataContext.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Label added to note successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<LabelCreationResponseDto>> CreateLabelAsync(LableCreationRequestDto lableCreationRequestDto)
        {
            try
            {
                var LabelName = await _dataContext.Label.FirstOrDefaultAsync(label => label.Name == lableCreationRequestDto.Name);
                if (LabelName != null)
                {
                    throw new LabelAllredyExistsException(LabelName);
                }

                Label label=new Label
                {
                    Name= lableCreationRequestDto.Name
                };

                await _dataContext.Label.AddAsync(label);
                var changes=await _dataContext.SaveChangesAsync();

                if (changes < 0)
                {
                    throw new Exception("Data base Error");
                }
                var responseDto = _mapper.Map<LabelCreationResponseDto>(label);
                return new ResponseBody<LabelCreationResponseDto>
                {
                   Data=responseDto,
                   Success=true,
                   StatusCode=HttpStatusCode.OK,
                   Message="label created"
                };

            }catch (LabelAllredyExistsException ex)
            {
                return new ResponseBody<LabelCreationResponseDto>
                {
                    Data=null,
                    Success=false,
                    StatusCode=HttpStatusCode.InternalServerError,
                    Message=ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> DeleteLabelAsync(int labelId)
        {
            try
            {
                var label = await _dataContext.Label.FirstOrDefaultAsync(l=>l.Id==labelId);
                if (label == null)
                {
                    return new ResponseBody<bool>
                    {
                        Data = false,
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Label not found"
                    };
                }

                _dataContext.Label.Remove(label);
                await _dataContext.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Label deleted successfully"
                };
            }catch(Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetAllLabelsAsync()
        {
            try
            {
                var labels = await _dataContext.Label.ToListAsync();

                var lableDtos = _mapper.Map<IEnumerable<LabelResponseDto>>(labels);
                return new ResponseBody<IEnumerable<LabelResponseDto>>
                {
                    Data = lableDtos,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Labels retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<IEnumerable<LabelResponseDto>>
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<LabelResponseDto>> GetLabelByIdAsync(int labelId)
        {
            try
            {
                var label = await _dataContext.Label.FindAsync(labelId);
                if (label == null)
                {
                    return new ResponseBody<LabelResponseDto>
                    {
                        Data = null,
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Label not found"
                    };
                }

                var labelDto=_mapper.Map<LabelResponseDto>(label);
                return new ResponseBody<LabelResponseDto>
                {
                    Data = labelDto,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Label retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<LabelResponseDto>
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<IEnumerable<LabelResponseDto>>> GetLabelsByNoteIdAsync(int noteId)
        {
            try
            {
                var note = await _dataContext.Note.Include(n => n.Labels)
                                                   .FirstOrDefaultAsync(n => n.Id == noteId);
                if (note == null)
                {
                    return new ResponseBody<IEnumerable<LabelResponseDto>>
                    {
                        Data = null,
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Note not found"
                    };
                }

                var noteDtos = _mapper.Map<IEnumerable<LabelResponseDto>>(note);
                return new ResponseBody<IEnumerable<LabelResponseDto>>
                {
                    Data = noteDtos,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Labels retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<IEnumerable<LabelResponseDto>>
                {
                    Data = null,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }

        public async Task<ResponseBody<bool>> UpdateLabelAsync(LableUpdateRequestDto lableUpdateRequestDto)
        {
            try
            {
                var existingLabel = await _dataContext.Label.FindAsync(lableUpdateRequestDto.Id);
                if (existingLabel == null)
                {
                    return new ResponseBody<bool>
                    {
                        Data = false,
                        Success = false,
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Label not found"
                    };
                }

                _mapper.Map(lableUpdateRequestDto, existingLabel);
                _dataContext.Label.Update(existingLabel);
                await _dataContext.SaveChangesAsync();

                return new ResponseBody<bool>
                {
                    Data = true,
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Label updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseBody<bool>
                {
                    Data = false,
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = ex.Message
                };
            }
        }
    }
}
