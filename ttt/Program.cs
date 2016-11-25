using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Data;
using System.Diagnostics;
namespace ttt
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, int> cipin = new Dictionary<string, int>();
            cipin.Add("总文档数", 0);
            StreamWriter sw = new StreamWriter("E:\\test2.txt");
            StreamWriter sw1 = new StreamWriter("E:\\test3.txt");
            Dictionary<string, string> fenlei = new Dictionary<string, string>();

            string text = System.IO.File.ReadAllText(@"E:\专业分类表.txt");
            string[] str = text.Split(';');
            for (int i = 0; i < str.Length; i++)
            {
                string[] ss = str[i].Split(',');
                fenlei.Add(ss[0], ss[1]);

            }
            int z = 0;
            string s = fenlei["哲学"];
            List<string> zhonglei = new List<string>();

            List<int> ww = new List<int>();

            foreach (var d in fenlei)
            {

                if (s != d.Value)
                {
                    ww.Add(z);
                    zhonglei.Add(s);
                }
                z++;
                s = d.Value;
            }
            zhonglei.Add(s);
            ww.Add(z);


            for (int i = 0; i < 505; i++)
            {
                StreamReader sr1 = new System.IO.StreamReader("E:\\" + i.ToString() + ".txt");
                string nextLine = sr1.ReadLine();
                int wendangshu;
                while (nextLine != null)
                {
                    string[] strs = nextLine.Split(',');
                    if (strs[0].Contains("zhuanye"))
                    {
                        cipin["总文档数"] += int.Parse(strs[1]) - 1;
                        wendangshu = int.Parse(strs[1]) - 1;
                        break;
                    }
                    nextLine = sr1.ReadLine();
                }

                sr1 = new System.IO.StreamReader("E:\\" + i.ToString() + ".txt");
                nextLine = sr1.ReadLine();
                while (nextLine != null)
                {
                    string[] strs = nextLine.Split(',');
                    if (int.Parse(strs[1]) >= 5)
                    {

                        if (cipin.ContainsKey(strs[0]))
                        {
                            cipin[strs[0]] += int.Parse(strs[1]);
                        }
                        else { cipin.Add(strs[0], int.Parse(strs[1])); }
                    }

                    nextLine = sr1.ReadLine();
                }
            }

            for (int i = 0; i < 505; i++)
            {
                StreamReader sr1 = new System.IO.StreamReader("E:\\" + i.ToString() + ".txt");
                string nextLine = sr1.ReadLine();
                int wendangshu = 5;
                string sr = null;
                while (nextLine != null)
                {
                    string[] strs = nextLine.Split(',');
                    if (strs[0].Contains("zhuanye"))
                    {

                        wendangshu = int.Parse(strs[1]) - 1;
                        sr = strs[0].Remove(strs[0].Length - 7);
                        break;
                    }
                    nextLine = sr1.ReadLine();
                }

                sr1 = new System.IO.StreamReader("E:\\" + i.ToString() + ".txt");
                nextLine = sr1.ReadLine();
                int k = 0;
                int q = 0;
                while (nextLine != null)
                {
                    string[] strs = nextLine.Split(',');

                    double tf = (Math.Round((double.Parse(strs[1]) / wendangshu), 2));
                    if (tf >= 1)
                        tf = 0.99;
                    double idf = 0;
                    if (cipin.ContainsKey(strs[0]))
                    {
                        idf = Math.Round(((double)cipin[strs[0]] / (double)cipin["总文档数"]), 2);
                    }
                    if ((int.Parse(strs[1]) >= 10) && (!strs[0].Contains("zhuanye")) && idf < 0.5 && q <= 20)
                    {
                        if (idf >= 0.1)
                            sr += "," + strs[0] + "," + Math.Round(tf * Math.Log10(1.0 / idf), 2);
                        else
                            sr += "," + strs[0] + "," + Math.Round(tf * Math.Log10(1.0 / 0.1), 2);
                        k++;
                        q++;
                    }

                    nextLine = sr1.ReadLine();

                }
                if (k >= 10)
                    sw.WriteLine(sr);
            }

            int x = 0;
            Dictionary<string, int> zhongleicipin = new Dictionary<string, int>();

            int p = 0;
            while (p < 505)
            {
                int wendangshu = 0;
                zhongleicipin.Clear();

                while (p < ww[x])
                {
                    StreamReader sr1 = new System.IO.StreamReader("E:\\" + p.ToString() + ".txt");
                    string nextLine = sr1.ReadLine();
                    
                    while (nextLine != null)
                    {
                        string[] strs = nextLine.Split(',');

                        if (!strs[0].Contains("zhuanye"))
                        {


                            if (!zhongleicipin.ContainsKey(strs[0]))
                            {
                                zhongleicipin.Add(strs[0], int.Parse(strs[1]));
                            }
                            else { zhongleicipin[strs[0]] += int.Parse(strs[1]); }

                            nextLine = sr1.ReadLine();

                        }
                        else
                        { 
                            wendangshu += int.Parse(strs[1]) - 1;
        
                        }
                        nextLine = sr1.ReadLine();
                    }


                    p++;

                    
                }
                string sr = zhonglei[x];
                foreach (var d in zhongleicipin)
                {
                    if (d.Value > 5 && cipin.ContainsKey(d.Key)) { 
                    double tf = Math.Round((double)d.Value / wendangshu, 2);
                    double idf = 0;
                    if (tf >= 1)
                        tf = 0.99;
                    idf = Math.Round((double)cipin[d.Key] / cipin["总文档数"], 2);
                    if (idf >= 0.1)
                        sr += "," + d.Key + "," + Math.Round(tf * Math.Log10(1.0 / idf), 2);
                    else
                        sr += "," + d.Key + "," + Math.Round(tf * Math.Log10(1.0 / 0.1), 2);
                    }
                }
                x++;
                    sw1.WriteLine(sr);
            }
            sw1.Close();

            sw.Close();

            
        }
    }
}
