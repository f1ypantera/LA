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
            
            string textFile = File.ReadAllText(@"D:\Test.txt");

            LexerAnalizer lexerAnalizer = new LexerAnalizer(textFile);
            lexerAnalizer.ShowKeyWords();
            lexerAnalizer.AnalizerText();
            lexerAnalizer.ShowSuccessResult();

            Console.WriteLine("\nWas been analized");
            Console.ReadLine();
        }
    }
}
