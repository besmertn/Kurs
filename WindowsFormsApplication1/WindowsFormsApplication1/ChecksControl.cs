using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication1
{
    class ChecksControl : GoodsDBControl
    {
        public void createCheck(string[] topText, string[] mainText, string[] bottomText, int number) {
            string path = @"checks/check";
            StreamWriter sr = File.CreateText(path + getCheckCounter(number) + ".txt");
            foreach(string line in topText){
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
            updateCheckCounter(number);
            sr.WriteLine(DateTime.Now);
            sr.Close();
        }

        public string[] generateMainCheckText(List<Goods> purchase, double summ) {
            List<string> resultText = new List<string>();
            foreach (Goods product in purchase) {
                resultText.Add(product.Name);
                double count = product.Count * product.Price;
                resultText.Add("\t" + product.Count + " X " + product.Price + " = " + count);
            }
            resultText.Add("------------------------------------------------------------------------------------");
            resultText.Add("СУММА \t" + summ);
            resultText.Add("------------------------------------------------------------------------------------");
            return resultText.ToArray();
        }
    }
}
