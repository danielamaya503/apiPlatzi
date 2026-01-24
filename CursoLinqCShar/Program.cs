using CursoLinqCShar;
using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        /*
        var frutas = new string[] { "sandia", "fresas", "mango", "mango verde", "mango maduro" };

        //ForEach recorremos cada parte del array
        //Where filtra si true o false
        //StartsWith buscara mango
        //where creara una nueva coleccion
        var MangoList = frutas.Where(p => p.StartsWith("mango")).ToList();

        //ForEach recorremos cada parte del array e imprimimos
        MangoList.ForEach(p => Console.WriteLine(p));

        */

        LinqQueries linqQueries = new LinqQueries();

        imprimir(linqQueries.TodaLaCollection());

    }

    public static void imprimir(IEnumerable<book> lista) {

        //Imprimimos encabezados
       
        Console.WriteLine("{0, -60} {1, 15} {2, 15}\n", "Titulo", "N. Paginas", "Fecha publicación");

        //Imprimimos cada libro reccorriendo la lista
        foreach (var item in lista) {
            //Imprimimos cada libro con formato
            //ToShortDateString convierte la fecha a un formato corto
            Console.WriteLine("{0, -60} {1, 15} {2, 15}", item.Title, item.Pagecount, item.PublishedDate.ToShortDateString());
        }
    }


}