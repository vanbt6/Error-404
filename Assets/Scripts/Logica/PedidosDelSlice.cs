using System.Collections.Generic;

namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Los 3 clientes del vertical slice, en orden. Textos y comandas son provisionales
    /// hasta que guion y game design los definan.
    /// </summary>
    public static class PedidosDelSlice
    {
        public static List<Pedido> Crear()
        {
            return new List<Pedido>
            {
                new Pedido(
                    id: "cliente_normal",
                    nombreCliente: "Cliente normal",
                    tipo: TipoCliente.Normal,
                    textoOrden: "Un café con leche y canela, por favor.",
                    comandaEsperada: new Comanda(IngredienteBase.Leche, TipoCafe.Sencillo, Complemento.Canela),
                    pagoBase: ReglasJuego.PagoNormal,
                    efectoSanidadAlEntregar: +ReglasJuego.SanidadOrdenExitosa),

                new Pedido(
                    id: "cliente_raro",
                    nombreCliente: "Cliente raro",
                    tipo: TipoCliente.Raro,
                    textoOrden: "Un doble con hielo y crema. Y apúrate.",
                    comandaEsperada: new Comanda(IngredienteBase.Hielo, TipoCafe.Doble, Complemento.Crema),
                    pagoBase: ReglasJuego.PagoRaro,
                    efectoSanidadAlEntregar: +ReglasJuego.SanidadOrdenExitosa,
                    descuentoPorCesion: ReglasJuego.DescuentoPorCesionRara),

                new Pedido(
                    id: "cliente_macabro",
                    nombreCliente: "Cliente macabro",
                    tipo: TipoCliente.Macabro,
                    textoOrden: "Lo de siempre... con un poco de sangre fresca.",
                    comandaEsperada: new Comanda(IngredienteBase.Sangre, TipoCafe.Sencillo, Complemento.Ninguno),
                    pagoBase: ReglasJuego.PagoMacabro,
                    efectoSanidadAlEntregar: -ReglasJuego.SanidadOrdenMacabra)
            };
        }
    }
}