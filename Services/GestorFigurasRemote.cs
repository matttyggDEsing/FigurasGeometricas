// Services/GestorFigurasRemoto.cs
using FigurasGeometricas.Models;
using System.Net.Http.Json;

namespace FigurasGeometricas.Services
{
    public class GestorFigurasRemoto
    {
        private readonly HttpClient client;

        public GestorFigurasRemoto(string baseUrl = "http://localhost:5038/api/")
        {
            client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public async Task<List<FiguraReadDto>> ObtenerFigurasAsync()
            => await client.GetFromJsonAsync<List<FiguraReadDto>>("figuras") ?? new();

        public async Task<FiguraReadDto?> CrearFiguraAsync(FiguraCreateDto dto)
        {
            var response = await client.PostAsJsonAsync("figuras", dto);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<FiguraReadDto>();
            return null;
        }

        public async Task<bool> EliminarFiguraAsync(int id)
        {
            var response = await client.DeleteAsync($"figuras/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<Dictionary<string, double>?> TotalesAsync()
            => await client.GetFromJsonAsync<Dictionary<string, double>>("figuras/totales");
    }
}
