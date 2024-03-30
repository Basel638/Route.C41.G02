using AutoMapper;
using Route.C41.G02.DAL.Models;
using Route.C41.G02.PL.ViewModels;

namespace Route.C41.G02.PL.Helpers
{
	public class MappingProfiles :Profile
	{
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();      
        }

    }
}
