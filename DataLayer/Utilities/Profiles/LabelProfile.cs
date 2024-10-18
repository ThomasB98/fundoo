using AutoMapper;
using ModelLayer.Model.DTO.Label;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Profiles
{
    public class LabelProfile :  Profile
    {
        public LabelProfile() {
            CreateMap<Label,LabelRequestDto>();
            CreateMap<LabelRequestDto, Label>();


            CreateMap<LabelResponseDto, Label>();
            CreateMap<Label, LabelResponseDto>();

            CreateMap<LabelCreationRequestWithNoteIdDto, Label>();
            CreateMap<Label, LabelCreationRequestWithNoteIdDto>();

            CreateMap<Label, LabelCreationResponseDto>();
            CreateMap<LabelCreationResponseDto,Label>();

            CreateMap<Label, LableCreationRequestDto>();
            CreateMap<LableCreationRequestDto, Label>();



        }
    }
}
