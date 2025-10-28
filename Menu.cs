using Spectre.Console;
using System;
using System.Collections.Generic;
using Color = Spectre.Console.Color;
using FigurasGeometricas.Models; // 👈 importante para acceder a las clases Figura, Circulo, etc.

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

        public static void Mostrar(GestorFiguras gestor)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();

                AnsiConsole.Write(
                    new FigletText("Figuras")
                        .LeftJustified()
                        .Color(Color.Aqua));

                AnsiConsole.MarkupLine("[bold yellow]Sistema de Gestión de Figuras Geométricas[/]\n");

                string opcion = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Seleccione una opción:[/]")
                        .HighlightStyle(new Style(Color.Black, Color.Yellow))
                        .AddChoices(opciones)
                );

                Console.Clear();
                EjecutarOpcion(Array.IndexOf(opciones, opcion), gestor, ref salir);
            }
        }

        // 🔹 Este método usa los de dibujo más abajo
        private static void AgregarFigura(GestorFiguras gestor)
        {
            var tipo = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Seleccione el tipo de figura a agregar:[/]")
                    .AddChoices("Círculo", "Rectángulo", "Triángulo")
            );

            string nombre = AnsiConsole.Ask<string>("Ingrese [green]el nombre[/] de la figura:");

            switch (tipo)
            {
                case "Círculo":
                    double radio = AnsiConsole.Ask<double>("Ingrese el [yellow]radio[/]:");
                    gestor.AgregarFigura(new Circulo(nombre, radio));
                    AnsiConsole.Write(new Rule("[aqua]Vista previa del Círculo[/]").RuleStyle("aqua"));
                    DibujarCirculo(radio);
                    break;

                case "Rectángulo":
                    double b = AnsiConsole.Ask<double>("Ingrese la [yellow]base[/]:");
                    double h = AnsiConsole.Ask<double>("Ingrese la [yellow]altura[/]:");
                    gestor.AgregarFigura(new Rectangulo(nombre, b, h));
                    AnsiConsole.Write(new Rule("[green]Vista previa del Rectángulo[/]").RuleStyle("green"));
                    DibujarRectangulo(b, h);
                    break;

                case "Triángulo":
                    double ladoA = AnsiConsole.Ask<double>("Ingrese el [yellow]lado A[/]:");
                    double ladoB = AnsiConsole.Ask<double>("Ingrese el [yellow]lado B[/]:");
                    double ladoC = AnsiConsole.Ask<double>("Ingrese el [yellow]lado C[/]:");

                    // Validación de triángulo antes de crear el objeto
                    if (ladoA + ladoB <= ladoC || ladoA + ladoC <= ladoB || ladoB + ladoC <= ladoA)
                    {
                        AnsiConsole.MarkupLine("[red] Los lados ingresados no forman un triángulo válido.[/]");
                        AnsiConsole.MarkupLine("[yellow]Presione cualquier tecla para volver al menú...[/]");
                        Console.ReadKey(true);
                        return; // 🔹 Salimos del 'case', sin agregar la figura
                    }

                    var triangulo = new Triangulo(nombre, ladoA, ladoB, ladoC);
                    gestor.AgregarFigura(triangulo);

                    AnsiConsole.Write(new Rule("[yellow]Vista previa del Triángulo[/]").RuleStyle("yellow"));

                    string tipoTriangulo = triangulo.TipoTriangulo();
                    int altura = (int)Math.Round(Math.Min(ladoA, Math.Min(ladoB, ladoC)));
                    DibujarTriangulo(altura, tipoTriangulo);

                    double area = triangulo.CalcularArea();
                    AnsiConsole.MarkupLine($"[green]Área:[/] {area:F2}, [green]Tipo:[/] {tipoTriangulo}");
                    break;

            }
        }


        // 🌀 --- FUNCIONES DE DIBUJO ---
        private static void DibujarCirculo(double radio)
        {
            int r = (int)Math.Round(radio);
            for (int y = -r; y <= r; y++)
            {
                for (int x = -r; x <= r; x++)
                {
                    double d = Math.Sqrt(x * x + y * y);
                    if (d < r)
                        AnsiConsole.Markup("[aqua]██[/]");
                    else
                        AnsiConsole.Markup("  ");
                }
                Console.WriteLine();
            }
        }

        private static void DibujarRectangulo(double b, double h)
        {
            int width = (int)Math.Round(b);
            int height = (int)Math.Round(h);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        AnsiConsole.Markup("[green]██[/]");
                    else
                        AnsiConsole.Markup("[black]  [/]");
                }
                Console.WriteLine();
            }
        }

        private static void DibujarTriangulo(int altura, string tipo)
        {
            switch (tipo)
            {
                case "Equilátero":
                    for (int i = 1; i <= altura; i++)
                    {
                        Console.Write(new string(' ', altura - i));
                        for (int j = 0; j < (2 * i - 1); j++)
                            AnsiConsole.Markup("[yellow]^[/]");
                        Console.WriteLine();
                    }
                    break;

                case "Isósceles":
                    for (int i = 1; i <= altura; i++)
                    {
                        Console.Write(new string(' ', altura - i));
                        for (int j = 0; j < i; j++)
                            AnsiConsole.Markup("[yellow]^[/]");
                        Console.WriteLine();
                    }
                    break;

                case "Escaleno":
                    for (int i = 1; i <= altura; i++)
                    {
                        for (int j = 0; j < i; j++)
                            AnsiConsole.Markup("[yellow]^[/]");
                        Console.WriteLine();
                    }
                    break;
            }
        }



        private static void EjecutarOpcion(int opcion, GestorFiguras gestor, ref bool salir)
        {
            switch (opcion)
            {
                case 0:
                    AgregarFigura(gestor);
                    AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
                case 1:
                    EliminarFigura(gestor);
                    AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
                case 2:
                    MostrarFiguras(gestor);
                    AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
                case 3:
                    MostrarTotales(gestor);
                    AnsiConsole.MarkupLine("\n[grey]Presione una tecla para continuar...[/]");
                    Console.ReadKey();
                    break;
                case 4:
                    salir = true;
                    break;
            }
        }

        // Métodos auxiliares para las opciones del menú (puedes ajustar según tu implementación)
        private static void EliminarFigura(GestorFiguras gestor)
        {
            if (gestor.ListaFiguras.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No hay figuras para eliminar.[/]");
                return;
            }

            var nombres = gestor.ListaFiguras.ConvertAll(f => f.Nombre);
            string nombre = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Seleccione la figura a eliminar:[/]")
                    .AddChoices(nombres)
            );
            gestor.EliminarFigura(nombre);
            AnsiConsole.MarkupLine($"[bold red] Figura '{nombre}' eliminada.[/]");
        }

        private static void MostrarFiguras(GestorFiguras gestor)
        {
            if (gestor.ListaFiguras.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No hay figuras para mostrar.[/]");
                return;
            }
            gestor.MostrarTodasLasFiguras();
        }

        private static void MostrarTotales(GestorFiguras gestor)
        {
            double area = gestor.CalcularAreaTotal();
            double perimetro = gestor.CalcularPerimetroTotal();
            AnsiConsole.MarkupLine($"[bold blue]Área total:[/] {area:N2}");
            AnsiConsole.MarkupLine($"[bold blue]Perímetro total:[/] {perimetro:N2}");
        }
    }
}
