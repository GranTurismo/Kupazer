using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kupazer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FirstRun();
        }

        private void FirstRun()
        {
            MakeNewTab();
        }

        private void MakeNewTab()
        {
            var newWebPage = new WebPage();
            newWebPage.CloseRequested += NewWebPage_CloseRequested;
            tabs.Items.Add(newWebPage);
            tabs.SelectedItem = newWebPage;
        }

        private void NewWebPage_CloseRequested(WebPage obj)
        {
            tabs.Items.Remove(obj);
        }

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var raisedBy = sender as TabControl;
            var selected = raisedBy.SelectedItem as TabItem;
            if (selected.Tag != null && selected.Tag as string == "newTab")
            {
                MakeNewTab();
            }
        }
    }
}
