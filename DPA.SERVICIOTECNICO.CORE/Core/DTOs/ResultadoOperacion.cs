namespace DPA.SERVICIOTECNICO.CORE.Core.DTOs
{
    // Resultado de una operación de escritura. Permite al controlador distinguir un dato inválido (400) de un
    // registro inexistente (404) sin recurrir a excepciones.
    public class ResultadoOperacion<T>
    {
        public bool Exito { get; private init; }
        public bool NoEncontrado { get; private init; }
        public string? Error { get; private init; }
        public T? Datos { get; private init; }

        public static ResultadoOperacion<T> Correcto(T datos) => new() { Exito = true, Datos = datos };
        public static ResultadoOperacion<T> Invalido(string error) => new() { Error = error };
        public static ResultadoOperacion<T> SinRegistro() => new() { NoEncontrado = true };
    }
}
