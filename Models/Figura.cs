using System;

namespace FigurasGeometricas.Models
{
    public abstract class Figura
    {
        public string Nombre { get; set; }

        public Figura(string nombre)
        {
            Nombre = nombre;
        }

        public abstract double CalcularArea();
        public abstract double CalcularPerimetro();

        public virtual void MostrarDatos()
        {
            Console.WriteLine($"\n🔹 {Nombre}");
            Console.WriteLine($"   Área: {CalcularArea():0.00}");
            Console.WriteLine($"   Perímetro: {CalcularPerimetro():0.00}");
        }
    }
}
