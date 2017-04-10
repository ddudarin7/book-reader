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
using System.Windows.Forms;

namespace book_reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Book book;
        private bool day = true;
        private SolidColorBrush tempFore = null;
        private SolidColorBrush tempPage = null;
        private SolidColorBrush tempBack = null;

        private int counter = 0;
        private int counter1 = 0;
        private int counter2 = 0;
        TextRange myRange = null;
        TextRange myRange1 = null;
        TextRange myRange2 = null;

        string searched = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainMenu_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MainMenu.Visibility = Visibility.Visible;
        }

        private void MainMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MainMenu.Visibility = Visibility.Hidden;
        }

        private void ZoomMenu_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ZoomMenu.Visibility = Visibility.Visible;
        }

        private void ZoomMenu_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ZoomMenu.Visibility = Visibility.Hidden;
        }

        private void Key_Pressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                Next();
            }
            if (e.Key == Key.Left)
            {
                Prev();
            }
            if(e.Key == Key.Enter)
            {
                Search();
            }
        }
        private void Key_Pressed2(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                changePage();
            }
            
        }

        private void Next()
        {
            if (book != null)
            {
                if (book.currentPage >= 0 && book.currentPage < book.Pages.Count - 1)
                {

                    if (DoublePage.Visibility == Visibility.Hidden)
                    {
                        book.currentPage++;
                        pageTxt.Document.Blocks.Clear();
                        pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                    }
                    else
                    {
                        try
                        {
                            if (book.currentPage + 2 <= book.Pages.Count - 1)
                            {
                                book.currentPage = book.currentPage + 2;
                            }
                            else
                            {
                                book.currentPage++;
                            }
                            DoublePageRefresh();
                        }
                        catch
                        {
                            pageTxt12.Document.Blocks.Clear();
                            pageTxt12.Document.Blocks.Add(new Paragraph(new Run("End")));
                        }
                    }
                }
            }
        }

        private void DoublePageRefresh()
        {
            try
            {
                if (book.currentPage % 2 == 0)
                {
                    pageTxt11.Document.Blocks.Clear();
                    pageTxt12.Document.Blocks.Clear();
                    pageTxt11.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                    pageTxt12.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage + 1].ToString())));
                }
                else
                {
                    pageTxt11.Document.Blocks.Clear();
                    pageTxt12.Document.Blocks.Clear();
                    pageTxt11.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage - 1].ToString())));
                    pageTxt12.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                }
            }
            catch
            {
                pageTxt11.Document.Blocks.Clear();
                pageTxt12.Document.Blocks.Clear();
                if (book != null)
                {
                    pageTxt11.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                    pageTxt12.Document.Blocks.Add(new Paragraph(new Run("End")));
                }
                else {
                    pageTxt11.Document.Blocks.Add(new Paragraph(new Run("Select import from menu to choose a book, or drag and drop the book to the paper.")));
                    pageTxt12.Document.Blocks.Add(new Paragraph(new Run("")));
                }
            }
        }

        private void Next_Page(object sender, RoutedEventArgs e)
        {
            Next();
        }

        private void PrevPage(object sender, RoutedEventArgs e)
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
                        pageTxt.Document.Blocks.Clear();
                        pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                    }
                    else
                    {
                        if (book.currentPage < 2)
                        {
                            return;
                        }
                        else
                        {
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
                book = new Book(fileDialog.FileName);
                book.currentPage = 0;
                book.currentPage1 = 1;

                pageTxt.Document.Blocks.Clear();
                pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                pageTxt11.Document.Blocks.Clear();
                pageTxt12.Document.Blocks.Clear();
                pageTxt11.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                pageTxt12.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage + 1].ToString())));
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
            if (book != null) { 
                pageTxt.Document.Blocks.Clear();
            
                pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
            }
        }

        private void ZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (SinglePage.Visibility == Visibility.Visible)
            {
                ZoomInSigleView();
            }
            else
            {
                ZoomInDoubleView();
            }
        }

        private void ZoomInSigleView()
        {
            pageTxt.FontSize = pageTxt.FontSize + 3;
            pageTxt.Width = pageTxt.Width + 100;
        }

        private void ZoomInDoubleView()
        {
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
            else
            {
                ZoomOutDoubleView();
            }
        }

        private void ZoomOutSingleView()
        {
            if (pageTxt.FontSize <= 5)
            {
                return;
            }
            pageTxt.FontSize = pageTxt.FontSize - 3;
            pageTxt.Width = pageTxt.Width - 100;
        }

        private void ZoomOutDoubleView()
        {
            if (pageTxt11.FontSize <= 5)
            {
                return;
            }
            pageTxt11.FontSize = pageTxt11.FontSize - 3;
            pageTxt11.Width = pageTxt11.Width - 100;
            pageTxt12.FontSize = pageTxt12.FontSize - 3;
            pageTxt12.Width = pageTxt12.Width - 100;
        }

        private void TextBox_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            e.Effects = System.Windows.DragDropEffects.Copy;
        }

        private void TextBox_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                string[] droppedFilePaths = e.Data.GetData(System.Windows.DataFormats.FileDrop, true) as string[];
                foreach (var path in droppedFilePaths)
                {
                    string location = null;

                    FileInfo fi = new FileInfo(path);
                    //fi.Length  //File size
                    //pageTxt.Text = fi.DirectoryName + "\\" + fi.Name; //Directory
                    pageTxt.Document.Blocks.Clear();
                    pageTxt.Document.Blocks.Add(new Paragraph(new Run(fi.DirectoryName + "\\" + fi.Name)));
                    using (var fs = fi.OpenRead())
                    {
                        try
                        {
                            book = new Book(fi.DirectoryName + "\\" + fi.Name);
                            book.currentPage = 0;
                            book.currentPage1 = 1;
                            pageTxt.Document.Blocks.Clear();
                            pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                            pageTxt11.Document.Blocks.Clear();
                            pageTxt12.Document.Blocks.Clear();
                            pageTxt11.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                            pageTxt12.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage + 1].ToString())));

                        }
                        catch
                        {
                            pageTxt.Document.Blocks.Clear();
                            pageTxt.Document.Blocks.Add(new Paragraph(new Run("Knjiga mora biti u .txt formatu!")));
                        }
                    }
                }
            }
        }

        private void Drag_Handler(object sender, System.Windows.DragEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (book != null)
            {
                string[] lines = { book.Path, book.currentPage.ToString(), book.currentPage1.ToString() };
                System.IO.File.WriteAllLines(@"..\..\metaData\metaData.txt", lines);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\metaData\metaData.txt");
            if (lines.Length == 0)
            {
                return;
            }
            else
            {
                book = new Book(lines[0]);
                book.currentPage = Int32.Parse(lines[1]);
                book.currentPage1 = Int32.Parse(lines[2]);

                pageTxt.Document.Blocks.Clear();
                pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));
                DoublePageRefresh();
            }
        }

        private void GoToPage(object sender, RoutedEventArgs e)
        {
            changePage();
        }

        private void changePage()
        {
            try
            {
                book.currentPage = Convert.ToInt32(PageInput.Text);
                pageTxt.Document.Blocks.Clear();
                pageTxt.Document.Blocks.Add(new Paragraph(new Run(book.Pages[book.currentPage].ToString())));

                DoublePageRefresh();
            }
            catch { }
        }

        private void MenuItemColor_Click(object sender, RoutedEventArgs e)
        {
            if (SinglePage.Visibility == Visibility.Visible)
            {
                TextRange range = new TextRange(pageTxt.Selection.Start, pageTxt.Selection.End);

                range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Red));
            }
            else
            {
                if (!pageTxt11.Selection.IsEmpty)
                {
                    TextRange range = new TextRange(pageTxt11.Selection.Start, pageTxt11.Selection.End);

                    range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Red));
                }
                if (!pageTxt12.Selection.IsEmpty)
                {
                    TextRange range = new TextRange(pageTxt12.Selection.Start, pageTxt11.Selection.End);

                    range.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Red));
                }
            }
        }

        private void colorPicker(object sender, RoutedEventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;


            if ((sender as System.Windows.Controls.Button).Name.ToString() == "Text")
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                    SolidColorBrush color1 = new SolidColorBrush(color);
                    pageTxt.Foreground = color1;
                    pageTxt11.Foreground = color1;
                    pageTxt12.Foreground = color1;
                    tempFore = color1;
                }
            }
            if ((sender as System.Windows.Controls.Button).Name.ToString() == "Page")
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                    SolidColorBrush color1 = new SolidColorBrush(color);
                    pageTxt.Background = color1;
                    pageTxt11.Background = color1;
                    pageTxt12.Background = color1;
                    tempPage = color1;
                }
            }
            if ((sender as System.Windows.Controls.Button).Name.ToString() == "Background1")
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
                    SolidColorBrush color1 = new SolidColorBrush(color);
                    Background.Background = color1;
                    tempBack = color1;
                }
            }
        }

        private void ColorsChangeClick(object sender, RoutedEventArgs e)
        {
            var colorFore = Color.FromRgb(84, 35, 28);
            SolidColorBrush color1 = new SolidColorBrush(colorFore);
            var colorPage = Color.FromRgb(246, 197, 190);
            SolidColorBrush color2 = new SolidColorBrush(colorPage);
            var white = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            var black = new SolidColorBrush(Color.FromRgb(0, 0, 0));

            if (day)
            {
                pageTxt.Foreground = color1;
                pageTxt.Background = color2;
                pageTxt11.Foreground = color1;
                pageTxt11.Background = color2;
                pageTxt12.Foreground = color1;
                pageTxt12.Background = color2;
                Background.Background = color2;
                //prebaci na noc
                day = false;
            }
            else
            {
                if (tempFore != null)
                {
                    pageTxt.Foreground = tempFore;
                    pageTxt.Background = white;
                    pageTxt11.Foreground = tempFore;
                    pageTxt11.Background = white;
                    pageTxt12.Foreground = tempFore;
                    pageTxt12.Background = white;
                    Background.Background = new SolidColorBrush(Color.FromRgb(95, 158, 160));
                }
                if (tempPage != null)
                {
                    pageTxt.Foreground = black;
                    pageTxt.Background = tempPage;
                    pageTxt11.Foreground = black;
                    pageTxt11.Background = tempPage;
                    pageTxt12.Foreground = black;
                    pageTxt12.Background = tempPage;
                    Background.Background = new SolidColorBrush(Color.FromRgb(95, 158, 160));
                }
                if (tempBack != null) {
                    pageTxt.Foreground = black;
                    pageTxt.Background = white;
                    pageTxt11.Foreground = black;
                    pageTxt11.Background = white;
                    pageTxt12.Foreground = black;
                    pageTxt12.Background = white;
                    Background.Background = tempBack;
                }
                if (tempFore == null && tempPage == null && tempBack == null)
                {
                    pageTxt.Foreground = black;
                    pageTxt.Background = white;
                    pageTxt11.Foreground = black;
                    pageTxt11.Background = white;
                    pageTxt12.Foreground = black;
                    pageTxt12.Background = white;
                    Background.Background = new SolidColorBrush(Color.FromRgb(95, 158, 160));
                }
                day = true;
            }
        }

        private void Search_Click_Event(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            string wordsToSearch = SearchBox.Text;
            string oldSearched = searched;
            searched = wordsToSearch;
            if (searched != oldSearched)
            {
                counter = 0;
            }
            try
            {
                if (SinglePage.Visibility == Visibility.Visible)
                {
                    string richText = new TextRange(pageTxt.Document.ContentStart, pageTxt.Document.ContentEnd).Text;
                    if (richText.Contains(wordsToSearch))
                    {
                        if (counter == 0)
                        {
                            try {
                                if (searched != oldSearched)
                                {
                                    myRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));

                                }
                            }
                            catch
                            { }
                            myRange = FindWordFromPosition(pageTxt.Document.ContentStart, wordsToSearch);
                            myRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));
                            counter++;
                        }
                        else if (counter > 0)
                        {
                            if (searched != oldSearched)
                            {
                                myRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));
                                
                            }
                            myRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));
                            myRange = FindWordFromPosition(myRange.End, wordsToSearch);
                            myRange.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));
                            counter++;
                        }
                    }
                }
                else
                {
                    string richText11 = new TextRange(pageTxt11.Document.ContentStart, pageTxt11.Document.ContentEnd).Text;
                    string richText12 = new TextRange(pageTxt12.Document.ContentStart, pageTxt12.Document.ContentEnd).Text;
                    if (richText11.Contains(wordsToSearch))
                    {
                        if (counter1 == 0)
                        {
                            myRange1 = FindWordFromPosition(pageTxt11.Document.ContentStart, wordsToSearch);
                            myRange1.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));
                            counter1++;
                        }
                        else if (counter1 > 0)
                        {
                            if (searched != oldSearched)
                            {
                                myRange1.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));

                            }
                            myRange1.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));
                            myRange1 = FindWordFromPosition(myRange1.End, wordsToSearch);
                            myRange1.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));

                            counter1++;
                        }
                    }

                    if (richText12.Contains(wordsToSearch))
                    {
                        if (counter2 == 0)
                        {
                            myRange2 = FindWordFromPosition(pageTxt12.Document.ContentStart, wordsToSearch);
                            myRange2.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));
                            counter2++;
                        }
                        else if (counter2 > 0)
                        {
                            if (searched != oldSearched)
                            {
                                myRange2.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));
                            }
                            myRange2.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.White));
                            myRange2 = FindWordFromPosition(myRange2.End, wordsToSearch);
                            myRange2.ApplyPropertyValue(FlowDocument.BackgroundProperty, new SolidColorBrush(Colors.Orange));
                            counter2++;
                        }
                    }
                }
            }
            catch { }
        }

        TextRange FindWordFromPosition(TextPointer position, string word)
        {
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    string textRun = position.GetTextInRun(LogicalDirection.Forward);

                    // Find the starting index of any substring that matches "word".
                    int indexInRun = textRun.IndexOf(word);
                    if (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun);
                        TextPointer end = start.GetPositionAtOffset(word.Length);
                        return new TextRange(start, end);
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }
            // position will be null if "word" is not found.
            return null;
        }
    }
}
