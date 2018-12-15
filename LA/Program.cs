using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LA
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Lexer Analizer\n");
            
            string textFile = File.ReadAllText(@"D:\GitHubProjects\Test.txt");
            textFile = textFile.Replace("\r", "");
            LexerAnalizer lexerAnalizer = new LexerAnalizer(textFile);
            lexerAnalizer.ShowKeyWords();
            lexerAnalizer.AnalizerText();
            lexerAnalizer.ShowSuccessResult();
            lexerAnalizer.ShowErrorResult();

            
            Console.ReadLine();
        }
    }
}
