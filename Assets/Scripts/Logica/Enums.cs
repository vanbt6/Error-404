namespace CafeteriaJam.Logica
{
    /// <summary>Perfil del cliente según el GDD (sección 5).</summary>
    public enum TipoCliente
    {
        Normal,
        Raro,
        Macabro
    }

    /// <summary>Los 4 finales posibles (GDD sección 6).</summary>
    public enum TipoFinal
    {
        RenunciaDigna,
        EquilibrioAsertivo,
        DesechoDesechado,
        SocioEnLasSombras
    }

    /// <summary>Paso 1 de la preparación. Las variantes macabras son provisionales: las define el equipo de narrativa.</summary>
    public enum IngredienteBase
    {
        Leche,
        Hielo,
        Solo,
        Sangre // macabro
    }

    /// <summary>Paso 2 de la preparación. Una sola opción fija por comanda.</summary>
    public enum TipoCafe
    {
        Sencillo,
        Doble,
        ConVeneno // macabro
    }

    /// <summary>Paso 3 de la preparación. Una sola opción fija (sin selecciones acumuladas).</summary>
    public enum Complemento
    {
        Ninguno,
        Crema,
        Canela,
        Dedos // macabro
    }

    /// <summary>Lo que devuelve TurnoModel.Entregar().</summary>
    public enum ResultadoEntrega
    {
        Correcta,
        ErrorPreparacion
    }
}