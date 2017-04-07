using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book_reader
{
    class Page
    {
        List<string> lines;
        string text;

        public Page()
        {

        }

        public Page(List<string> lines)
        {
            List<string> newList = new List<string>(lines);
            this.lines = newList;
        }

        public List<string> Lines { get { return lines; } set { lines = value; } }

        override public string ToString()
        {
            string res = "";
            foreach (string line in lines)
            {
                res = res + line + "\n";
            }
            return res;

        }

        public string ToText()
        {
            string res = ""; 
            foreach(string line in lines)
            {
                res = res + line + "\n";
            }
            return res;
        }




    }
}
