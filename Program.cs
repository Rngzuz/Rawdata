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
            var query = db.Answers.FromSql("SELECT * FROM posts join posts_answer using(\"id\")");

            // Limit to 20 entities and execute
            var answers = query.Take(20).ToList();

            // Print shit to console for testing
            PrintTable<Answer>(answers);
        }

        static void PrintTable<T>(IList<T> list) where T : class
        {
            var charCount = 0;

            foreach (var prop in list[0].GetType().GetProperties())
            {
                var text = string.Format($"| {{0,{prop.Name.Length + 10}}} ", prop.Name);
                Console.Write(text);
                charCount += text.Length;
            }
            Console.Write("|\n");

            for (var i = 0; i < charCount + 1; i++)
            {
                Console.Write("-");
            }

            Console.Write("\n");

            foreach (var obj in list)
            {
                var props = obj.GetType().GetProperties();

                foreach (var prop in props)
                {
                    var value = prop.GetValue(obj, null)
                        .ToString();


                    if (value.Length > prop.Name.Length + 10)
                        value = value
                            .Substring(0, prop.Name.Length + 10);

                    Console.Write(string.Format($"| {{0,{prop.Name.Length + 10}}} ", value));
                }

                Console.Write("|\n");
            }
        }
    }
}
