using System;
using System.Threading.Tasks;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
        {

            ParseHTML parse = new ParseHTML();
            Task.Run(async () => await parse.SaveData());

            Console.WriteLine("Hello World!");

            Console.ReadLine();
        }
    }
}
