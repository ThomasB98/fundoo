using AutoMapper;
using ModelLayer.Model.DTO.Collaborator;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Utilities.Profiles
{
    public class CollaboratorProfile : Profile
    {
        public CollaboratorProfile()
        {
            CreateMap<CollaboratorDto, Collaborator>();
            CreateMap<Collaborator, CollaboratorDto>();

            CreateMap<CollaboratorResponceDto, Collaborator>();
            CreateMap<Collaborator, CollaboratorResponceDto>();

            CreateMap<CollaboratorRequestDto, Collaborator>();
            CreateMap<Collaborator, CollaboratorRequestDto>();

        }
    }
}
