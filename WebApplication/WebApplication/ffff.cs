
using System.Linq;
using System;
using System.Collections.Generic;


public class Solution
{


    public class Sec
    {
        private static string GetSubline(string text, string substring)
        {
            var len = Math.Max(substring.Length, text.Length);



            var minStr = text.Length == len ? text : substring;
            var maxStr = text.Length == len ? substring : text;
            var resemb = "";
            for (int length = 1; length <= minStr.Length; length++)
            {
                var hashes = new List<int>();
                for (int start = 0; start + length <= minStr.Length; start++)
                {
                    var x = minStr.Substring(start, length);
                    hashes.Add(x.GetHashCode());
                }

                var newAns = Check(length, hashes.ToArray(), maxStr);
                if (Equals(newAns, string.Empty)) return resemb;
                resemb = newAns;
            }
            return string.Empty;
        }

        public static string Check(int length, int[] hashes, string checkString)
        {
            for (int start = 0; start < checkString.Length - length + 1; start++)
            {
                var x = checkString.Substring(start, length);
                if (hashes.Contains(checkString.Substring(start, length).GetHashCode()))
                {
                    return checkString.Substring(start, length);
                }
            }

            return string.Empty;
        }

        static void Main()
        {
            Console.WriteLine(GetSubline(Console.ReadLine(), Console.ReadLine()));
        }
    }
}
 
  
