﻿using AutoMapper;
using SumincogarBackend.DTO.CatalogoDTO;
using SumincogarBackend.DTO.CategoriaDTO;
using SumincogarBackend.DTO.DetalleInventarioDTO;
using SumincogarBackend.DTO.FichaTecnicaDTO;
using SumincogarBackend.DTO.GamaColorDTO;
using SumincogarBackend.DTO.ParametroTecnicoDTO;
using SumincogarBackend.DTO.ProductoDTO;
using SumincogarBackend.DTO.PromocionDTO;
using SumincogarBackend.DTO.SubCategoriaDTO;
using SumincogarBackend.DTO.UsuariosDTO;
using SumincogarBackend.Models;

namespace SumincogarBackend.AutoMapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CrearCategoria, Categoria>();
            CreateMap<Categoria, BuscarCategoria>();

            CreateMap<CrearSubCategoria, SubCategoria>();
            CreateMap<SubCategoria, BuscarSubCategoria>();

            CreateMap<CrearGamaColor, GamaColor>();
            CreateMap<GamaColor, BuscarGamaColor>();

            CreateMap<CrearFichaTecnica, Fichatecnica>()
                .ForMember(x => x.DocumentoUrl, options => options.Ignore());
            CreateMap<Fichatecnica, BuscarFichaTecnica>()
                .ForMember(dest => dest.SubCategoriaName, 
                    opt => opt.MapFrom(src => src.Subcategoria!.SubcategoriaNombre))
                .ForMember(dest => dest.DocumentoUrl,
                    opt => opt.MapFrom(src => src.DocumentoUrl == null ? "" : src.DocumentoUrl))
                .ForMember(dest => dest.Parametros,
                    opt => opt.MapFrom(src => src.Parametrotecnico));

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

            CreateMap<Detalleinventario, BuscarDetalleInventario>().ForMember(
                dest => dest.Orden,
                opt => opt.MapFrom(src => src.Stock!.Equals("ALTO") ? 1 : src.Stock!.Equals("MEDIO") ? 2 : src.Stock!.Equals("BAJO") ? 3 : 4)
            );

            CreateMap<CrearUsuarioDTO, Usuario>();

        }
    }
}
