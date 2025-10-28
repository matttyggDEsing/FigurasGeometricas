using FigurasGeometricas.Services;

namespace FigurasGeometricas
{
    internal class Program
    {
        static async Task Main()
        {
            var gestor = new GestorFigurasRemoto();
            await Menu.MostrarAsync(gestor);
        }
    }
}
