using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Kupazer
{
    class WebPage : TabItem
    {
        private static int Id { get; set; } = 1;
        public int Index { get; set; }
        private TextBox _UrlTextBox=null;
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
        private void MakeTab()
        {
            TabIndex = Index-1;
            Header = "New Tab";
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
            Browser.MouseDown += Browser_MouseDown;
            Browser.MouseUp += Browser_MouseUp;
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

        private void Browser_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (pressedLeft)
                if (LeftButtonPressedPoint.Y - 200 < e.GetPosition(this).Y)
                    CloseRequested.Invoke(this);
                else
                    pressedLeft = false;
        }

        bool pressedLeft = false;
        Point LeftButtonPressedPoint = default(Point);

        private void Browser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                pressedLeft = true;
                LeftButtonPressedPoint = e.GetPosition(this);
            }
        }

        private void Browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Url = e.Uri.AbsoluteUri;
            Header=e.Uri.Authority;
        }

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
