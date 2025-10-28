

// Models/FiguraCreateDto.cs
namespace FigurasGeometricas.Models
{
    public class FiguraReadDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public double Area { get; set; }
        public double Perimetro { get; set; }
    }

}