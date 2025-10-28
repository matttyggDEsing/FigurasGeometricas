using FigurasGeometricas.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FigurasGeometricas.Models
{
    public class GestorFiguras
    {
        public List<Figura> ListaFiguras { get; private set; }

        public GestorFiguras()
        {
            ListaFiguras = new List<Figura>();
        }

        public void AgregarFigura(Figura figura)
        {
            ListaFiguras.Add(figura);
        }

        public void EliminarFigura(string nombre)
        {
            var figura = ListaFiguras.FirstOrDefault(f => f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (figura != null)
            {
                ListaFiguras.Remove(figura);
                Console.WriteLine($" '{nombre}' eliminada correctamente.");
            }
            else
            {
                Console.WriteLine($" No se encontró una figura con el nombre '{nombre}'.");
            }
        }

        public void MostrarTodasLasFiguras()
        {
            if (!ListaFiguras.Any())
            {
                Console.WriteLine("No hay figuras registradas.");
                return;
            }

            foreach (var figura in ListaFiguras)
                figura.MostrarDatos();
        }

        public double CalcularAreaTotal() => ListaFiguras.Sum(f => f.CalcularArea());
        public double CalcularPerimetroTotal() => ListaFiguras.Sum(f => f.CalcularPerimetro());
    }
}
