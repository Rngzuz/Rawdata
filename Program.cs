using System;
using System.Collections.Generic;
using System.Linq;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

            var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

            var json = JsonConvert
                .SerializeObject(questions, Formatting.Indented, jsonSettings);

            Console.WriteLine(json);
        }
    }
}
