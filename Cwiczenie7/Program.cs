using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cwiczenie7.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cwiczenie7
{
    public class Program
    {
        public static void Main(string[] args)
        {

            CreateHostBuilder(args).Build().Run();
        }

        public static void QueryExamples()
        {
            var db = new s5554Context();

            // SELECT * FROM Student
            //var students = db.Student.ToList();

            // wszyscy studenci ktorych imie zaczyna sie na
            /* var student = db.Student
                 .Where(s => s.FirstName.StartsWith("K"))
                 .OrderBy(s => s.LastName)
                 .ThenBy(s => s.FirstName)
                 .ToList();*/

            // Lazy Loading
            /* var student = db.Student
                 .Where(s => s.FirstName.StartsWith("K"))
                 .OrderBy(s => s.LastName)
                 .ThenBy(s => s.FirstName);*/

            /* var students = db.Student.ToList();

             int g = 0;
             // problem N+1
             foreach(var s in students)  // tu jest SELECT * FROM Student
             {

             }*/

            //var student = db.Student.OrderBy(s => s.LastName);

            //var student2 = student.Where(s => s.LastName == "Potocki");

            /*var students = db.Student
                .GroupBy(s => s.FirstName)
                .Select(s => new
                {
                    Imie = s.Key,
                    LiczbaStudentow = s.Count()
                }).ToList();*/
        }

        public static void UpdateExamples()
        {
            var db = new s5554Context();

            // update

            /*var st1 = db.Student.First(); // pierwszy rekord z bazy
            st1.LastName = "Zmiana";*/

            //db.SaveChanges();  // wysylanie zmian z kolekcji sa wysylane w jednej transakcji do bazy
            //db.SaveChangesAsync();

            // update studenta ktorego jeszcze nie ma, zostanie on stworzony
            var st1 = new Student
            {
                IndexNumber = "8",
                LastName = "Drewniany"
            };
            db.Attach(st1);

            //db.Entry(st1).Property("LastName").IsModified = true;
            db.SaveChanges();
        }

        public static void RemoveExamples()
        {
            var db = new s5554Context();
            // usuwanie studenta pobierajac go najpierw
            /* var st = db.Student.OrderByDescending(s => s.IndexNumber).First();
             db.Student.Remove(st);*/

            var s = new Student
            {
                IndexNumber = "111"
            };
            db.Attach(s); // dolaczanie obiektu
            db.Remove(s); // ustawienie flagi na Deleted i usiniecie
            //db.Entry(s).State = EntityState.Deleted; to samo co dwie komendy powyzej

            db.SaveChanges();
        }

        public static void InsertExamples()
        {
            var db = new s5554Context();

            var s = new Student
            {
                FirstName = "Kamil",
                LastName = "Potocki"
            };

            db.Student.Add(s);
            db.SaveChanges();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
