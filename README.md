# 📐 Figuras Geométricas — Cliente en C# (.NET 8)

¡Hola! 👋 Este proyecto es una **aplicación de consola interactiva** hecha en **C#** que se conecta a una **API REST** para crear, mostrar y eliminar figuras geométricas (como círculos, rectángulos y triángulos).  
Está pensada para **aprender cómo se comunica un programa con una API**, usando colores, menús y animaciones en la terminal. 🎨💻

---

## 🚀 Objetivo del proyecto
Enseñar a programar paso a paso usando un caso divertido: **manejar figuras geométricas desde consola**.  
Cada parte del código tiene una función clara, y este README te va a guiar **línea por línea** 🧠✨.

---

## 🧩 Estructura del proyecto

```
/FigurasGeometricas
├─ FigurasGeometricas.csproj        → Archivo que define el proyecto de C#
├─ FigurasGeometricas.sln           → Solución de Visual Studio
├─ Program.cs                       → Punto de entrada principal del programa
├─ Menu.cs                          → Lógica del menú principal (interfaz del usuario)
├─ Models/                          → Carpeta con clases que representan datos
│   ├─ FiguraCreateDto.cs           → Modelo para crear figuras
│   └─ FiguraReadDto.cs             → Modelo para leer figuras desde la API
└─ Services/
    └─ GestorFigurasRemoto.cs       → Clase que se conecta con la API remota
```

---

## 🛠️ Cómo ejecutar el programa

1. Asegurate de tener **.NET 8 instalado**.
2. Abrí una consola dentro de la carpeta del proyecto.
3. Escribí:

```bash
dotnet run
```

4. ¡Y listo! 🎉 Vas a ver el menú en la terminal.

---

## 🖥️ Archivo: `Program.cs`

Este archivo es el **inicio del programa**.  
Cuando lo ejecutás, crea un “gestor” que se encarga de hablar con la API y muestra el menú principal.

```csharp
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
```

### 🧠 Explicación paso a paso:
- `var gestor = new GestorFigurasRemoto();` → crea un objeto que sabe comunicarse con la API.
- `await Menu.MostrarAsync(gestor);` → muestra el menú con las opciones para el usuario.

👉 **Ejemplo:**
Cuando corrés el programa, vas a ver algo así:

```
FIGURAS
Cliente de Figuras Geométricas (API)

Seleccione una opción:
> Agregar Figura
  Eliminar Figura
  Mostrar Figuras
  Calcular Área y Perímetro Total
  Salir
```

---

## 🎨 Archivo: `Menu.cs`

Este archivo es el **corazón del programa**.  
Aquí está el **menú interactivo** que te permite agregar, eliminar o ver figuras.

Usa una librería llamada `Spectre.Console` 🧡 que permite mostrar texto con colores, tablas y menús bonitos.

### ✳️ Código principal:

```csharp
public static async Task MostrarAsync(GestorFigurasRemoto gestor)
{
    bool salir = false;
    while (!salir)
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("Figuras").LeftJustified().Color(Color.Aqua));
        AnsiConsole.MarkupLine("[bold yellow]Cliente de Figuras Geométricas (API)[/]\n");

        string opcion = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Seleccione una opción:[/]")
                .AddChoices(opciones)
        );

        Console.Clear();
        salir = await EjecutarOpcionAsync(Array.IndexOf(opciones, opcion), gestor, salir);
    }
}
```

### 🧠 Explicación:
1. Limpia la consola.
2. Muestra un título colorido (usando `FigletText`).
3. Pregunta al usuario qué quiere hacer (con un menú seleccionable).
4. Ejecuta la opción elegida (crear, borrar, ver, etc).

---

## ➕ Agregar una figura

Cuando elegís “Agregar Figura”, el programa te guía paso a paso:

```csharp
dto.Radio = AnsiConsole.Ask<double>("Ingrese el [yellow]radio[/]:");
```

👆 Esa línea **le pregunta al usuario el radio** del círculo.  
Después el código **verifica que el valor sea válido** (mayor que 0).

Si está todo bien, el programa llama a la API:

```csharp
var creada = await gestor.CrearFiguraAsync(dto);
```

Y muestra el resultado:

```
Figura creada correctamente: Circulo1 (ID 5)
Área: 28.27, Perímetro: 18.84
```

---

## 📋 Ver todas las figuras

