using AutoMapper;
using AutoTagBackEnd.Dto;
using AutoTagBackEnd.Models;

namespace AutoTagBackEnd
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Portal, PortalDto>();
		}
	}
}

