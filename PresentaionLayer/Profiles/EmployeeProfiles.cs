using AutoMapper;
using DataAccessLayer.Models;
using PresentaionLayer.ViewModels;

namespace PresentaionLayer.Profiles
{
    public class EmployeeProfiles : Profile
    {
        public EmployeeProfiles()
        {
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();
        }
    }
}
