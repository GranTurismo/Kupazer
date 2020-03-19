using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kupazer
{
    class WebPage : TabItem
    {
        private static int Id { get; set; } = 1;
        private const string _blank = "about:blank";
        public int Index { get; set; }
        private TextBox _UrlTextBox = null;
        public string Url
        {
            get
            {
                return _UrlTextBox.Text;
            }
            private set
            {
                _UrlTextBox.Text = value;
            }
        }
        public event Action<WebPage> CloseRequested;
        public WebBrowser Browser { get; private set; }
        public Button Refresh { get; private set; }
        public Button Go { get; private set; }

        public WebPage()
        {
            Index = ++Id;
            MakeTab();
        }

        Label label = null;

        private void MakeTab()
        {
            TabIndex = Index - 1;
            StackPanel headerStack = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            label = new Label()
            {
                Padding = new Thickness(0),
                Content = "New Tab",
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };
            Button button = new Button()
            {
                Padding = new Thickness(7, 0, 7, 0),
                Margin = new Thickness(4, 0, 0, 0),
                Content = "X",
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            button.Click += Button_Click1;
            headerStack.Children.Add(label);
            headerStack.Children.Add(button);
            Header = headerStack;//"New Tab";
            _UrlTextBox = new TextBox();
            _UrlTextBox.KeyUp += Url_KeyUp;
            Grid grid = new Grid()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            ColumnDefinition column1 = new ColumnDefinition();
            column1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition column2 = new ColumnDefinition()
            {
                Width = GridLength.Auto
            };
            ColumnDefinition column3 = new ColumnDefinition()
            {
                Width = GridLength.Auto
            };
            RowDefinition row1 = new RowDefinition()
            {
                Height = GridLength.Auto
            };
            RowDefinition row2 = new RowDefinition()
            {
                Height = new GridLength(1, GridUnitType.Star),
            };
            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            grid.ColumnDefinitions.Add(column1);
            grid.ColumnDefinitions.Add(column2);
            grid.ColumnDefinitions.Add(column3);
            Button refresh = new Button();
            refresh.Content = new Image()
            {
                Source = new BitmapImage(new Uri("Icon/refresh.png", UriKind.Relative)),
                Width = 20,
            };
            Button go = new Button()
            {
                Content = new Image()
                {
                    Source = new BitmapImage(new Uri("Icon/go.png", UriKind.Relative)),
                    Width = 20,
                }
            };
            refresh.Click += Button_Click;
            go.Click += go_Click;
            Browser = new WebBrowser();
            Browser.Navigated += Browser_Navigated;
            grid.Children.Add(_UrlTextBox);
            grid.Children.Add(refresh);
            grid.Children.Add(go);
            grid.Children.Add(Browser);
            Grid.SetColumn(_UrlTextBox, 0);
            Grid.SetRow(_UrlTextBox, 0);
            Grid.SetColumn(go, 1);
            Grid.SetRow(go, 0);
            Grid.SetColumn(refresh, 2);
            Grid.SetRow(refresh, 0);
            Grid.SetColumnSpan(Browser, 3);
            Grid.SetRow(Browser, 1);
            Content = grid;
        }

        internal void ResetTab()
        {
            label.Content = "New Tab";
            Browser.Navigate(_blank);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            CloseRequested.Invoke(this);
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Url = e.Uri.AbsoluteUri;
            if (e.Uri.AbsoluteUri == _blank)
            {
                Url = string.Empty;
                ChangeTitle("New Tab");
            }
            else
                ChangeTitle(e.Uri.Authority);
        }

        private void ChangeTitle(string authority) => label.Content = authority;

        private void Url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                go_Click(sender, new RoutedEventArgs());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Browser.Refresh();
        }

        private void go_Click(object sender, RoutedEventArgs e)
        {
            if (!Url.Contains("http://") && !Url.Contains("https://"))
                Url = "http://" + Url;
            Browser.Navigate(Url);
        }
    }
}
