using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
 
namespace ConsoleApp1
{
    enum State
    {
        Empty,
        Wall,
        Counted,
        Visited
    };

    class Employee
    {
        public int happyEmployeesCount { get; set; }
        //public int happyEmployeesCountFake { get; set; }


    }

    public class Program
    {
        static void Main()
        {
            var input = Console.ReadLine().Split(' ');
            var rulesNumber = int.Parse(input[0]);
            var width = int.Parse(input[1]);
            var SetOfRules = new List<string[]>();
            for (int i = 0; i < rulesNumber; i++)
            {
                var rule = Console.ReadLine().Split('-');
                SetOfRules.Add(rule);
            }

            var text = Console.ReadLine()
                .Split(' ');
             //   , ',', ':',';', '.','!','?','-','+','\'','"','(',')','<','>','\\','/'
            var answer = "";

            var start = 0;
            var count = 0;
            while (text.Length > start)
            {
                if (count + text[start].Length > width)
                {
                    var prefix = new List<int>();
                    if(SetOfRules.All(x =>
                    {
                        prefix.Add(x[0].Length);
                        return text[start].ToLower().StartsWith(x[0]);
                    }))
                    {
                        var prefix1 = prefix.Max();
                        answer += text[start].Substring(0, prefix1) + "-\n";
                        count = text[start].Substring(prefix1).Length + 1; 
                        answer += text[start].Substring(prefix1) + " ";
                    }
                    else
                    {
                        answer += "\n";
                        answer += text[start] + " ";
                        count = 1 + text[start].Length;
                    }
                }
                else if (count + 1 + text[start].Length > width)
                {
                    answer += text[start] + "\n";
                    count = 0;
                }
                else
                {
                    answer += text[start] + " ";
                    count += 1 + text[start].Length;
                }
                start += 1;
                
            }
            Console.WriteLine(answer);

        }
    }
}