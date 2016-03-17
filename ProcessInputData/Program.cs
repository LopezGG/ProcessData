using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProcessInputData
{
    class Program
    {
        static void Main (string[] args)
        {
            string DataPath = args[0];
            int MinCount = Convert.ToInt32(args[1]);
            //this multiplied by the doc Count is used
            double maxFactor = Convert.ToDouble(args[2]);
            string TrainPath = args[3];
            string TestPath = args[4];
            List< Document> DocumentCollection = new List< Document>();
            HashSet<string> VocabSet = new HashSet<string>();
            Dictionary<string, int> Vocab = new Dictionary<string, int>();
            ReadData(DataPath, ref DocumentCollection, ref Vocab);

            maxFactor *= DocumentCollection.Count;

            foreach (var item in Vocab)
            {
                if (item.Value >= MinCount && item.Value <= maxFactor)
                    VocabSet.Add(item.Key);
            }

            int DocCount = 0;
            StreamWriter Strain = new StreamWriter(TrainPath);
            StreamWriter Stest = new StreamWriter(TestPath);
            foreach (var Doc in DocumentCollection)
            {

                List <string> Sb = new List<string>();
                Sb.Add(Doc.ClassLabel);
                foreach (var word in Doc.Words)
                {
                    if(VocabSet.Contains(word))
                        Sb.Add(word + ":1");
                }
                if (Sb.Count > 1)
                {
                    DocCount++;
                    if (DocCount%10 <= 7 )
                        Strain.WriteLine(string.Join(" ", Sb.ToArray()));
                    else
                        Stest.WriteLine(string.Join(" ", Sb.ToArray()));
                }
                    
            }
            Strain.Close();
            Stest.Close();
            Console.WriteLine("done");
            Console.ReadLine();

        }
        //This reads the input separates ClassLabels and stores count
        public static void ReadData (string DataPath, ref  List< Document> DocumentCollection , ref Dictionary<string, int> Vocab)
        {
            Regex rgx = new Regex("[^a-zA-Z\']");
            string line = "", classLabel = "";
            using (StreamReader Sr = new StreamReader(DataPath))
            {
                while ((line = Sr.ReadLine()) != null)
                {
                    line = rgx.Replace(line, " ");
                    line = line.ToLower();
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    string[] words = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    classLabel = words[0];
                    Document temp = new Document(classLabel);
                    for (int i = 1; i < words.Length; i++)
                    {
                        temp.Words.Add(words[i]);
                        if (Vocab.ContainsKey(words[i]))
                            Vocab[words[i]]++;
                        else
                            Vocab.Add(words[i], 1);
                    }
                    temp.Words = temp.Words.Distinct().ToList();
                    DocumentCollection.Add(temp);
                }
            }
        }
    }
}
