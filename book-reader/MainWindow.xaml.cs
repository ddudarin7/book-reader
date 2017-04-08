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
                    
                    if (DoublePage.Visibility == Visibility.Hidden) {
                        book.currentPage++;
                        pageTxt.Text = book.Pages[book.currentPage].ToString();
                    }
                    else
                    {
                        try
                        {
                            if (book.currentPage + 2 <= book.Pages.Count - 1)
                            {
                                book.currentPage = book.currentPage + 2;
                            }
                            else {
                                book.currentPage++;
                            }
                            DoublePageRefresh();
                        }
                        catch
                        {
                            pageTxt12.Text = "End";
                        }
                    }
                }
            }
        }

        private void DoublePageRefresh() {
            try
            {
                if (book.currentPage % 2 == 0)
                {
                    pageTxt11.Text = book.Pages[book.currentPage].ToString();
                    pageTxt12.Text = book.Pages[book.currentPage + 1].ToString();
                }
                else
                {
                    pageTxt11.Text = book.Pages[book.currentPage - 1].ToString();
                    pageTxt12.Text = book.Pages[book.currentPage].ToString();
                }
            }
            catch {
                pageTxt11.Text = book.Pages[book.currentPage].ToString();
                pageTxt12.Text = "End";
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
                    if (DoublePage.Visibility == Visibility.Hidden)
                    {
                        book.currentPage--;
                        pageTxt.Text = book.Pages[book.currentPage].ToString();
                    }
                    else {
                        if (book.currentPage < 2)
                        {
                            return;
                        }
                        else {
                            book.currentPage = book.currentPage - 2;
                            DoublePageRefresh();
                        }
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
                pageTxt12.Text = book.Pages[book.currentPage+1].ToString();                
            }
        }

        private void Double_Page(object sender, RoutedEventArgs e)
        {
            SinglePage.Visibility = Visibility.Hidden;
            DoublePage.Visibility = Visibility.Visible;

            DoublePageRefresh();
        }

        private void Single_Page(object sender, RoutedEventArgs e)
        {
            DoublePage.Visibility = Visibility.Hidden;
            SinglePage.Visibility = Visibility.Visible;
            pageTxt.Text = book.Pages[book.currentPage].ToString();
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (SinglePage.Visibility == Visibility.Visible)
            {
                ZoomInSigleView();
            }
            else {
                ZoomInDoubleView();
            }
        }

        private void ZoomInSigleView() {
            pageTxt.FontSize = pageTxt.FontSize + 3;
            pageTxt.Width = pageTxt.Width + 100;
        }

        private void ZoomInDoubleView() {
            pageTxt11.FontSize = pageTxt11.FontSize + 3;
            pageTxt11.Width = pageTxt11.Width + 100;
            pageTxt12.FontSize = pageTxt12.FontSize + 3;
            pageTxt12.Width = pageTxt12.Width + 100;
        }

        private void ZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (SinglePage.Visibility == Visibility.Visible)
            {
                ZoomOutSingleView();
            }
            else {
                ZoomOutDoubleView();
            }
        }

        private void ZoomOutSingleView() {
            if (pageTxt.FontSize <= 5)
            {
                return;
            }
            pageTxt.FontSize = pageTxt.FontSize - 3;
            pageTxt.Width = pageTxt.Width - 100;
        }

        private void ZoomOutDoubleView() {
            if (pageTxt11.FontSize <= 5)
            {
                return;
            }
            pageTxt11.FontSize = pageTxt11.FontSize - 3;
            pageTxt11.Width = pageTxt11.Width - 100;
            pageTxt12.FontSize = pageTxt12.FontSize - 3;
            pageTxt12.Width = pageTxt12.Width - 100;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (book != null) {
                string[] lines = { book.Path, book.currentPage.ToString(), book.currentPage1.ToString() };
                System.IO.File.WriteAllLines(@".\metaData.txt", lines);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] lines=System.IO.File.ReadAllLines(@".\metaData.txt");
            if (lines.Length == 0)
            {
                return;
            }
            else {
                book=new Book(lines[0]);
                book.currentPage = Int32.Parse(lines[1]);
                book.currentPage1 = Int32.Parse(lines[2]);
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
                pageTxt.Text = book.Pages[book.currentPage].ToString();
                DoublePageRefresh();
            }
        }

        private void GoToPage(object sender, RoutedEventArgs e)
        {
            book.currentPage = Convert.ToInt32(PageInput.Text);
            //book.currentPage1 = book.currentPage + 1;
            pageTxt.Text = book.Pages[book.currentPage].ToString();

            DoublePageRefresh();


        }
    }
}
