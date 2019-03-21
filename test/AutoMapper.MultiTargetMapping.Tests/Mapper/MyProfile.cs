using AutoMapper.MultiTargetMapping.Tests.Models;

namespace AutoMapper.MultiTargetMapping.Tests.Mapper
{

    public class MyProfile
       : Profile
    {

        public MyProfile()
        {
            this.CreateMap<AModel, BModel>()
                .ForMember(x => x.Field3, x => x.MapFrom(z => z.Field1 + "_b"))
                .ForMember(x => x.Field4, x => x.MapFrom(z => z.Field2 + "_b"));
            this.CreateMap<AModel, CModel>()
                .ForMember(x => x.Field5, x => x.MapFrom(z => z.Field1 + "_c"))
                .ForMember(x => x.Field6, x => x.MapFrom(z => z.Field2 + "_c"));
        }

    }

}
