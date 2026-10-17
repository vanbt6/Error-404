using System;
using System.Collections.Generic;

namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Estado completo del turno. Es la única clase con la que Unity necesita hablar.
    ///
    /// Flujo por cliente:
    ///   1. Leer PedidoActual (texto, pago, etc.)
    ///   2. Entregar(comanda) o Rechazar()
    ///   3. Solo cliente raro, tras entrega correcta: CederPeticionExtra() las veces que el jugador ceda
    ///   4. SiguienteCliente() o, si ya no hay más, TerminarTurno()
    ///
    /// Si la sanidad llega a 0 en cualquier momento, el turno termina solo
    /// y se dispara TurnoTerminado(DesechoDesechado).
    /// </summary>
    public class TurnoModel
    {
        private readonly List<Pedido> pedidos;
        private int indiceActual;
        private bool pedidoActualResuelto;
        private bool entregaActualCorrecta;
        private int pagoRecibidoActual;

        public SanidadManager Sanidad { get; private set; }
        public int Dinero { get; private set; }
        public int Cuota { get; private set; }
        public bool TurnoFinalizado { get; private set; }
        public TipoFinal? FinalObtenido { get; private set; }

        /// <summary>(dineroAnterior, dineroNuevo).</summary>
        public event Action<int, int> DineroCambio;

        /// <summary>Se dispara una sola vez, ya sea por game over o por TerminarTurno().</summary>
        public event Action<TipoFinal> TurnoTerminado;

        public TurnoModel(IEnumerable<Pedido> pedidosDelTurno, int cuota = ReglasJuego.Cuota)
        {
            pedidos = new List<Pedido>(pedidosDelTurno);
            if (pedidos.Count == 0)
                throw new ArgumentException("El turno necesita al menos un pedido.");

            Cuota = cuota;
            Sanidad = new SanidadManager();
            Sanidad.GameOver += AlLlegarAGameOver;
        }

        public Pedido PedidoActual { get { return pedidos[indiceActual]; } }
        public bool HayMasClientes { get { return indiceActual + 1 < pedidos.Count; } }
        public bool CuotaCumplida { get { return Dinero >= Cuota; } }

        /// <summary>True si al cliente actual todavía se le puede ceder una petición extra.</summary>
        public bool PuedeCederExtra
        {
            get { return !TurnoFinalizado && PedidoActual.PuedePedirExtras && entregaActualCorrecta; }
        }

        public ResultadoEntrega Entregar(Comanda preparada)
        {
            ValidarQueSePuedeResolver();
            pedidoActualResuelto = true;

            if (!CafeValidator.EsCorrecta(PedidoActual.ComandaEsperada, preparada))
            {
                // Error: no se cobra y baja la sanidad.
                Sanidad.AplicarErrorPreparacion();
                return ResultadoEntrega.ErrorPreparacion;
            }

            entregaActualCorrecta = true;
            pagoRecibidoActual = PedidoActual.PagoBase;
            CambiarDinero(pagoRecibidoActual);
            Sanidad.AplicarCambio(PedidoActual.EfectoSanidadAlEntregar); // puede disparar game over
            return ResultadoEntrega.Correcta;
        }

        public void Rechazar()
        {
            ValidarQueSePuedeResolver();
            pedidoActualResuelto = true;
            Sanidad.AplicarCambio(PedidoActual.EfectoSanidadAlRechazar);
        }

        /// <summary>El cliente raro pide "más crema" y el jugador cede: -5 sanidad y menos dinero.</summary>
        public void CederPeticionExtra()
        {
            if (!PuedeCederExtra)
                throw new InvalidOperationException("Solo se puede ceder al cliente raro después de una entrega correcta.");

            int descuento = Math.Min(PedidoActual.DescuentoPorCesion, pagoRecibidoActual);
            pagoRecibidoActual -= descuento;
            CambiarDinero(-descuento);
            Sanidad.AplicarCesionRara(); // puede disparar game over
        }

        public void SiguienteCliente()
        {
            if (TurnoFinalizado) return;
            if (!pedidoActualResuelto)
                throw new InvalidOperationException("Primero hay que entregar o rechazar el pedido actual.");
            if (!HayMasClientes)
                throw new InvalidOperationException("No hay más clientes; llama TerminarTurno().");

            indiceActual++;
            pedidoActualResuelto = false;
            entregaActualCorrecta = false;
            pagoRecibidoActual = 0;
        }

        /// <summary>Evalúa y devuelve el final. Si ya terminó por game over, devuelve ese final.</summary>
        public TipoFinal TerminarTurno()
        {
            if (FinalObtenido.HasValue) return FinalObtenido.Value;

            TipoFinal final = FinalesEvaluator.Evaluar(Sanidad.Sanidad, Dinero, Cuota);
            Finalizar(final);
            return final;
        }

        private void AlLlegarAGameOver()
        {
            Finalizar(TipoFinal.DesechoDesechado);
        }

        private void Finalizar(TipoFinal final)
        {
            if (TurnoFinalizado) return;
            TurnoFinalizado = true;
            FinalObtenido = final;
            if (TurnoTerminado != null) TurnoTerminado(final);
        }

        private void CambiarDinero(int delta)
        {
            if (delta == 0) return;
            int anterior = Dinero;
            Dinero += delta;
            if (DineroCambio != null) DineroCambio(anterior, Dinero);
        }

        private void ValidarQueSePuedeResolver()
        {
            if (TurnoFinalizado)
                throw new InvalidOperationException("El turno ya terminó.");
            if (pedidoActualResuelto)
                throw new InvalidOperationException("Este pedido ya se resolvió; llama SiguienteCliente().");
        }
    }
}