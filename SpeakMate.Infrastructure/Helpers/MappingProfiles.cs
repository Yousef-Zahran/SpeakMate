using AutoMapper;
using SpeakMate.Core.DTOs;
using SpeakMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeakMate.Infrastructure.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderDisplayName, opt => opt.MapFrom(src => src.Sender.DisplayName))
                .ForMember(dest => dest.SenderImageUrl, opt => opt.MapFrom(src => src.Sender.ImageUrl))
                .ForMember(dest => dest.RecipientDisplayName, opt => opt.MapFrom(src => src.Recipient.DisplayName))
                .ForMember(dest => dest.RecipientImageUrl, opt => opt.MapFrom(src => src.Recipient.ImageUrl));

            CreateMap<MessageCorrection, MessageCorrectionDto>()
            .ForMember(d => d.OriginalContent, o => o.MapFrom(s => s.Message.Content))
            .ForMember(d => d.CorrectedByName, o => o.MapFrom(s => s.CorrectedBy.DisplayName));

            CreateMap<SavedWord, SavedWordDto>();
        }
    }
}
