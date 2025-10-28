using FigurasGeometricas.Models;
using System;

namespace FigurasGeometricas.Models
{
    public class Triangulo : Figura
    {
        public double LadoA { get; set; }
        public double LadoB { get; set; }
        public double LadoC { get; set; }

        public Triangulo(string nombre, double a, double b, double c) : base(nombre)
        {
            LadoA = a;
            LadoB = b;
            LadoC = c;
        }

        public override double CalcularPerimetro() => LadoA + LadoB + LadoC;

        public override double CalcularArea()
        {
            // Validar desigualdad triangular
            if (LadoA + LadoB <= LadoC || LadoA + LadoC <= LadoB || LadoB + LadoC <= LadoA)
                return double.NaN; // No es un triángulo válido

            double s = CalcularPerimetro() / 2;
            return Math.Sqrt(s * (s - LadoA) * (s - LadoB) * (s - LadoC));
        }

        public string TipoTriangulo()
        {
            if (LadoA == LadoB && LadoB == LadoC)
                return "Equilátero";
            else if (LadoA == LadoB || LadoA == LadoC || LadoB == LadoC)
                return "Isósceles";
            else
                return "Escaleno";
        }
    }
}
