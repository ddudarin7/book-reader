using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace book_reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Book book;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
        }

        private void MainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
        }

        private void ZoomMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            ZoomMenu.Visibility = Visibility.Visible;
        }

        private void ZoomMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            ZoomMenu.Visibility = Visibility.Hidden;
        }

        private void Key_Pressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                Next();
            }
            if (e.Key == Key.Left)
            {
                Prev();
            }
        }

        private void Next()
        {
            if (book != null)
            {
                if (book.currentPage >= 0 && book.currentPage < book.Pages.Count - 1)
                {
                    book.currentPage++;
                    book.currentPage1++;
                    pageTxt.Text = book.Pages[book.currentPage].ToString();
                    try {
                        pageTxt11.Text = book.Pages[book.currentPage + 1].ToString();
                        pageTxt12.Text = book.Pages[book.currentPage1 + 1].ToString();
                    }
                    catch
                    {           
                        pageTxt12.Text = "End";
                    }
                }
            }
        }

        private void Next_Page(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void PrevPage()
        {
            Prev();
        }

        private void Prev()
        {
            if (book != null)
            {
                if (book.currentPage >= 1)
                {
                    book.currentPage--;
                    book.currentPage1--;
                    pageTxt.Text = book.Pages[book.currentPage].ToString();
                    try
                    {
                        pageTxt11.Text = book.Pages[book.currentPage - 1].ToString();
                        pageTxt12.Text = book.Pages[book.currentPage1 - 1].ToString();
                    }
                    catch
                    {
                        pageTxt11.Text = book.Pages[0].ToString();
                        pageTxt12.Text = book.Pages[0].ToString();
                    }
                }
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".txt"; // Default file extension
            fileDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                pageTxt.Text = fileDialog.FileName;

                book = new Book(fileDialog.FileName);
                book.currentPage = 0;
                book.currentPage1 = 1;

                pageTxt.Text = book.Pages[book.currentPage].ToString();
                pageTxt11.Text = book.Pages[book.currentPage].ToString();
                pageTxt12.Text = book.Pages[book.currentPage1].ToString();                
            }
        }

        private void Double_Page(object sender, RoutedEventArgs e)
        {
            SinglePage.Visibility = Visibility.Hidden;
            DoublePage.Visibility = Visibility.Visible;
        }

        private void Single_Page(object sender, RoutedEventArgs e)
        {
            DoublePage.Visibility = Visibility.Hidden;
            SinglePage.Visibility = Visibility.Visible;
            
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            pageTxt.FontSize = pageTxt.FontSize + 3;
            pageTxt.Width = pageTxt.Width + 100;
        }

        

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (pageTxt.FontSize <= 5) {
                return;
            }
            pageTxt.FontSize = pageTxt.FontSize - 3;
            pageTxt.Width = pageTxt.Width - 100;
        }


        private void TextBox_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }

        private void TextBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    string location = null;

                    FileInfo fi = new FileInfo(path);
                    //fi.Length  //File size
                    pageTxt.Text = fi.DirectoryName + "\\" + fi.Name; //Directory
                    
                    using (var fs = fi.OpenRead())
                    {
                        try
                        {
                            book = new Book(fi.DirectoryName + "\\" + fi.Name);
                            book.currentPage = 0;
                            book.currentPage1 = 1;
                            pageTxt.Text = book.Pages[book.currentPage].ToString();
                            pageTxt11.Text = book.Pages[book.currentPage].ToString();
                            pageTxt12.Text = book.Pages[book.currentPage1].ToString();

                        }
                        catch
                        {
                            pageTxt.Text = "Knjiga mora biti u .txt formatu.";
                        }
                    }
                }   
            }
        }

        private void Drag_Handler(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
