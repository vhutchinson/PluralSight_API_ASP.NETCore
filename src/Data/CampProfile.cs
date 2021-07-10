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
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName)) // Venue in CampModel comes from Camp's Location's VenueName
                .ReverseMap();

            // Mapping between Talk and TalkModel
            this.CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.Camp, opt => opt.Ignore())        // Do not map Camp for TalkModel -> Talk only
                .ForMember(t => t.Speaker, opt => opt.Ignore());    // Do not map Speaker for TalkModel -> Talk only

            // Mapping between Speaker and SpeakerModel
            this.CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
