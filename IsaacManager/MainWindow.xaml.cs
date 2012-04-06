using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IsaacManagerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.DataContext = new MainWindowViewModel();
            InitializeComponent();
        }

        private void SavetoSlot(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).SaveToSlot();
        }

        private void SaveNewSlot(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).SaveNewSlot();
        }

        private void RestoreSlot(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).RestoreSlot();
        }

        private void RenameSlot(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).RenameSlot();
        }

        private void DeleteSlot(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).DeleteSlot();
        }

        private void LaunchIsaac(object sender, RoutedEventArgs e)
        {
            ((MainWindowViewModel)this.DataContext).RunIsaac();
        }
    }
}
