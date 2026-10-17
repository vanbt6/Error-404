namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Datos de un cliente y su orden. No tiene lógica: el TurnoModel lee estos valores.
    /// </summary>
    public class Pedido
    {
        public string Id { get; private set; }
        public string NombreCliente { get; private set; }
        public TipoCliente Tipo { get; private set; }
        public string TextoOrden { get; private set; }
        public Comanda ComandaEsperada { get; private set; }

        /// <summary>Dinero que paga si la entrega es correcta.</summary>
        public int PagoBase { get; private set; }

        /// <summary>Cambio de sanidad al entregar correctamente (ej. +20 normal, -40 macabro).</summary>
        public int EfectoSanidadAlEntregar { get; private set; }

        /// <summary>Cambio de sanidad al rechazar la orden.</summary>
        public int EfectoSanidadAlRechazar { get; private set; }

        /// <summary>Dinero que se pierde por cada petición extra cedida (solo cliente raro).</summary>
        public int DescuentoPorCesion { get; private set; }

        public bool RequiereIngredientesPerturbadores { get { return ComandaEsperada.EsPerturbadora; } }
        public bool PuedePedirExtras { get { return Tipo == TipoCliente.Raro; } }

        public Pedido(
            string id,
            string nombreCliente,
            TipoCliente tipo,
            string textoOrden,
            Comanda comandaEsperada,
            int pagoBase,
            int efectoSanidadAlEntregar,
            int efectoSanidadAlRechazar = ReglasJuego.SanidadRechazo,
            int descuentoPorCesion = 0)
        {
            Id = id;
            NombreCliente = nombreCliente;
            Tipo = tipo;
            TextoOrden = textoOrden;
            ComandaEsperada = comandaEsperada;
            PagoBase = pagoBase;
            EfectoSanidadAlEntregar = efectoSanidadAlEntregar;
            EfectoSanidadAlRechazar = efectoSanidadAlRechazar;
            DescuentoPorCesion = descuentoPorCesion;
        }
    }
}