```csharp
var figuras = await gestor.ObtenerFigurasAsync();
var tabla = new Table().AddColumns("ID", "Nombre", "Tipo", "Área", "Perímetro");
```

El programa **pide las figuras a la API** y las muestra en una **tabla colorida**:

| ID | Nombre | Tipo | Área | Perímetro |
|----|---------|------|------|-----------|
| 1  | Circulo1 | círculo | 28.27 | 18.84 |
| 2  | TrianguloA | triángulo | 12.5 | 18.0 |

---

## ❌ Eliminar una figura

Si querés borrar una figura:

```csharp
int id = int.Parse(seleccion.Split(" - ")[0]);
await gestor.EliminarFiguraAsync(id);
```

El programa **te muestra una lista de figuras**, elegís una, y se borra de la base de datos.  
Después muestra:

```
✅ Figura eliminada correctamente.
```

---

## 🔢 Calcular totales

También podés ver **la suma de todas las áreas y perímetros** de las figuras guardadas:

```csharp
var totales = await gestor.TotalesAsync();
AnsiConsole.MarkupLine($"[bold blue]Área total:[/] {totales["area"]:F2}");
AnsiConsole.MarkupLine($"[bold blue]Perímetro total:[/] {totales["perimetro"]:F2}");
```

Ejemplo:

```
Área total: 85.72
Perímetro total: 73.44
```

---

## 🧰 Archivo: `GestorFigurasRemoto.cs`

Este archivo se encarga de **comunicarse con la API**.  
Usa `HttpClient` para enviar y recibir datos desde internet.

```csharp
public async Task<List<FiguraReadDto>> ObtenerFigurasAsync()
    => await client.GetFromJsonAsync<List<FiguraReadDto>>("figuras") ?? new();
```

🧠 Eso significa:
- Va a la dirección `/api/figuras`.
- Pide las figuras.
- Las convierte a una lista de objetos `FiguraReadDto`.

👉 Ejemplo visual:
```
Petición: GET http://localhost:5038/api/figuras
Respuesta: [
  { "id": 1, "nombre": "Circulo1", "tipo": "circulo", "area": 28.27, "perimetro": 18.84 }
]
```

---

## 🧱 Modelos (Models)

Son las **estructuras de datos** que el programa usa para enviar y recibir información de la API.

### 🟢 `FiguraCreateDto.cs`

```csharp
public class FiguraCreateDto
{
    public string Tipo { get; set; }
    public string Nombre { get; set; }
    public double? Radio { get; set; }
    public double? Base { get; set; }
    public double? Altura { get; set; }
    public double? LadoA { get; set; }
    public double? LadoB { get; set; }
    public double? LadoC { get; set; }
}
```

🧠 Sirve para **enviar datos** cuando creás una figura.

Ejemplo de envío a la API:
```json
{
  "tipo": "circulo",
  "nombre": "MiCirculo",
  "radio": 3.0
}
```

---

### 🔵 `FiguraReadDto.cs`

```csharp
public class FiguraReadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public double Area { get; set; }
    public double Perimetro { get; set; }
}
```

🧠 Sirve para **recibir datos** desde la API.  
Cada figura creada vuelve con estos valores.

Ejemplo de respuesta:
```json
{
  "id": 1,
  "nombre": "MiCirculo",
  "tipo": "circulo",
  "area": 28.27,
  "perimetro": 18.84
}
```

---

## 🧮 Cómo funciona todo junto

1. El usuario elige una opción en el menú.
2. El programa le pregunta los datos (por ejemplo, el radio).
3. Crea un `FiguraCreateDto` con esa información.
4. Llama a la API para guardar la figura.
5. Muestra el resultado en pantalla.
6. ¡Y todo se ve colorido gracias a `Spectre.Console`! 🌈

---


## 📦 Dependencias usadas

- **Spectre.Console** → para colores, tablas y menús.
- **Microsoft.EntityFrameworkCore.Sqlite** → conexión con base de datos (si se usa localmente).
- **HttpClient / System.Net.Http.Json** → para conectarse a la API.

---

## ❤️ Créditos

Creado por **Matttygg Desing**  
Un proyecto educativo para aprender **C#, APIs y programación divertida** 🎓💻

---

> “El mejor código es el que cualquiera puede entender, incluso un niño curioso.” 👦✨
