using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LA
{
    class SuccessResult
    {
        public string text;
        public string category;
        public int code;
        public int line;
        public int position;

        public SuccessResult(string text, string category, int code, int line, int position)
        {
            this.text = text;
            this.category = category;
            this.code = code;
            this.line = line;
            this.position = position;
        }
    }
}
