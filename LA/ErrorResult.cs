using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA
{
    class ErrorResult
    {
        public string text;
        public int line;
        public int position;
        public ErrorResult(string text, int lineCnt, int posCnt)
        {
            this.text = text;
            this.line = lineCnt;
            this.position = posCnt;
        }

        public ErrorResult()
        {
            text = "Unclosed comment";
        }
    }
}
