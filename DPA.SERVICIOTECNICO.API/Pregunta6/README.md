# Pregunta 6: Pruebas con Postman

Esta carpeta contiene las pruebas de los dos controladores de la API:

- `PC01-ServicioTecnico.postman_collection.json`: colección exportada de Postman (formato v2.1).
- `resultado-newman.json` y `resultado-newman.txt`: resultado de la última ejecución con Newman, en detalle y como
  salida de consola.

## Requisitos previos

1. Crear la base de datos con `BaseDeDatos/ServicioTecnicoDB.sql`, que incluye los datos iniciales.
2. Iniciar la API con el perfil http (`dotnet run --launch-profile http` en `DPA.SERVICIOTECNICO.API`); queda
   disponible en http://localhost:5181.

## Ejecución en Postman

1. Seleccionar **Import** y elegir `PC01-ServicioTecnico.postman_collection.json`.
2. En el menú de la colección, elegir **Run collection** y ejecutarla sin cambiar el orden de las solicitudes.

La variable `baseUrl` apunta a http://localhost:5181. Las variables `tipoReparacionId` y `ordenReparacionId` se
completan con el Id que devuelve cada POST.

## Ejecución con Newman

Desde esta carpeta, con Node.js instalado:

```
npx --yes newman@6.2.3 run PC01-ServicioTecnico.postman_collection.json --reporters cli,json --reporter-json-export resultado-newman.json
```

## Cobertura

| Carpeta | Solicitudes | Pruebas |
|---|---|---|
| TipoReparacion (controlador con DbContext) | 7: listar, crear, consultar por Id, actualizar, verificar la actualización, eliminar y confirmar la eliminación (404) | 18 |
| OrdenReparacion (controlador con Repository) | 9: el mismo ciclo, más dos POST que deben rechazarse con 400 (equipo inexistente y costo negativo) | 30 |

Cada solicitud verifica el código de estado y el cuerpo de la respuesta: Id, nombres, precios y costos, estado de la
orden, cabecera Location en los POST, fecha de ingreso asignada por la API, y equipo y tipo de reparación en texto.
Cada carpeta crea su propio registro y lo elimina al final, por lo que la colección puede ejecutarse varias veces sin
dejar datos de prueba.

## Resultado

Ejecución del 11/10/2026 con Newman 6.2.3:

| Solicitudes | Pruebas | Aprobadas | Fallidas |
|---|---|---|---|
| 16 | 48 | 48 | 0 |

La colección se ejecutó dos veces consecutivas con el mismo resultado; al terminar, la base conservó únicamente sus
datos iniciales.
