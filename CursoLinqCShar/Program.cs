using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        var frutas = new string[] { "sandia", "fresas", "mango", "mango verde", "mango maduro" };

        //ForEach recorremos cada parte del array
        //Where filtra si true o false
        //StartsWith buscara mango
        //where creara una nueva coleccion
        var MangoList = frutas.Where(p => p.StartsWith("mango")).ToList();

        //ForEach recorremos cada parte del array e imprimimos
        MangoList.ForEach(p => Console.WriteLine(p));

    }
}