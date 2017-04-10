﻿using System;
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
        public int currentPage1;
        private string path;

        public Book()
        {

        }

        public Book(string path)
        {
            this.path = path;
            LoadBook(path);
        }

        public string GetBookName() {
            string [] tokens=path.Split('\\');
            return tokens[tokens.Length-1];
        }

        public string Path {
            get { return path; }
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
                if(counter % 37 == 0 && counter != 0)
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
