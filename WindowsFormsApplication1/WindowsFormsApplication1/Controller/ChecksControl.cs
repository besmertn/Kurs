using System;
using System.Collections.Generic;
using System.IO;

namespace WindowsFormsApplication1.Controller
{
    internal class ChecksControl : GoodsDbControl
    {
        public string CreateCheck(string[] topText, string[] mainText, string[] bottomText, int number)
        {
            string path = @"checks" + number;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = path + "/check" + GetCheckCounter(number) + ".txt";
            StreamWriter sr = File.CreateText(path);
            foreach (string line in topText)
            {
                sr.WriteLine(line);
            }
            foreach (string line in mainText)
            {
                sr.WriteLine(line);
            }
            foreach (string line in bottomText)
            {
                sr.WriteLine(line);
            }
            UpdateCheckCounter(number);
            sr.WriteLine(DateTime.Now);
            sr.Close();
            return path;
        }

        public string[] GenerateMainCheckText(List<Goods> purchase, double summ)
        {
            List<string> resultText = new List<string>();
            foreach (Goods product in purchase)
            {
                resultText.Add(product.Name);
                double count = product.Count*product.Price;
                resultText.Add("\t" + product.Count + " X " + product.Price + " = " + count);
            }
            resultText.Add("------------------------------------------------------------------------------------");
            resultText.Add("СУММА \t" + summ);
            resultText.Add("------------------------------------------------------------------------------------");
            return resultText.ToArray();
        }
    }
}
