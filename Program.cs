using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace AnalisisF1
{
    class ResultadoCarrera
    {
        public int Temporada { get; set; }
        public string Equipo { get; set; }
        public string Piloto { get; set; }
        public string Carrera { get; set; }
        public int? PosicionClasificacion { get; set; }
        public int? PosicionFinal { get; set; }
        public double Puntos { get; set; }

        public static ResultadoCarrera DesdeLineaCsv(string linea)
        {
            var partes = linea.Split(',');
            if (partes.Length < 7) return null;

            var resultado = new ResultadoCarrera
            {
                Temporada = int.TryParse(partes[0].Trim(), out int t) ? t : 0,
                Equipo = partes[1].Trim(),
                Piloto = partes[2].Trim(),
                Carrera = partes[3].Trim(),
                PosicionClasificacion = int.TryParse(partes[4].Trim(), out int pc) ? pc : (int?)null,
                PosicionFinal = int.TryParse(partes[5].Trim(), out int pf) ? pf : (int?)null,
                Puntos = double.TryParse(partes[6].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double pts) ? pts : 0.0
            };

            return resultado;
        }

        public override string ToString()
        {
            return $"{Temporada}, {Equipo}, {Piloto}, {Carrera}, Clasificación: {(PosicionClasificacion?.ToString() ?? "-")}, Final: {(PosicionFinal?.ToString() ?? "-")}, Puntos: {Puntos}";
        }
    }

    class Programa
    {
        static List<ResultadoCarrera> datos = new List<ResultadoCarrera>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Programa de Análisis de F1 (2016-2024) ===\n");

            string rutaArchivo = @"C:\Users\alano\OneDrive\Documentos\Programacion-Quality\G2-TP3-Gomez-Ordoñez-Scarponi-Carbajal\f1_last5years.csv";
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine($"No se encontró el archivo: {rutaArchivo}");
                Console.WriteLine("Asegúrate de colocar el archivo CSV en la ruta especificada.");
                return;
            }

            CargarDatosCsv(rutaArchivo);
            Console.WriteLine($"Se cargaron {datos.Count} registros.\n");

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                Console.Write("Elige una opción: ");
                var opcion = Console.ReadLine();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        BuscarPodiosPiloto();
                        break;
                    case "2":
                        DatosEquipoPorTemporada();
                        break;
                    case "3":
                        MostrarMayorRemontada();
                        break;
                    case "4":
                        ListarEquiposOrdenados();
                        break;
                    case "5":
                        MostrarTodosLosDatos();
                        break;
                    case "6":
                        PromedioPosicionPiloto();
                        break;
                    case "7":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intenta nuevamente.\n");
                        break;
                }
            }

            Console.WriteLine("Saliendo... ¡Hasta luego!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("Menú:");
            Console.WriteLine("1) Buscar podios de un piloto");
            Console.WriteLine("2) Datos de un equipo en una temporada específica");
            Console.WriteLine("3) Mostrar la mayor remontada");
            Console.WriteLine("4) Listar equipos ordenados alfabéticamente");
            Console.WriteLine("5) Mostrar todos los datos cargados");
            Console.WriteLine("6) Mostrar el promedio");
            Console.WriteLine("7) Salir\n");
        }

        static void CargarDatosCsv(string ruta)
        {
            datos.Clear();
            var lineas = File.ReadAllLines(ruta);

            int inicio = 0;
            if (lineas.Length > 0 && !int.TryParse(lineas[0].Split(',')[0].Trim(), out _))
            {
                inicio = 1;
            }

            for (int i = inicio; i < lineas.Length; i++)
            {
                var resultado = ResultadoCarrera.DesdeLineaCsv(lineas[i]);
                if (resultado != null) datos.Add(resultado);
            }
        }

        static void BuscarPodiosPiloto()
        {
            Console.Write("Ingresa el nombre del piloto: ");
            string nombre = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(nombre))
            {
                Console.WriteLine("Nombre vacío. Volviendo al menú.\n");
                return;
            }

            int podios = 0;
            foreach (var r in datos)
            {
                if (r.Piloto.IndexOf(nombre, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    r.PosicionFinal.HasValue &&
                    r.PosicionFinal.Value >= 1 &&
                    r.PosicionFinal.Value <= 3)
                {
                    podios++;
                }
            }

            Console.WriteLine($"Piloto: '{nombre}'. Podios totales: {podios}\n");
        }

        static void DatosEquipoPorTemporada()
        {
            Console.Write("Ingresa el nombre del equipo: ");
            string equipo = Console.ReadLine().Trim();
            Console.Write("Ingresa el año: ");
            if (!int.TryParse(Console.ReadLine(), out int anio))
            {
                Console.WriteLine("Año inválido.\n");
                return;
            }

            var resultados = new List<ResultadoCarrera>();
            foreach (var r in datos)
            {
                if (r.Temporada == anio && r.Equipo.IndexOf(equipo, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    resultados.Add(r);
                }
            }

            if (resultados.Count == 0)
            {
                Console.WriteLine($"No se encontraron resultados para '{equipo}' en {anio}.\n");
                return;
            }

            Console.WriteLine($"Resultados de '{equipo}' en {anio}:\n");
            double puntosTotales = 0;
            resultados.Sort((a, b) => string.Compare(a.Carrera, b.Carrera, StringComparison.OrdinalIgnoreCase));
            foreach (var r in resultados)
            {
                Console.WriteLine($"Carrera: {r.Carrera} | Piloto: {r.Piloto} | Puntos: {r.Puntos}");
                puntosTotales += r.Puntos;
            }

            Console.WriteLine($"\nPuntos totales: {puntosTotales}\n");
        }

        static void MostrarMayorRemontada()
        {
            int mejorDelta = int.MinValue;
            ResultadoCarrera mejorResultado = null;

            foreach (var r in datos)
            {
                if (r.PosicionClasificacion.HasValue && r.PosicionFinal.HasValue)
                {
                    int delta = r.PosicionClasificacion.Value - r.PosicionFinal.Value;
                    if (delta > mejorDelta)
                    {
                        mejorDelta = delta;
                        mejorResultado = r;
                    }
                }
            }

            if (mejorResultado == null)
            {
                Console.WriteLine("No se encontró una remontada válida.\n");
                return;
            }

            Console.WriteLine("Mayor remontada:");
            Console.WriteLine($"Piloto: {mejorResultado.Piloto}");
            Console.WriteLine($"Carrera: {mejorResultado.Carrera}");
            Console.WriteLine($"Posiciones ganadas: {mejorDelta}\n");
        }

        static void ListarEquiposOrdenados()
        {
            var equipos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var r in datos)
            {
                equipos.Add(r.Equipo);
            }

            var listaEquipos = new List<string>(equipos);
            listaEquipos.Sort(StringComparer.OrdinalIgnoreCase);

            Console.WriteLine("Equipos ordenados:");
            foreach (var equipo in listaEquipos)
            {
                Console.WriteLine(equipo);
            }
            Console.WriteLine();
        }

        static void MostrarTodosLosDatos()
        {
            Console.WriteLine("Datos cargados:\n");
            foreach (var r in datos)
            {
                Console.WriteLine(r.ToString());
            }
            Console.WriteLine($"\nTotal de registros: {datos.Count}\n");
        }
        static void PromedioPosicionPiloto()
        {
            Console.Write("Ingresa el nombre del piloto: ");
            string piloto = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(piloto))
            {
                Console.WriteLine("Nombre vacío. Volviendo al menú.\n");
                return;
            }

            int totalPosiciones = 0;
            int cantidadCarreras = 0;

            foreach (var r in datos)
            {
                if (r.Piloto.IndexOf(piloto, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    r.PosicionFinal.HasValue)
                {
                    totalPosiciones += r.PosicionFinal.Value;
                    cantidadCarreras++;
                }
            }

            if (cantidadCarreras == 0)
            {
                Console.WriteLine($"No se encontraron carreras para el piloto '{piloto}'.\n");
                return;
            }

            double promedio = (double)totalPosiciones / cantidadCarreras;
            Console.WriteLine($"Promedio de posición final de '{piloto}': {promedio:F2}\n");
        }
    }
}