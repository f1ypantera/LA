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
            {";", 40 },
            {"," , 41},
            {"(" , 42},
            {")" , 43},
            {"($", 44 },
            {"$)", 45 },
             {"+", 46 }
        };

        private Dictionary<string, int> KEYWORDS = new Dictionary<string, int>
        {
            {"PROCEDURE", 401},
            {"BEGIN", 402},
            {"END", 403},
            {"RETURN", 404},
            {"PHONE", 405}

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

            for (int i = 0; i < text.Length; i++)
            {

                if (text[i] == '\n')
                {
                    lineCnt++;
                    posCnt = 1;
                }
                if (i - 1 >= 0 && text[i - 1] == '\n')
                {
                    posCnt = 1;
                }

              
                int tmp = checkDividers(i);
                    if (tmp >= 0)
                    {
                        i = tmp;
                    }
                    else if (tmp == -2)
                    {
                        return;
                    }
                    if (i != tmp)
                    {

                        tmp = checkDigits(i);
                        if (tmp >= 0)
                        {
                            i = tmp;
                        }

                        if (i != tmp)
                        {

                            tmp = checkChars(i);
                            if (tmp >= 0)
                            {
                                i = tmp;
                            }

                            if (i != tmp && text[i] != ' ' && text[i] != '\n')
                            {
                                ErrorOutput.Add(new ErrorResult("UNEXPECTED SYMBOL " + text[i].ToString(), lineCnt, posCnt));
                            }

                        }
                    }


                    posCnt++;
                }

            }
        



        public int checkDividers(int i)
        {



            if (DIVIDERS.ContainsKey(text[i].ToString()))
            {

                if (text[i + 1] != '*' && text[i + 1] != '$')
                {

                    if (text[i].ToString() == "+" && text[i + 1].ToString() == "(" && text[i + 5].ToString() == ")" )
                    {
                        SuccessOutput.Add(new SuccessResult(text.Substring(i, 13), "PHONE NUMBER", 405, lineCnt, posCnt));
                        return i + 13;
                    }
                    else
                    {
                        SuccessOutput.Add(new SuccessResult(text[i].ToString(), "DIVIDER", DIVIDERS[text[i].ToString()], lineCnt, posCnt));
                        return i;
                    }

                }
                else
                {

                    if (text[i + 1] == '$')
                    {
                        SuccessOutput.Add(new SuccessResult(text.Substring(i, 2), "DIVIDER", DIVIDERS[text.Substring(i, 2)], lineCnt, posCnt));
                        return i + 1;
                    }
                    int startLine = lineCnt;
                    int startPos = posCnt;
                    for (int j = i + 2; j < text.Length; j++)
                    {

                        if (text[j] == '\n')
                        {
                            lineCnt++;
                            posCnt = 1;
                        }
                        if (text[j] == '*' && j + 1 != text.Length && text[j + 1] == ')')
                        {
                            posCnt += 2;
                            return j + 1;
                        }
                        else if (j + 1 >= text.Length)
                        {
                            ErrorOutput.Add(new ErrorResult("UNCLOSED COMMENT", startLine, startPos));
                            return j + 1;
                        }
                        else
                        {
                            posCnt++;
                        }
                    }
                }
            }
            else if (text[i] == '$' && i + 1 < text.Length - 1 && text[i + 1] == ')')
            {
                SuccessOutput.Add(new SuccessResult(text.Substring(i, 2), "DIVIDER", DIVIDERS[text.Substring(i, 2)], lineCnt, posCnt));
                return i + 1;
            }
            return -1;
        }

        public int checkDigits(int i)
        {
            if (Char.IsDigit(text[i]) && i < text.Length - 1)
            {
                for (int j = i + 1; j < text.Length; j++)
                {

                    if (Char.IsDigit(text[j]))
                    {
                        posCnt++;
                        continue;
                    }
                    else if (text[j] == ' ' || text[j] == '\n')
                    {
                        int index = SuccessOutput.FindIndex(identifier => identifier.text == text.Substring(i, j - i));
                        if (index >= 0)
                        {
                            SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "IDENTIFIER", SuccessOutput[index].code, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                        }
                        else
                        {
                            int count = SuccessOutput.Count(identifier => identifier.category == "DIGIT");
                            SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "DIGIT", 500 + count + 1, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                        }
                        return j - 1;
                    }
                    else if (DIVIDERS.ContainsKey(text[j].ToString()))
                    {
                        SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "DIGIT", 501, lineCnt, posCnt));
                        return j - 1;
                    }
                    else
                    {
                        int lineStart = lineCnt;
                        while (text[j] != ' ' && text[j] != '\n')
                        {
                            if (text[j] == '\n')
                            {
                                lineCnt++;
                                posCnt = 1;
                            }
                            posCnt++;
                            j++;
                        }
                        ErrorOutput.Add(new ErrorResult("UNEXPECTED SYMBOL  " + text.Substring(i, j - i), lineStart, posCnt + 1 - text.Substring(i, j - i).Length));
                        return j - 1;
                    }
                }
            }
            return -1;
        }

        public int checkChars(int i)
        {
            if (Char.IsLetter(text[i]) && i < text.Length - 1)
            {
                for (int j = i + 1; j < text.Length; j++)
                {
                    if (Char.IsLetterOrDigit(text[j]))
                    {
                        posCnt++;
                        continue;
                    }
                    else if (text[j] == ' ' || text[j] == '\n' || DIVIDERS.ContainsKey(text[j].ToString()))
                    {
                        if (KEYWORDS.ContainsKey(text.Substring(i, j - i)))
                        {
                            SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "KEYWORD", KEYWORDS[text.Substring(i, j - i)], lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                        }
                        else
                        {
                            int index = SuccessOutput.FindIndex(identifier => identifier.text == text.Substring(i, j - i));
                            if (index >= 0)
                            {
                                SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "IDENTIFIER", SuccessOutput[index].code, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                            }
                            else
                            {
                                int count = SuccessOutput.Count(identifier => identifier.category == "IDENTIFIER");
                                SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "IDENTIFIER", 1000 + count + 1, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                            }
                        }
                        return j - 1;
                    }
                    else
                    {
                        if (KEYWORDS.ContainsKey(text.Substring(i, j - i)))
                        {
                            SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "KEYWORD", KEYWORDS[text.Substring(i, j - i)], lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                        }
                        else
                        {
                            int index = SuccessOutput.FindIndex(identifier => identifier.text == text.Substring(i, j - i));
                            if (index >= 0)
                            {
                                SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "IDENTIFIER", SuccessOutput[index].code, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                            }
                            else
                            {
                                int count = SuccessOutput.Count(identifier => identifier.category == "IDENTIFIER");
                                SuccessOutput.Add(new SuccessResult(text.Substring(i, j - i), "IDENTIFIER", 1000 + count + 1, lineCnt, posCnt + 1 - text.Substring(i, j - i).Length));
                            }

                        }

                        return j;
                    }
                }
            }
            return -1;

        }








        public void ShowKeyWords()
        {
            foreach (KeyValuePair<string, int> keyValue in KEYWORDS)
            {
                Console.WriteLine("{0,-10}\t{1,5}", keyValue.Key, keyValue.Value);
            }
            Console.WriteLine("\n");
        }

        public void ShowSuccessResult()
        {
            if (SuccessOutput != null)
            {
                Console.WriteLine("{0}\t{1,8}\t{2,10}\t{3}\t{4}", "code", "value", "category", "line", "position");
                foreach (SuccessResult res in SuccessOutput)
                {
                    Console.WriteLine("{0}\t{1,8}\t{2,10}\t{3,2}\t{4,4}", res.code, res.text, res.category, res.line, res.position);

                }
            }
            else
            {
                Console.WriteLine("Empty text");
            }

        }
        public void ShowErrorResult()
        {
            if (ErrorOutput.Count == 0)
            {
                Console.WriteLine("\nDO NOT HAVE ERRORS");
            }
            else
            {
                Console.Write("\n\n------------LIST OF ERRORS------------\n");
                foreach (ErrorResult err in ErrorOutput)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error(" + err.line + "," + err.position + "):" + err.text);
                }


            }
        }
    }
}


