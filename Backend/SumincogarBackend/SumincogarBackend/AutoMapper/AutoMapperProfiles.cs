using AutoMapper;
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.AutoMapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearCategoria, Categorium>();
            CreateMap<Categorium, BuscarCategoria>();

            CreateMap<CrearProducto, Producto>();
            CreateMap<Producto, BuscarProducto>().ForMember(
                dest => dest.CategoriaNombre,
                opt => opt.MapFrom(src => src.Categoria!.Categorianombre)
            ); ;
        }
    }
}
