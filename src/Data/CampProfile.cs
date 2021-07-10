using AutoMapper;
using CoreCodeCamp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            // Mapping between Camp and CampModel
            this.CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName))
                .ReverseMap();

            // Mapping between Talk and TalkModel
            this.CreateMap<Talk, TalkModel>()
                .ReverseMap();

            // Mapping between Speaker and SpeakerModel
            this.CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
