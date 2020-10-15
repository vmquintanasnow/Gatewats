using AutoMapper;
using Musala.Business.DTO;
using Musala.Domain.Entity;

namespace Musala.Business.Mappers
{
    public class GatewayProfile : Profile
    {
        public GatewayProfile()
        {
            CreateMap<Gateway, GatewayDto>()
                .ForMember(dest=>dest.Id,opt=>opt.MapFrom(src=>src.Id.ToString()));
            CreateMap<SaveGatewayDto, Gateway>();
        }
    }

    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceDto>();
            CreateMap<SaveDeviceDto, Device>();
        }
    }

}
