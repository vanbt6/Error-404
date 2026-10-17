namespace CafeteriaJam.Logica
{
    /// <summary>Compara lo que preparó el jugador contra lo que pidió el cliente.</summary>
    public static class CafeValidator
    {
        public static bool EsCorrecta(Comanda esperada, Comanda preparada)
        {
            return esperada == preparada;
        }

        /// <summary>Sobrecarga cómoda para la UI: recibe los 3 ingredientes sueltos.</summary>
        public static bool EsCorrecta(Pedido pedido, IngredienteBase baseIngrediente, TipoCafe cafe, Complemento complemento)
        {
            return EsCorrecta(pedido.ComandaEsperada, new Comanda(baseIngrediente, cafe, complemento));
        }
    }
}