using AutoMapper;
using BookingAPI.Models;
using BookingAPI.Models.Dto;

namespace BookingAPI.Mappers;

public class MappingConfig: Profile
{
    public MappingConfig()
    {
        CreateMap<Property, AddPropertyDto>().ReverseMap();
        CreateMap<Property, PropertyDto>().ReverseMap();    
        CreateMap<Booking, AddBookingDto>().ReverseMap();
        CreateMap<Booking, BookingDto>().ReverseMap();
    }
}
