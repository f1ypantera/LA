using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA
{
    class Lexer
    {
        public string text { get; }
        public int code { get; }
        public int category { get; }
        public Lexer(string text)
        {
            this.text = text;
        }
    }
}
