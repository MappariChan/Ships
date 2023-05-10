using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Cursova_Ships.Classes
{
    //Клас що відповідає за опуклий многокутник, що генерує користувач
    public class Poligon
    {
        private List<Point3D> points;

        public List<Point3D> Points 
        {
            get 
            {
                return points;
            }
        }

        private ModelVisual3D poligonGeometry;

        public Poligon(HelixToolkit.Wpf.HelixViewport3D scene)
        {
            points = new List<Point3D>();
            poligonGeometry = new ModelVisual3D();
            var Geometry = new GeometryModel3D();
            var Square = new MeshGeometry3D()
            {
                Positions = new Point3DCollection(),
                TriangleIndices = new Int32Collection()
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
            scene.Children.Add(poligonGeometry);
        }

        private bool IsConvex()
        {
            for (int i = 0; i < points.Count(); i++)
            {
                int counter = 0;
                int rightAngles = 0;
                Vector vector = new Vector(points[i].X - points[(i + 1) % points.Count()].X, points[i].Y - points[(i + 1) % points.Count()].Y);
                double newX = vector.X * Math.Cos(Math.PI / 2) + vector.Y * Math.Sin(Math.PI / 2);
                double newY = -vector.X * Math.Sin(Math.PI / 2) + vector.Y * Math.Cos(Math.PI / 2);
                Vector normal = new Vector(newX, newY);
                for (int j = 0; j < points.Count(); j++)
                {
                    if (i != j)
                    {
                        Vector vector1 = new Vector(points[i].X - points[j].X, points[i].Y - points[j].Y);
                        double angle = Vector.AngleBetween(normal, vector1);
                        if (angle < 90 && angle > -90)
                        {
                            counter++;
                        }
                        else if (Math.Abs(angle) == 90)
                        {
                            rightAngles++;
                        }
                    }
                }
                if (counter + rightAngles < points.Count() - 1 && counter != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddVertexToScene(Point3D point)
        {
            ((MeshGeometry3D)((GeometryModel3D)poligonGeometry.Content).Geometry).Positions.Add(point);
            if (points.Count > 2)
            {
                ((MeshGeometry3D)((GeometryModel3D)poligonGeometry.Content).Geometry).TriangleIndices.Add(0);
                ((MeshGeometry3D)((GeometryModel3D)poligonGeometry.Content).Geometry).TriangleIndices.Add(points.Count() - 2);
                ((MeshGeometry3D)((GeometryModel3D)poligonGeometry.Content).Geometry).TriangleIndices.Add(points.Count() - 1);
            }
        }

        public bool AddPoint(Point3D point)
        {
            points.Add(point);
            bool isConvex = IsConvex();
            if (!isConvex)
            {
                points.Remove(point);
            }
            else
            {
                AddVertexToScene(point);
            }
            return isConvex;
        }

        public void RemoveFromScene(HelixToolkit.Wpf.HelixViewport3D scene)
        {
            scene.Children.Remove(poligonGeometry);   
        }

        public void ReloadObject(HelixToolkit.Wpf.HelixViewport3D scene)
        {
            scene.Children.Remove(poligonGeometry);
            poligonGeometry = poligonGeometry.Clone();
            scene.Children.Add(poligonGeometry);
        }
    }
}