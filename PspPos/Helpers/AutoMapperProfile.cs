using AutoMapper;
using PspPos.Models;

namespace PspPos.Helpers;
public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Sample, SampleViewModel>();
        CreateMap<SamplePostModel, Sample>();
    }
}

