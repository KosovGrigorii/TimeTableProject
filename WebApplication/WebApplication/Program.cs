using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Program
    {
        
        public void Main(string[] args) {
            var first = Console.ReadLine()
                .Split(' ')
                .Select(integer=>Double.Parse(integer))
                .ToArray();
            var n = first[0];
            var x = first[1];
            var k = first[2];
            var second = Console.ReadLine()
                .Split(' ')
                .Select(integer => Double.Parse(integer))
                .ToHashSet();

            var res = 0.0;
            while (k > 0)
            {
                var min = second.Min();
                res += min;
                second.Remove(min);
                second.Add(min + x);
                k -= 1;
            }
        
            Console.WriteLine((int)res);
        }
    

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}