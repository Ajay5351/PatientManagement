using AutoMapper;
using PatientManagement.Models;

namespace PatientManagement.BusinessLogic.Helpers
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<PatientCreateRequest, PatientModel>().ReverseMap();

            CreateMap<PatientUpdateRequest, PatientModel>().ReverseMap();
        }
    }
}
