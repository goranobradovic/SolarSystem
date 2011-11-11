using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Projektni_zadatak.HelpEntities;

namespace Projektni_zadatak.Bodies
{
    public class Sphere
    {
        double _radius;
        SphereColor _color;
        String _name;
        Position3D _currentPosition;

        public Sphere()
        {
            Radius = 0;
            Color = new SphereColor();
            Name = "";
            CurrentPosition = new Position3D(0, 0, 0);
        }

        public Sphere(double radius, SphereColor color, String name)
        {
            Radius = radius;
            Color = color;
            Name = name;
        }

        public Sphere(double radius, SphereColor color, Position3D position, String name)
        {
            Radius = radius;
            Color = color;
            CurrentPosition = position;
            Name = name;
        }

        public Sphere(Sphere sp)
        {
            this.Radius = sp.Radius;
            this.Name = sp.Name;
            this.Color = sp.Color;
            this.CurrentPosition = sp.CurrentPosition;
        }

        public Position3D CurrentPosition
        {
            get { return _currentPosition; }
            set { _currentPosition = value; }
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public SphereColor Color
        {
            get { return _color; }
            set { _color = value; }
        }

    }
}
