using hkexnews.Model;
using System;
using System.Threading.Tasks;

namespace hkexnews
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ParseHTML parse = new ParseHTML();

                HKNewsContext db = new HKNewsContext();

                db.Records.Add(new Records
                {
                    Code = "11",
                    Date = "1111",
                    Stock_Name = "222",
                    Num = "11",
                    Rate = "2"
                });

                db.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            //parse.GetHtml();

            //Task.Run(async () => await parse.SaveData());

            Console.WriteLine("Hello World!");

            Console.ReadLine();
        }
    }
}
