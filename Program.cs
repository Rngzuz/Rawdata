using System;
using System.Collections.Generic;
using System.Linq;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create db context
            var db = new Context();

            // Translate raw SQL query to a linq query for the collection Answers
            // var query = db.Answers.FromSql("SELECT * FROM posts join posts_answer using(\"id\")");

            var questions = db.Questions
                .Include(e => e.Answers)
                .AsNoTracking()
                .Take(5)
                .ToList();

            PrintTable<Question>(questions);
        }

        static void PrintTable<T>(IList<T> list) where T : class
        {
            foreach (var obj in list)
            {
                var props = obj.GetType().GetProperties();
                Console.WriteLine("{");

                foreach (var prop in props)
                {
                    string value;

                    try {
                        value = prop
                            .GetValue(obj, null)
                            .ToString();
                    } catch {
                        continue;
                    }

                    Console.WriteLine(
                        "    {0}: {1}",
                        prop.Name,
                        value.Length > 40 ? value.Substring(0, 37) + "..." : value
                    );
                }

                Console.WriteLine("}\n");
            }
        }
    }
}
