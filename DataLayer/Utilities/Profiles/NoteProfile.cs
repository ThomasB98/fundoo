using AutoMapper;
using ModelLayer.Model.DTO.Note;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<NoteDto, Note>();
            CreateMap<Note, NoteDto>();

            CreateMap<Note,NoteRequestDto>();
            CreateMap<NoteRequestDto, Note>();

            CreateMap<Note, NoteResponseDto>();
            CreateMap<NoteResponseDto, Note>();
        }
    }
}
