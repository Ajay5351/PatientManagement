using AutoMapper;
using PatientManagement.Data;
using PatientManagement.Models;

namespace PatientManagement.Helpers
{
    public class ApplicationHelper : Profile
    {
        public ApplicationHelper()
        {
            CreateMap<Patient, PatientModel>().ReverseMap();
        }
    }
}
