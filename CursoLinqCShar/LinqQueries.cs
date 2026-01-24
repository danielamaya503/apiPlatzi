using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace CursoLinqCShar
{
    public class LinqQueries
    {
        //Coleccion de libros
        private List<book> librosCollection = new List<book>();

        //Constructor de la clase
        public LinqQueries() 
        {
            //Obtener la ruta completa del archivo books.json
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "books.json");

            if (!File.Exists(jsonPath)) 
            {
                throw new FileNotFoundException("El archivo books.json no se encuentra en la ruta: " + jsonPath);
            }

            //StreamReader es para leer archivos de texto
            using (StreamReader reader = new(jsonPath)) {

                //ReadToEnd lee todo el contenido del archivo
                string json = reader.ReadToEnd();

                //Deserializar el JSON a una lista de objetos book
                //Colleccion de libros a tipo List<book>
                //JsonSerializerOptions es para configurar opciones de serializacion
                //PropertyNameCaseInsensitive = true para que no importe mayusculas o minusculas en los nombres de las propiedades
                librosCollection = JsonSerializer.Deserialize<List<book>>(json, 
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });


            }
        }

        //Metodo que devuelve toda la coleccion de libros
        public IEnumerable<book> TodaLaCollection() {
            return librosCollection;
        }
    }
}
