using AutoMapper;
using ModelLayer.Model.DTO;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile() {
            CreateMap<NoteDto, Note>();
            CreateMap<Note, NoteDto>();
        }
    }
}
