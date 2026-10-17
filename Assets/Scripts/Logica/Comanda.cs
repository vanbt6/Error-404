using System;

namespace CafeteriaJam.Logica
{
    /// <summary>
    /// Los 3 pasos de una preparación: base, café y complemento.
    /// Se usa tanto para lo que pide el cliente como para lo que prepara el jugador.
    /// </summary>
    public readonly struct Comanda : IEquatable<Comanda>
    {
        public readonly IngredienteBase Base;
        public readonly TipoCafe Cafe;
        public readonly Complemento Complemento;

        public Comanda(IngredienteBase baseIngrediente, TipoCafe cafe, Complemento complemento)
        {
            Base = baseIngrediente;
            Cafe = cafe;
            Complemento = complemento;
        }

        /// <summary>True si alguno de los 3 pasos lleva un ingrediente macabro.</summary>
        public bool EsPerturbadora
        {
            get
            {
                return Base == IngredienteBase.Sangre
                    || Cafe == TipoCafe.ConVeneno
                    || Complemento == Complemento.Dedos;
            }
        }

        public bool Equals(Comanda otra)
        {
            return Base == otra.Base && Cafe == otra.Cafe && Complemento == otra.Complemento;
        }

        public override bool Equals(object obj)
        {
            return obj is Comanda && Equals((Comanda)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (int)Base;
                hash = hash * 31 + (int)Cafe;
                hash = hash * 31 + (int)Complemento;
                return hash;
            }
        }

        public static bool operator ==(Comanda a, Comanda b) { return a.Equals(b); }
        public static bool operator !=(Comanda a, Comanda b) { return !a.Equals(b); }

        public override string ToString()
        {
            return Base + " + " + Cafe + " + " + Complemento;
        }
    }
}