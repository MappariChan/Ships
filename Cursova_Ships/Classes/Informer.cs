using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cursova_Ships.Classes
{
    //клас що повертає форму інформації про корабель
    public static class Informer
    {
        private static Window CreateInfoWindow(Ship ship)
        {
            var Window = new Window();
            Window.Background = (Brush)new BrushConverter().ConvertFromString("#3a3040");
            Window.MinHeight = 300;
            Window.MinWidth = 300;
            Window.Height = 300;
            Window.Width = 300;
            var border = new Border();
            border.Margin = new Thickness(35);
            border.CornerRadius = new CornerRadius(5);
            border.Background = (Brush)new BrushConverter().ConvertFromString("#262229");
            var grid = new Grid();
            grid.Margin = new Thickness(15);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            var labelType = new Label();
            labelType.Content = $"Type: {ship.ToString()}";
            labelType.VerticalAlignment = VerticalAlignment.Center;
            labelType.Foreground = Brushes.White;
            grid.Children.Add(labelType);
            Grid.SetRow(labelType, 0);
            var labelName = new Label();
            labelName.Content = $"Name: {ship.Name}";
            labelName.VerticalAlignment = VerticalAlignment.Center;
            labelName.Foreground = Brushes.White;
            grid.Children.Add(labelName);
            Grid.SetRow(labelName, 1);
            var labelSpeed = new Label();
            labelSpeed.Content = $"Speed: {ship.Speed}";
            labelSpeed.VerticalAlignment = VerticalAlignment.Center;
            labelSpeed.Foreground = Brushes.White;
            grid.Children.Add(labelSpeed);
            Grid.SetRow(labelSpeed, 2);
            var labelPos = new Label();
            labelPos.Content = $"Coordinates: {ship.CurrentPosition}";
            labelPos.VerticalAlignment = VerticalAlignment.Center;
            labelPos.Foreground = Brushes.White;
            grid.Children.Add(labelPos);
            Grid.SetRow(labelPos, 3);
            var button = new Button();
            button.Margin = new Thickness(5);
            button.Click += Button_Click;
            button.Foreground = Brushes.White;
            button.Content = "OK";
            grid.Children.Add(button);
            Grid.SetRow(button, 4);
            border.Child = grid;
            Window.Content = border;
            return Window;
        }

        private static void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow((Button)sender).Close();
        }

        public static void ShowInfo(Ship ship)
        {
            var window = CreateInfoWindow(ship);
            window.ShowDialog();
        }
    }
}
