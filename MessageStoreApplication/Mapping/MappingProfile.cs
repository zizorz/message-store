using AutoMapper;
using MessageStoreApplication.Models;

namespace MessageStoreApplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageOutput>();
            CreateMap<MessageInput, Message>()
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.OwnerId, o => o.Ignore());
        }
    }
}