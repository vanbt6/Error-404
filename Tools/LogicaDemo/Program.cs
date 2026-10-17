using System;
using System.Collections.Generic;
using CafeteriaJam.Logica;

// Simula partidas en consola para probar la lógica sin Unity.
public static class Program
{
    // Acciones posibles por cliente: "ok" (entrega correcta), "error", "rechazar", "ok+N" (correcta y cede N extras)
    private static TurnoModel Jugar(string[] acciones, bool imprimir)
    {
        var turno = new TurnoModel(PedidosDelSlice.Crear());
        if (imprimir)
        {
            turno.Sanidad.SanidadCambio += (a, n) => Console.WriteLine("    sanidad " + a + " -> " + n);
            turno.DineroCambio += (a, n) => Console.WriteLine("    dinero  $" + a + " -> $" + n);
            turno.TurnoTerminado += f => Console.WriteLine("    >> FIN DEL TURNO: " + f);
        }

        for (int i = 0; i < acciones.Length && !turno.TurnoFinalizado; i++)
        {
            Pedido p = turno.PedidoActual;
            string accion = acciones[i];
            if (imprimir) Console.WriteLine("  [" + p.Tipo + "] " + accion);

            if (accion == "rechazar")
                turno.Rechazar();
            else if (accion == "error")
                turno.Entregar(new Comanda(IngredienteBase.Solo, TipoCafe.Sencillo, Complemento.Ninguno));
            else
            {
                turno.Entregar(p.ComandaEsperada);
                int cesiones = accion.StartsWith("ok+") ? int.Parse(accion.Substring(3)) : 0;
                for (int c = 0; c < cesiones && turno.PuedeCederExtra; c++)
                    turno.CederPeticionExtra();
            }

            if (!turno.TurnoFinalizado && turno.HayMasClientes) turno.SiguienteCliente();
        }

        if (!turno.TurnoFinalizado) turno.TerminarTurno();
        return turno;
    }

    public static void Main()
    {
        var escenarios = new Dictionary<string, string[]>
        {
            { "Juego 'perfecto' sin cruzar líneas", new[] { "ok", "ok", "rechazar" } },
            { "Cede al raro y acepta al macabro",  new[] { "ok", "ok+2", "ok" } },
            { "Todo sale mal",                     new[] { "error", "error", "rechazar" } },
            { "Colapso mental (game over)",        new[] { "error", "error", "ok" } },
        };

        foreach (var e in escenarios)
        {
            Console.WriteLine(e.Key);
            var t = Jugar(e.Value, true);
            Console.WriteLine("    resultado: sanidad " + t.Sanidad.Sanidad + ", $" + t.Dinero + " -> " + t.FinalObtenido + "\n");
        }

        // Recorre TODAS las combinaciones de decisiones para ver qué finales se pueden alcanzar.
        string[] normal = { "ok", "error", "rechazar" };
        string[] raro = { "ok", "ok+1", "ok+2", "ok+3", "ok+4", "error", "rechazar" };
        string[] macabro = { "ok", "error", "rechazar" };
        var conteo = new Dictionary<TipoFinal, int>();
        foreach (TipoFinal f in Enum.GetValues(typeof(TipoFinal))) conteo[f] = 0;

        foreach (var a in normal)
            foreach (var b in raro)
                foreach (var c in macabro)
                    conteo[Jugar(new[] { a, b, c }, false).FinalObtenido.Value]++;

        Console.WriteLine("Finales alcanzables (de " + (normal.Length * raro.Length * macabro.Length) + " combinaciones):");
        foreach (var kv in conteo) Console.WriteLine("    " + kv.Key + ": " + kv.Value);
    }
}