using AutoMapper;
using BackendTest.Data.Entities;
using BackendTest.Internal.BusinessObjects;

namespace BackendTestWebAPI.RequestHandlers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateNodeDto, Node>();
            CreateMap<EditNodeDto, Node>();
        }
    }
}
