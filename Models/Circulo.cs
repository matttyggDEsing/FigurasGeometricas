using FigurasGeometricas.Models;
using System;

namespace FigurasGeometricas.Models
{
    public class Circulo : Figura
    {
        public double Radio { get; set; }

        public Circulo(string nombre, double radio) : base(nombre)
        {
            Radio = radio;
        }

        public override double CalcularArea() => Math.PI * Radio * Radio;
        public override double CalcularPerimetro() => 2 * Math.PI * Radio;
    }
}
