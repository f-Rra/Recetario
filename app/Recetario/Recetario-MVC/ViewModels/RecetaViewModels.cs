using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class RecetaListaItem : IRecetaBuscada
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int PorcionesBase { get; set; }
    public int CantidadIngredientes { get; set; }
    public int CantidadPasos { get; set; }
    public bool Activo { get; set; }

    /// <summary>
    /// Ingrediente por el que la receta apareció en la búsqueda, cuando no
    /// coincidió por nombre ni por código.
    /// </summary>
    public string? IngredienteCoincidente { get; set; }

    public bool Completa => CantidadIngredientes > 0 && CantidadPasos > 0;

    /// <summary>Qué le falta a la receta, para el título del aviso.</summary>
    public string Advertencia => (CantidadIngredientes, CantidadPasos) switch
    {
        (0, 0) => "No tiene ingredientes ni procedimiento cargados",
        (0, _) => "No tiene ingredientes cargados",
        (_, 0) => "No tiene procedimiento cargado",
        _ => string.Empty
    };

    /// <summary>Lo mismo, en dos palabras: entra en la cinta de la ficha.</summary>
    public string AdvertenciaCorta => (CantidadIngredientes, CantidadPasos) switch
    {
        (0, 0) => "sin ingredientes ni pasos",
        (0, _) => "sin ingredientes",
        (_, 0) => "sin procedimiento",
        _ => string.Empty
    };
}

/// <summary>La pantalla del listado y el formulario de su modal de alta.</summary>
public class RecetasPaginaViewModel
{
    public string? Busqueda { get; set; }
    public int? IdClasificacion { get; set; }
    public List<RecetaListaItem> Lista { get; set; } = new();

    /// <summary>Para el modal de alta; trae el código que va a tocar.</summary>
    public RecetaFormViewModel Nueva { get; set; } = new();

    /// <summary>Se reabre el modal cuando el alta vuelve con errores.</summary>
    public string? ModalAbierto { get; set; }
}

public class RecetaFormViewModel
{
    public int IdReceta { get; set; }

    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty; // autogenerado, solo lectura

    [Required(ErrorMessage = "Ingresá el nombre.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Elegí una clasificación.")]
    [Display(Name = "Clasificación")]
    public int? IdClasificacion { get; set; }

    [Required(ErrorMessage = "Ingresá las porciones base.")]
    [Range(1, 9999, ErrorMessage = "Las porciones deben ser al menos {1}.")]
    [Display(Name = "Porciones base")]
    public int? PorcionesBase { get; set; }

    [Display(Name = "Activa")]
    public bool Activo { get; set; } = true;
}

public class RecetaDetalleViewModel
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int PorcionesBase { get; set; }
    public bool Activo { get; set; }

    public List<IngredienteRecetaItem> Ingredientes { get; set; } = new();
    public List<PasoItem> Pasos { get; set; } = new();

    // Forms inline de la página de edición
    public RecetaFormViewModel Datos { get; set; } = new();
    public IngredienteRecetaFormViewModel NuevoIngrediente { get; set; } = new();
    public PasoFormViewModel NuevoPaso { get; set; } = new();
}

public class IngredienteRecetaItem
{
    public int IdIngrediente { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal CantNeta { get; set; }
    public decimal Rendimiento { get; set; }
    public decimal CantBruta { get; set; }
    public string Unidad { get; set; } = string.Empty;
}

public class PasoItem
{
    public int IdProcedimiento { get; set; }
    public int NroPaso { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class IngredienteRecetaFormViewModel
{
    public int IdReceta { get; set; }

    [Required(ErrorMessage = "Elegí un ingrediente.")]
    [Display(Name = "Ingrediente")]
    public int? IdIngrediente { get; set; }

    [Required(ErrorMessage = "Ingresá la cantidad neta.")]
    [Range(0.0001, 999999, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    [Display(Name = "Cantidad neta")]
    public decimal? CantNeta { get; set; }

    [Required(ErrorMessage = "Ingresá el rendimiento.")]
    [Range(0.01, 100, ErrorMessage = "El rendimiento va de {1} a {2}%.")]
    [Display(Name = "Rendimiento %")]
    public decimal? Rendimiento { get; set; } = 100;
}

public class PasoFormViewModel
{
    public int IdReceta { get; set; }

    [Required(ErrorMessage = "Ingresá la descripción del paso.")]
    [Display(Name = "Descripción del paso")]
    public string Descripcion { get; set; } = string.Empty;
}
