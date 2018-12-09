using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA
{
    class LexerAnalizer
    {
        public string text;
        private List<SuccessResult> SuccessOutput;
        private List<ErrorResult> ErrorOutput;
        private int posCnt;
        private int lineCnt;

        private Dictionary<string, int> DIVIDERS = new Dictionary<string, int>
        {
            {";", 59 },
            {"," , 42},
            {"(" , 40},
            {")" , 41},
            {"($", 6 },
            {"$)", 7 }
        };

        private Dictionary<string, int> KEYWORDS = new Dictionary<string, int>
        {
            {"PROCEDURE", 401},
            {"BEGIN", 402},
            {"END", 403},
            {"RETURN", 404}

        };
        public LexerAnalizer(string text)
        {
            this.text = text;
        }

        public void AnalizerText()
        {

            SuccessOutput = new List<SuccessResult>();
            ErrorOutput = new List<ErrorResult>();
            lineCnt = 1;
            posCnt = 1;
            for(int i = 0; i<text.Length; i++)
            {
                checkChars(i);
            }

        }

        public void checkDigits()
        {
           

        }
        public int  checkChars(int i)
        {
            if (Char.IsLetter(text[i]) && i < text.Length-1)
            {
                for(int j = 1; j <text.Length; j++)
                {
                    if (Char.IsLetterOrDigit(text[j]))
                    {
                        posCnt++;
                        continue;
                    }
               
         
                        if(KEYWORDS.ContainsKey(text.Substring(i,j - i)))
                        {
                            SuccessOutput.Add(new SuccessResult(text.Substring(i,j-i), "KEYWORD" , KEYWORDS[text.Substring(i, j - i)], lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                        }
                    
                   
                }
               
            }

            return 1;
        }

        public void ShowKeyWords()
        {
            foreach (KeyValuePair<string, int> keyValue in KEYWORDS)
            {
                Console.WriteLine("{0,-10}\t{1,5}", keyValue.Key,keyValue.Value);
            }
            Console.WriteLine("\n");
        }

        public void ShowSuccessResult()
        {           
            if (SuccessOutput != null)
            {
                Console.WriteLine("code\ttext\tlineNumber\tposition");
                foreach (SuccessResult res in SuccessOutput)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}", res.code, res.text, res.line, res.position);
                }
            }
            else
            {
                Console.WriteLine("Empty text");
            }
          
        }
        public void ShowErrorResult()
        {

        }
    }
}

 