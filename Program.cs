using FigurasGeometricas.Models;

namespace FigurasGeometricas
{
    internal class Program
    {
        static void Main()
        {
            GestorFiguras gestor = new GestorFiguras();
            Menu.Mostrar(gestor);
        }
    }
}
