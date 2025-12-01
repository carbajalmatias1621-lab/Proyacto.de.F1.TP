## 📌 **Descripción del Proyecto**

Este proyecto consiste en el desarrollo de una **aplicación de consola en C#** orientada al análisis de datos reales de Fórmula 1.
El sistema carga información desde un archivo **CSV** con resultados de carreras entre los años **2016 y 2024**, y permite obtener distintos indicadores estadísticos mediante consultas realizadas por el usuario.

El trabajo se desarrolló para la materia **Programación**, aplicando lectura de archivos, manipulación de colecciones, validación de datos, ordenamientos, filtrados y creación de nuevas funcionalidades.

---

## 🎯 **Objetivos del Trabajo Práctico**

* Practicar el manejo de archivos externos (.csv).
* Integrar estructuras de datos y clases personalizadas.
* Implementar consultas dinámicas sobre listas.
* Diseñar un menú interactivo.
* Añadir un **nuevo indicador propio** como analistas de datos.
* Documentar correctamente el proyecto.

---

## 🧩 **Contenido del CSV**

Cada línea del archivo contiene:

* Temporada
* Equipo
* Piloto
* Carrera
* Posición de clasificación
* Posición final
* Puntos obtenidos

La clase **ResultadoCarrera** encapsula esta información y provee el método:

```csharp
public static ResultadoCarrera DesdeLineaCsv(string linea)
```

que transforma una línea del CSV en un objeto válido.

---

## 🖥️ **Funciones Principales del Programa**

El menú permite:

### **1) Buscar podios de un piloto**

Cuenta cuántas veces un piloto terminó entre los 3 primeros.

---

### **2) Datos de un equipo por temporada**

Permite ingresar un equipo y un año para listar:

* carreras disputadas
* pilotos
* puntos obtenidos
* acumulado total

---

### **3) Mostrar la mayor remontada**

Busca la carrera donde un piloto ganó más posiciones respecto a su clasificación.

---

### **4) Listar equipos ordenados alfabéticamente**

Genera un listado único de equipos sin repetir y ordenado.

---

### **5) Mostrar todos los datos cargados**

Imprime cada registro del CSV formateado como texto.

---

### **6) Salir del programa**

---

## ⭐ **Nueva Funcionalidad Agregada**

### ✔️ *Indicador de Eficiencia del Piloto* *(creado por el equipo)*

Como analistas de datos, se agregó un nuevo cálculo para evaluar el rendimiento completo de un piloto.

El nuevo método implementado fue:

```csharp
// --- INDICADOR NUEVO ---
static void IndicadorEficienciaPiloto()
```

### 📌 ¿Qué mide este indicador?

Combina:

* Podios obtenidos
* Puntos totales
* Cantidad de carreras disputadas

La fórmula diseñada es:

```
Indicador = Podios + (PuntosTotales / Carreras)
```

De esta forma:

* Premia la regularidad (puntos por carrera)
* Premia los resultados destacados (podios)
* Penaliza poca participación (más carreras mejoran la precisión)

### 📤 Ejemplo de salida:

```
Piloto: Max Verstappen
Carreras: 87
Podios: 53
Puntos totales: 1550
INDICADOR DE EFICIENCIA: 71.82
```

---

## 🛠️ **Tecnologías Utilizadas**

* Lenguaje: **C# (.NET)**
* Paradigma: Programación estructurada + clases modelo
* Lectura de archivos: **System.IO**
* Colecciones: List<T>, HashSet<T>



## 👥 **Integrantes**

* Gómez
* Ordoñez
* Scarponi
* Carbajal

---

Si querés también te lo paso:
📄 en PDF
📌 o lo adapto al formato de tu carpeta real del proyecto

Decime y te lo genero.
