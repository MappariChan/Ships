using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Cursova_Ships.Classes
{
    //Статичний клас для підготування об'єктів в 3д сцені і для роботи з цією ж сценою
    public static class Preparings
    {
        public static void PrepareViewport(PerspectiveCamera camera, ISurface surface)
        {
            Point3D sizesOfSurface = surface.FindSize();
            Vector3D newSurfacePosition = new Vector3D()
            {
                X = sizesOfSurface.X / 2,
                Y = sizesOfSurface.Z / 2,
                Z = 0
            };
            var tGroup = new Transform3DGroup();
            var tTranslate = new TranslateTransform3D(newSurfacePosition);
            tGroup.Children.Add(tTranslate);
            var newAxis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
            var tRotate = new RotateTransform3D(newAxis, (Point3D)newSurfacePosition);
            tGroup.Children.Add(tRotate);
            surface.SetTransform(tGroup);
            PrepareCamera(camera, surface);
        }

        public static void PrepareCamera(PerspectiveCamera camera, ISurface surface)
        {
            Point3D sizesOfSurface = surface.FindSize();
            Vector3D position = new Vector3D()
            {
                X = sizesOfSurface.X / 2,
                Y = sizesOfSurface.Z / 2,
                Z = 0
            };
            camera.Position = new Point3D()
            {
                X = position.X,
                Y = position.Y,
                Z = position.X / Math.Tan(camera.FieldOfView * Math.PI / 180 / 2)
            };
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.UpDirection = new Vector3D(0, 1, 0);
        }

        public static void PrepareShip(UIElement3D shipGeometry, Point3D currentPosition)
        {
            var tGroup = new Transform3DGroup();
            var tTranslate = new TranslateTransform3D((Vector3D)currentPosition);
            tGroup.Children.Add(tTranslate);
            var newAxis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90);
            var tRotate = new RotateTransform3D(newAxis, currentPosition);
            tGroup.Children.Add(tRotate);
            var tScale = new ScaleTransform3D(new Vector3D(2.5, 2.5, 2.5), currentPosition);
            tGroup.Children.Add(tScale);
            shipGeometry.Transform = tGroup;
        }

        public static void RotateShip(Ship ship, Point3D end)
        {
            var start = ship.CurrentPosition;
            var translatedPoints = new Point(end.X - start.X, end.Y - start.Y);
            double angle = 0;
            double tg;
            if (translatedPoints.X * translatedPoints.Y > 0)
            {
                tg = Math.Abs(translatedPoints.Y) / Math.Abs(translatedPoints.X);
                if (translatedPoints.X > 0) angle -= 90;
                else angle += 90;
            }
            else
            {
                tg = Math.Abs(translatedPoints.X) / Math.Abs(translatedPoints.Y);
                if (translatedPoints.X > 0) angle += 180;
            }
            angle += Math.Atan(tg) * 180 / Math.PI;
            var shipGeometry = ship.ShipGeometry;
            var tGroup = (Transform3DGroup)shipGeometry.Transform;
            var newAxis = new AxisAngleRotation3D(new Vector3D(0, 0, 1), angle);
            var tRotate = new RotateTransform3D(newAxis, start);
            tGroup.Children.Add(tRotate);
        }

        public static Point3D TranslateTo3DPoint(this Point position2D, FrameworkElement container, ISurface surface)
        {
            var surfaceSize = surface.FindSize();
            return new Point3D()
            {
                X = position2D.X / container.Width * surfaceSize.X,
                Y = (1 - position2D.Y / container.Height) * surfaceSize.Z,
                Z = 3
            };
        }

        public static void PrepareArrow(Rectangle compas, Rectangle arrow, Wind wind)
        {
            Canvas.SetLeft(arrow, (compas.Width - arrow.Width) / 2);
            Canvas.SetTop(arrow, (compas.Height - arrow.Height) / 2);
            Vector coordinateStart = new Vector(1, 0);
            var tGroup = new TransformGroup();
            var tRotation = new RotateTransform(Vector.AngleBetween(wind.Vector, coordinateStart), arrow.Width / 2, arrow.Height / 2);
            tGroup.Children.Add(tRotation);
            arrow.RenderTransform = tGroup;
        }

        public static void RotateArrow(Rectangle arrow, double angle)
        {
            var tGroup = (TransformGroup)arrow.RenderTransform;
            var tRotate = new RotateTransform(angle, arrow.Width / 2, arrow.Height / 2);
            tGroup.Children.Add(tRotate);
        }

        public static Ship findClosestShip(Point mousePos, List<Ship> ships, FrameworkElement container, ISurface surface)
        {
            Point3D translatedMousePos = TranslateTo3DPoint(mousePos, container, surface);
            Ship closest = ships[0];
            for(int i = 1; i < ships.Count(); i++)
            {
                if (distanceBetween(translatedMousePos, ships[i]) < distanceBetween(translatedMousePos, closest))
                {
                    closest = ships[i];
                }
            }
            return closest;
        }

        private static double distanceBetween(Point3D mousePos, Ship ship)
        {
            return (ship.CurrentPosition - mousePos).Length;
        }

        public static void RemoveVoyage(List<Voyage> voyages, Ship ship)
        {
            for (int i = 0; i < voyages.Count(); i++)
            {
                if (voyages[i].Ship == ship)
                {
                    voyages.Remove(voyages[i]);
                    return;
                }
            }
        }

        public static void IsAllRealyStopped(List<Voyage> voyages)
        {
            for (int i = 0; i < voyages.Count(); i++)
            {
                if (!voyages[i].IsReallyEnd())
                {
                    voyages[i].Ship.Unstope();
                }
            }
        }

        public static ModelVisual3D Clone(this ModelVisual3D model)
        {
            var poligonGeometry = new ModelVisual3D();
            var Geometry = new GeometryModel3D();
            var Square = new MeshGeometry3D()
            {
                Positions = ((MeshGeometry3D)((GeometryModel3D)model.Content).Geometry).Positions.Clone(),
                TriangleIndices = ((MeshGeometry3D)((GeometryModel3D)model.Content).Geometry).TriangleIndices.Clone()
            };
            var Material = new DiffuseMaterial
            {
                Brush = new SolidColorBrush()
                {
                    Color = Colors.HotPink,
                    Opacity = 0.35
                }
            };
            Geometry.Geometry = Square;
            Geometry.Material = Material;
            poligonGeometry.Content = Geometry;
            return poligonGeometry;
        }
    }
}
