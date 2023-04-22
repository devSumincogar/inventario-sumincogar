using AutoMapper;
using SumincogarBackend.DTO.CatalogoDTO;
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.ParametroTecnicoDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.DTO.PromocionDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.AutoMapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearCategoria, Categoria>();
            CreateMap<Categoria, BuscarCategoria>();

            CreateMap<CrearFichaTecnica, Fichatecnica>()
                .ForMember(x => x.DocumentoUrl, options => options.Ignore());
            CreateMap<Fichatecnica, BuscarFichaTecnica>()
                .ForMember(dest => dest.CategoriaName, 
                    opt => opt.MapFrom(src => src.Categoria!.CategoriaNombre))
                .ForMember(dest => dest.DocumentoUrl,
                    opt => opt.MapFrom(src => src.DocumentoUrl == null ? "" : src.DocumentoUrl));

            CreateMap<CrearParametroTecnico, Parametrotecnico>();
            CreateMap<Parametrotecnico, BuscarParametroTecnico>();

            CreateMap<CrearCatalogo, Catalogo>()
                .ForMember(x => x.Url, options => options.Ignore())
                .ForMember(x => x.ImagenUrl, options => options.Ignore());
            CreateMap<Catalogo, BuscarCatalogo>()
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url == null ? "" : src.Url))
                .ForMember(dest => dest.ImagenUrl,
                    opt => opt.MapFrom(src => src.ImagenUrl == null ? "" : src.ImagenUrl));

            CreateMap<CrearPromocion, Promocion>()
                .ForMember(x => x.ImagenPrincipal, options => options.Ignore());
            CreateMap<Promocion, BuscarPromocion>()
                .ForMember(dest => dest.ImagenPrincipal,
                    opt => opt.MapFrom(src => src.ImagenPrincipal == null ? "" : src.ImagenPrincipal))
                .ForMember(dest => dest.Imagenes,
                    opt => opt.MapFrom(src => src.Promocionimagen));
            CreateMap<CrearPromocionImagen, Promocionimagen>()
                .ForMember(x => x.Url, options => options.Ignore());
            CreateMap<Promocionimagen, BuscarPromocionImagen>()
                .ForMember(dest => dest.Url,
                    opt => opt.MapFrom(src => src.Url == null ? "" : src.Url));

            CreateMap<CrearProducto, Producto>()
                .ForMember(x => x.ImagenUrl, options => options.Ignore());
            CreateMap<Producto, BuscarProducto>().ForMember(
                dest => dest.ImagenUrl,
                opt => opt.MapFrom(src => src.ImagenUrl == null ? "" : src.ImagenUrl)
            ).ForMember(dest => dest.Imagenes,
                    opt => opt.MapFrom(src => src.Imagenreferencial));
            CreateMap<CrearImagenReferencial, Imagenreferencial>()
                .ForMember(x => x.Url, options => options.Ignore());
            CreateMap<Imagenreferencial, BuscarImagenRefencial>().ForMember(
                dest => dest.Url,
                opt => opt.MapFrom(src => src.Url == null ? "" : src.Url)
            );

            CreateMap<Detalleinventario, BuscarDetalleInventario>();
        }
    }
}
