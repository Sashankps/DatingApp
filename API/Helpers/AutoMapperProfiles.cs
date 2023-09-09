using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            CreateMap<MemberUpdateDTO, AppUser>().ReverseMap();
            CreateMap<RegisterDTO, AppUser>().ReverseMap();
            CreateMap<Message, MessageDTO>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                .FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                .FirstOrDefault(x => x.IsMain).Url)).ReverseMap();
        }
    }
}