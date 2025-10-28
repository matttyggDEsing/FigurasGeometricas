// Models/FiguraCreateDto.cs
namespace FigurasGeometricas.Models
{
    public class FiguraCreateDto
    {
        public string Tipo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public double? Radio { get; set; }
        public double? Base { get; set; }
        public double? Altura { get; set; }
        public double? LadoA { get; set; }
        public double? LadoB { get; set; }
        public double? LadoC { get; set; }
    }

}