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

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ovo radi");
            System.Diagnostics.Debug.WriteLine("Ovo radi");
            var fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".txt"; // Default file extension
            fileDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            var result = fileDialog.ShowDialog();

            if (result == true)
            {
                pageTxt.Text = fileDialog.FileName;

                pageTxt.Text = File.ReadAllText(fileDialog.FileName);

            }
        }
    }
}
