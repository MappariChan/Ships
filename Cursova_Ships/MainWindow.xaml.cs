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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Cursova_Ships.Classes;
using System.Threading;
using System.Media;

namespace Cursova_Ships
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISurface Surface { get; set; }

        private Wind Wind { get; set; }

        private List<Ship> Ships { get; set; }

        private List<Voyage> Voyages { get; set; }

        private Thread thr { get; set; }

        private Poligon Poligon { get; set; }

        private bool ShipCreationBtnState;

        private bool PoligonCreationBtnState;

        private bool ShipDeletionBtnState;

        private bool ShipSelectionBtnState;

        private bool PoligonCreated;

        private bool ShipCreated;

        private void ResetState()
        {
            if (ShipCreationBtnState)
            {
                if (ShipCreated) Container.MouseDown -= VoyageCreationEvent;
                else Container.MouseDown -= ShipCreationEvent;
            }
            if (ShipDeletionBtnState)
            {
                Container.MouseDown -= ShipDeletionEvent;
            }
            if (PoligonCreationBtnState)
            {
                if (PoligonCreated) Container.MouseDown -= AddPointToPoligon;
                else Container.MouseDown -= PoligonCreation;
            }
            if (ShipSelectionBtnState)
            {
                Container.MouseDown -= ShipSelectionEvent;
            }
            ShipCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            ShipDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            PoligonCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            ShipSelectionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            ShipCreationBtnState = false;
            ShipDeletionBtnState = false;
            PoligonCreationBtnState = false;
            ShipSelectionBtnState = false;
            ShipCreated = false;
            PoligonCreated = false;
        }

        public MainWindow()
        {
            Ships = new List<Ship>();
            Voyages = new List<Voyage>();
            Wind = new Wind();
            InitializeComponent();
            ResetState();
            /*string[] directionSources = {
                @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\North.obj",
                @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\East.obj",
                @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\South.obj",
                @"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\West.obj"
            };*/
            Surface = Factory.CreateSea(@"D:\Cursova\Cursova_Ships\Cursova_Ships\Objects\StaticOcean.obj", Viewport);
            Preparings.PrepareViewport(Camera, Surface);
            Preparings.PrepareArrow(Compas, Arrow, Wind);
            Camera.Changed += CameraChangedEvent;
            thr = new Thread(new ThreadStart(ChangeFrame));
            thr.Start();
        }

        private void ChangeFrame()
        {
            while (true)
            {
                this.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < Ships.Count(); i++)
                    {
                        Ships[i].ContinueMooving();
                        for (int j = 0; j < Ships.Count(); j++)
                        {
                            if (i != j)
                            {
                                Ships[i].PredictColisionWith(Ships[j]);
                            }
                        }
                    }
                    double angle = Wind.ChangeDirectionAndSpeed();
                    Preparings.RotateArrow(Arrow, angle);
                    for (int i = 0; i < Voyages.Count; i++)
                    {
                        if (Voyages[i].IsShipInsideOfPoligon(Poligon))
                        {
                            MessageBox.Show($"{Voyages[i].Ship.Name} entered Zone!", "My App", MessageBoxButton.OK, MessageBoxImage.Information);
                            Logger.WritrLog($"{Voyages[i].Ship.Name} entered Zone!");
                        }
                        Voyages[i].MoveShip();
                    }
                });
                Thread.Sleep(20);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Container.Height = Math.Min(this.Height - 32, this.Width/2);
            Container.Width = Math.Min(this.Height - 32, this.Width/2);
        }

        private void ShipCreationEvent(object sender, MouseButtonEventArgs e)
        {
            ShipCreated = true;
            var Position2D = e.GetPosition(Container);
            var Result = new DialogWindowResult();
            DialogWindow dialogWindow = new DialogWindow(Result);
            dialogWindow.ShowDialog();
            var Position3D = Position2D.TranslateTo3DPoint(Container, Surface);
            if (Result.Failure)
            {
                ShipCreated = false;
                return;
            }
            Ships.Add(Factory.CreateShip(Position3D, Result.Speed, Result.Name, Viewport, Wind, Result.Type));
            if(Poligon != null) Poligon.ReloadObject(Viewport);
            Container.MouseDown -= ShipCreationEvent;
            Container.MouseDown += VoyageCreationEvent;
        }

        private void VoyageCreationEvent(object sender, MouseButtonEventArgs e)
        {
            ShipCreated = false;
            var Position2D = e.GetPosition(Container);
            var Position3D = Position2D.TranslateTo3DPoint(Container, Surface);
            Voyages.Add(Factory.CreateVoyage(Ships[^1], Position3D));
            Container.MouseDown -= VoyageCreationEvent;
            Container.MouseDown += ShipCreationEvent;
        }

        private void ShipDeletionEvent(object sender, MouseButtonEventArgs e)
        {
            if (Ships.Count() == 0) return;
            var Position2D = e.GetPosition(Container);
            Ship shipToDeletion = Preparings.findClosestShip(Position2D, Ships, Container, Surface);
            Viewport.Children.Remove(shipToDeletion.ShipGeometry);
            Preparings.RemoveVoyage(Voyages, shipToDeletion);
            Ships.Remove(shipToDeletion);
            Preparings.IsAllRealyStopped(Voyages);
        }

        private void ShipCreationMode(object sender, RoutedEventArgs e)
        {
            if (!ShipCreationBtnState)
            {
                ResetState();
                Container.MouseDown += ShipCreationEvent;
                ShipCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#0f7046");
                ShipCreationBtnState = true;
            }
            else
            {
                if (ShipCreated)
                {
                    Container.MouseDown -= VoyageCreationEvent;
                }
                else
                {
                    Container.MouseDown -= ShipCreationEvent;
                }
                ShipCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
                ShipCreationBtnState = false;
                ShipCreated = false;
            }
        }

        private void PoligonCreationMode(object sender, RoutedEventArgs e)
        {
            if (!PoligonCreationBtnState)
            {
                ResetState();
                PoligonCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#0f7046");
                Container.MouseDown += PoligonCreation;
                PoligonCreationBtnState = true;
            }
            else
            {
                if (PoligonCreated)
                {
                    Container.MouseDown -= AddPointToPoligon;
                }
                else
                {
                    Container.MouseDown -= PoligonCreation;
                }
                PoligonCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
                PoligonCreationBtnState = false;
                PoligonCreated = false;
            }
        }

        private void PoligonCreation(object sender, MouseButtonEventArgs e)
        {
            if(Poligon != null)
            {
                Poligon.RemoveFromScene(Viewport);
            }
            PoligonCreated = true;
            var pos = e.GetPosition(Container);
            var vertex = Preparings.TranslateTo3DPoint(pos, Container, Surface);
            Poligon = new Poligon(Viewport);
            Poligon.AddPoint(vertex);
            Container.MouseDown -= PoligonCreation;
            Container.MouseDown += AddPointToPoligon;
        }

        private void AddPointToPoligon(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(Container);
            var vertex = Preparings.TranslateTo3DPoint(pos, Container, Surface);
            if (!Poligon.AddPoint(vertex))
            {
                MessageBox.Show("Entered vertex make poligon unconvex!", "My App", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShipDeletionMode(object sender, RoutedEventArgs e)
        {
            if (!ShipDeletionBtnState)
            {
                ResetState();
                Container.MouseDown += ShipDeletionEvent;
                ShipDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#0f7046");
                ShipDeletionBtnState = true;
            }
            else
            {
                Container.MouseDown -= ShipDeletionEvent;
                ShipDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
                ShipCreationBtnState = false;
            }
        }

        private void PoligonDeletionEvent(object sender, RoutedEventArgs e)
        {
            ResetState();
            if (Poligon == null) return;
            Poligon.RemoveFromScene(Viewport);
            Poligon = null;
        }

        private void ReturnCameraToStartPosition(object sender, RoutedEventArgs e)
        {
            Camera.Changed -= CameraChangedEvent;
            Preparings.PrepareCamera(Camera, Surface);
            ShipCreationBtn.IsEnabled = true;
            ShipDeletionBtn.IsEnabled = true;
            PoligonCreationBtn.IsEnabled = true;
            PoligonDeletionBtn.IsEnabled = true;
            ShipSelectionBtn.IsEnabled = true;
            ShipCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            ShipDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            PoligonCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            PoligonDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            ShipSelectionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
            Camera.Changed += CameraChangedEvent;
        }

        private void CameraChangedEvent(object sender, EventArgs e)
        {
            ResetState();
            ShipCreationBtn.IsEnabled = false;
            ShipDeletionBtn.IsEnabled = false;
            PoligonCreationBtn.IsEnabled = false;
            PoligonDeletionBtn.IsEnabled = false;
            ShipSelectionBtn.IsEnabled = false;
            ShipCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#243628");
            ShipDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#243628");
            PoligonCreationBtn.Background = (Brush)new BrushConverter().ConvertFromString("#243628");
            PoligonDeletionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#243628");
            ShipSelectionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#243628");
        }

        private void ShipSelectionMode(object sender, RoutedEventArgs e)
        {
            if (!ShipSelectionBtnState)
            {
                ResetState();
                Container.MouseDown += ShipSelectionEvent;
                ShipSelectionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#0f7046");
                ShipSelectionBtnState = true;
            }
            else
            {
                Container.MouseDown -= ShipSelectionEvent;
                ShipSelectionBtn.Background = (Brush)new BrushConverter().ConvertFromString("#16b570");
                ShipSelectionBtnState = false;
            }
        }

        private void ShipSelectionEvent(object sender, MouseButtonEventArgs e)
        {
            if (Ships.Count() == 0) return;
            var Position2D = e.GetPosition(Container);
            Ship selectedShip = Preparings.findClosestShip(Position2D, Ships, Container, Surface);
            Informer.ShowInfo(selectedShip);
        }
    }
}
