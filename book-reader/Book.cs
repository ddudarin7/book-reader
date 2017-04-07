using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace book_reader
{
    class Book
    {
        private Dictionary<int,Page> pages;
        public int currentPage;

        public Book()
        {

        }

        public Book(string path)
        {

            LoadBook(path);
        }

        public void LoadBook(string path) {
            string[] lines = File.ReadAllLines(path);
            pages = new Dictionary<int,Page>();

            int counter = 0;
            int pageNum = 0;
            List<string> temp = new List<string>(); 
            foreach (string line in lines)
            {
                
                temp.Add(line);
               
                if(counter == lines.Length - 1)
                {
                    Page pageFin = new Page(temp);
                    this.pages.Add(pageNum,pageFin);
                    pageNum++;
                }
                if(counter % 50 == 0 && counter != 0)
                {

                    Page newPage = new Page(temp);
                    this.pages.Add(pageNum,newPage);
                    temp.Clear();
                    temp = new List<string>();
                    pageNum++;
                }
                counter++;
            }


        }



        public Dictionary<int,Page> Pages { get { return pages; } set { pages = value; } }




    }
}
