using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace NLP_Assingmnet2_cs
{
    internal class Program
    {
        public static string enJson_en = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\app_data\en.json";
        public static string output_en = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\output\94463125_Assignment2_Part1_EN.txt";
        public static string enJson_fa = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\app_data\fa.json";
        public static string output_fa = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\output\94463125_Assignment2_Part1_FA.txt";
        public static string output_en_count = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\output\94463125_Assignment2_Part2_EN.txt";
        public static string output_fa_count = @"D:\Doros\term 8\NLP\Assignment\NLP_Assingmnet2_cs\NLP_Assingmnet2_cs\output\94463125_Assignment2_Part2_FA.txt";

        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var dict = new Dictionary<string, int>();
            string[] lines = File.ReadAllLines(enJson_fa);
            wordCounter(lines);
            sentensCounter(lines);
            Console.ReadLine();
        }

        private static void sentensCounter(string[] lines)
        {
            var dict = new Dictionary<string, int>();
            foreach (var item in lines)
            {
                Item res = JsonConvert.DeserializeObject<Item>(item);
                string body = res.body;
                body = body.ToLower();

                string[] sentens = Regex.Split(body, @"(?<=[\.!\?])\s+");

                //Console.WriteLine(res.body);
                foreach (var senten in sentens)
                    if (dict.ContainsKey(senten))
                        dict[senten]++;
                    else
                        dict[senten] = 1;
            }
            int sentenceCount = 0;
            int AverageSentenceLength = 0;
            foreach (KeyValuePair<string, int> kvp in dict)
            {
                sentenceCount += kvp.Value;
                AverageSentenceLength += kvp.Key.Length;
            }
            AverageSentenceLength = AverageSentenceLength / dict.Values.Count;
            string output = sentenceCount.ToString() + "  " + AverageSentenceLength.ToString() + Environment.NewLine;
            output += String.Join(
               "\n",
               dict
                    .Where(kvp => kvp.Value > 1 && kvp.Key.Length >= 10 && kvp.Key.Length <= 11)
                    .Select(kvp =>
                        String.Format(
                           kvp.Key)).Take(10)
                    .ToArray());

            if (!File.Exists(output_fa))
            {
                File.AppendAllText(output_fa, output);
            }
            Console.WriteLine(output);
        }

        public static void wordCounter(string[] lines)
        {
            var dict = new Dictionary<string, int>();
            foreach (var item in lines)
            {
                Item res = JsonConvert.DeserializeObject<Item>(item);
                string body = res.body;
                body = body.ToLower();
                string[] words = Regex.Split(body, "\\W+");
                //Console.WriteLine(res.body);
                foreach (var word in words)
                    if (dict.ContainsKey(word))
                        dict[word]++;
                    else
                        dict[word] = 1;
            }

            string output = String.Join(

                "\n",
                dict
                     .Where(kvp => kvp.Value > 1).OrderByDescending(p => p.Value)
                     .Select(kvp =>
                         String.Format(
                          "\"{0}\" = {1}",
                             kvp.Key,
                             kvp.Value)).Take(1000)
                     .ToArray());

            if (!File.Exists(output_fa_count))
            {
                File.AppendAllText(output_fa_count, output);
            }
            Console.WriteLine(output);
        }
    }

    public class Item
    {
        public string id;
        public string title;
        public string body;
    }
}