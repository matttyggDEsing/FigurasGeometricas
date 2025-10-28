using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FigurasGeometricas.Models;
using FigurasGeometricas.Services;
using Color = Spectre.Console.Color;

namespace FigurasGeometricas
{
    public static class Menu
    {
        private static readonly string[] opciones =
        {
            " Agregar Figura",
            " Eliminar Figura",
            " Mostrar Figuras",
            " Calcular Área y Perímetro Total",
            " Salir"
        };

        public static async Task MostrarAsync(GestorFigurasRemoto gestor)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();

                AnsiConsole.Write(
                    new FigletText("Figuras")
                        .LeftJustified()
                        .Color(Color.Aqua));

                AnsiConsole.MarkupLine("[bold yellow]Cliente de Figuras Geométricas (API)[/]\n");

                string opcion = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Seleccione una opción:[/]")
                        .HighlightStyle(new Style(Color.Black, Color.Yellow))
                        .AddChoices(opciones)
                );

                Console.Clear();
                salir = await EjecutarOpcionAsync(Array.IndexOf(opciones, opcion), gestor, salir);
            }
        }

        // 🔹 Crear figura (POST)
        private static async Task AgregarFiguraAsync(GestorFigurasRemoto gestor)
        {
            var tipo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Seleccione el tipo de figura a agregar:[/]")
                    .AddChoices("Círculo", "Rectángulo", "Triángulo")
            );

            string nombre = AnsiConsole.Ask<string>("Ingrese [green]el nombre[/] de la figura:");

            var dto = new FiguraCreateDto
            {
                Tipo = tipo.ToLower()
                           .Replace("í", "i")
                           .Replace("é", "e")
                           .Replace("á", "a")
                           .Replace("ó", "o")
                           .Replace("ú", "u"),
                Nombre = nombre
            };

            switch (tipo)
            {
                case "Círculo":
                    dto.Radio = AnsiConsole.Ask<double>("Ingrese el [yellow]radio[/]:");
                    if (dto.Radio <= 0)
                    {
                        AnsiConsole.MarkupLine("[red]El radio debe ser mayor a 0.[/]");
                        return;
                    }
                    break;

                case "Rectángulo":
                    dto.Base = AnsiConsole.Ask<double>("Ingrese la [yellow]base[/]:");
                    dto.Altura = AnsiConsole.Ask<double>("Ingrese la [yellow]altura[/]:");
                    if (dto.Base <= 0 || dto.Altura <= 0)
                    {
                        AnsiConsole.MarkupLine("[red]Base y altura deben ser mayores a 0.[/]");
                        return;
                    }
                    break;

                case "Triángulo":
                    dto.LadoA = AnsiConsole.Ask<double>("Ingrese el [yellow]lado A[/]:");
                    dto.LadoB = AnsiConsole.Ask<double>("Ingrese el [yellow]lado B[/]:");
                    dto.LadoC = AnsiConsole.Ask<double>("Ingrese el [yellow]lado C[/]:");

                    if (dto.LadoA <= 0 || dto.LadoB <= 0 || dto.LadoC <= 0)
                    {
                        AnsiConsole.MarkupLine("[red]Todos los lados deben ser mayores a 0.[/]");
                        return;
                    }

                    // Validar desigualdad triangular
                    if (dto.LadoA + dto.LadoB <= dto.LadoC ||
                        dto.LadoA + dto.LadoC <= dto.LadoB ||
                        dto.LadoB + dto.LadoC <= dto.LadoA)
                    {
                        AnsiConsole.MarkupLine("[red]Los lados ingresados no forman un triángulo válido.[/]");
                        return;
                    }
                    break;
            }

            var creada = await gestor.CrearFiguraAsync(dto);

            if (creada != null)
            {
                AnsiConsole.MarkupLine($"[green]Figura creada correctamente:[/] [bold]{creada.Nombre}[/] (ID {creada.Id})");
                AnsiConsole.MarkupLine($"Área: {creada.Area:F2}, Perímetro: {creada.Perimetro:F2}");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Error al crear la figura (ver log del servidor).[/]");
            }
        }

        // 🔹 Listar figuras (GET)
        private static async Task MostrarFigurasAsync(GestorFigurasRemoto gestor)
        {
            var figuras = await gestor.ObtenerFigurasAsync();

            if (figuras.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No hay figuras registradas.[/]");
                return;
            }

            var tabla = new Table().AddColumns("ID", "Nombre", "Tipo", "Área", "Perímetro");

            foreach (var f in figuras)
                tabla.AddRow(f.Id.ToString(), f.Nombre, f.Tipo, f.Area.ToString("F2"), f.Perimetro.ToString("F2"));

            AnsiConsole.Write(tabla);
        }

        // 🔹 Eliminar figura (DELETE)
        private static async Task EliminarFiguraAsync(GestorFigurasRemoto gestor)
        {
            var figuras = await gestor.ObtenerFigurasAsync();

            if (figuras.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No hay figuras para eliminar.[/]");
                return;
            }

            var opciones = new List<string>();
            foreach (var f in figuras)
                opciones.Add($"{f.Id} - {f.Nombre} ({f.Tipo})");

            string seleccion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Seleccione la figura a eliminar:[/]")
                    .AddChoices(opciones)
            );

            int id = int.Parse(seleccion.Split(" - ")[0]);

            if (await gestor.EliminarFiguraAsync(id))
                AnsiConsole.MarkupLine($"[green]Figura eliminada correctamente.[/]");
            else
                AnsiConsole.MarkupLine("[red]Error: no se pudo eliminar la figura.[/]");
        }

        // 🔹 Totales (GET /figuras/totales)
        private static async Task MostrarTotalesAsync(GestorFigurasRemoto gestor)
        {
            var totales = await gestor.TotalesAsync();
            if (totales == null)
            {
                AnsiConsole.MarkupLine("[red]Error al obtener los totales.[/]");
                return;
            }

            AnsiConsole.MarkupLine($"[bold blue]Área total:[/] {totales["area"]:F2}");
            AnsiConsole.MarkupLine($"[bold blue]Perímetro total:[/] {totales["perimetro"]:F2}");
        }

        // 🔹 Controlador de opciones
        private static async Task<bool> EjecutarOpcionAsync(int opcion, GestorFigurasRemoto gestor, bool salir)
        {
            switch (opcion)
            {
                case 0:
                    await AgregarFiguraAsync(gestor);
                    break;
                case 1:
                    await EliminarFiguraAsync(gestor);
                    break;
                case 2:
                    await MostrarFigurasAsync(gestor);
                    break;
                case 3:
                    await MostrarTotalesAsync(gestor);
                    break;
                case 4:
                    return true;
            }

            AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
            Console.ReadKey(true);
            return salir;
        }
    }
}
