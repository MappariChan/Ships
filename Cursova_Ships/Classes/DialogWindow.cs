using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Cursova_Ships.Classes
{

    //Клас що створює діалогове вікно створення корабля
    public class DialogWindow
    {
        private Window Window { get; set; }
        private DialogWindowResult Result { get; set; }
        
        private bool typeSeted;

        //Конструктор в якому генерується діалогове вікно
        public DialogWindow(DialogWindowResult result)
        {
            typeSeted = false;
            Result = result;
            Result.Failure = true;
            Window = new Window();
            Window.Background = (Brush)new BrushConverter().ConvertFromString("#3a3040");
            Window.MinHeight = 300;
            Window.MinWidth = 300;
            Window.Height = 300;
            Window.Width = 300;
            var grid = new Grid();
            grid.Margin = new Thickness(50);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            grid.RowDefinitions.Add(new RowDefinition());
            var column = new ColumnDefinition();
            column.Width = new GridLength(1, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            column.Width = new GridLength(3, GridUnitType.Star);
            grid.ColumnDefinitions.Add(column);
            var comboBox = new ComboBox();
            comboBox.Text = "Type of ship";
            comboBox.Margin = new Thickness(5);
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            var textBlock1 = new TextBlock();
            textBlock1.Text = "Motor ship";
            comboBox.Items.Add(textBlock1);
            var textBlock2 = new TextBlock();
            textBlock2.Text = "Sailing ship";
            comboBox.Items.Add(textBlock2);
            grid.Children.Add(comboBox);
            Grid.SetColumnSpan(comboBox, 2);
            var labelSpeed = new Label();
            labelSpeed.Content = "Speed:";
            labelSpeed.HorizontalAlignment = HorizontalAlignment.Center;
            labelSpeed.VerticalAlignment = VerticalAlignment.Center;
            labelSpeed.Foreground = Brushes.White;
            grid.Children.Add(labelSpeed);
            Grid.SetRow(labelSpeed, 1);
            var textBox = new TextBox();
            textBox.Margin = new Thickness(5);
            textBox.Background = (Brush)new BrushConverter().ConvertFromString("#262229");
            textBox.Foreground = Brushes.White;
            textBox.Visibility = Visibility.Hidden;
            grid.Children.Add(textBox);
            Grid.SetRow(textBox, 1);
            Grid.SetColumn(textBox, 1);
            var labelName = new Label();
            labelName.Content = "Name:";
            labelName.HorizontalAlignment = HorizontalAlignment.Center;
            labelName.VerticalAlignment = VerticalAlignment.Center;
            labelName.Foreground = Brushes.White;
            grid.Children.Add(labelName);
            Grid.SetRow(labelName, 2);
            var textBox2 = new TextBox();
            textBox2.Margin = new Thickness(5);
            textBox2.Background = (Brush)new BrushConverter().ConvertFromString("#262229");
            textBox2.Foreground = Brushes.White;
            grid.Children.Add(textBox2);
            Grid.SetRow(textBox2, 2);
            Grid.SetColumn(textBox2, 1);
            var button = new Button();
            button.Margin = new Thickness(5);
            button.Click += Button_Click;
            button.Foreground = Brushes.White;
            button.Content = "Create ship";
            grid.Children.Add(button);
            Grid.SetRow(button, 3);
            Grid.SetColumnSpan(button, 2);
            Window.Content = grid;
        }

        //Подія зміни типу корабля, потрібна для зміни діалогово вікна створення корабля
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbox = (ComboBox)sender;
            var grid = (Grid)cbox.Parent;
            var speedTextBlock = (TextBox)grid.Children[2];
            if (((TextBlock)cbox.SelectedItem).Text != "Sailing ship")
            {
                speedTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                speedTextBlock.Visibility = Visibility.Hidden;
            }
            typeSeted = true;
        }

        //Збереження і повернення данних, що ввів користувач
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!typeSeted)
            {
                MessageBox.Show("Firstly choose type of ship!", "My App", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            string type = ((ComboBox)((Grid)Window.Content).Children[0]).Text;
            string name = ((TextBox)((Grid)Window.Content).Children[4]).Text;
            bool correctData;
            double converted = 0;
            if (type != "Sailing ship")
            {
                correctData = double.TryParse(((TextBox)((Grid)Window.Content).Children[2]).Text, out converted);
                if (correctData)
                {
                    Result.Speed = converted;
                }
                else
                {
                    MessageBox.Show("Wrong format of float number try to use ',' between int and real value!", "My App", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Result.Type = type;
            Result.Name = name;
            Result.Failure = false;
            Window.Close();
        }

        //Запуск діалогово вікна
        public void ShowDialog()
        {
            Window.ShowDialog();
        }
    }
}
