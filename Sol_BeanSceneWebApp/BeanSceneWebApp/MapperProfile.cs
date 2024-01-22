using AutoMapper;
using BeanScene.WebApp;
using BeanSceneWebApp.Data;

namespace BeanScene.WebApp
{
    public class MapperProfile : Profile
    {


        public MapperProfile()
        {
            CreateMap<Sitting, BeanSceneWebApp.Areas.Administration.Models.Sitting.Create>().ReverseMap();

            CreateMap<Sitting, BeanSceneWebApp.Areas.Administration.Models.Sitting.Edit>().ReverseMap();
            CreateMap<Reservation, BeanSceneWebApp.Models.Reservation.Create>().ReverseMap();
        }
    }
}
