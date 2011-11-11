using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projektni_zadatak.HelpEntities;
using System.Drawing;

namespace Projektni_zadatak.Bodies
{
    public class Star : Sphere
    {
        public Star()
        {
            StartPosition = new Position3D(0, 0, 0);
            Shinines = 0;
            Planets = new List<Planet>();
        }
        public Star(Sphere sp, double shinines):base(sp)
        {
            Shinines = shinines;
            Planets = new List<Planet>();
        }

        Position3D _startPosition;

        public Position3D StartPosition
        {
            get { return _startPosition; }
            set { _startPosition = value; }
        }

        Orbit _orbit;

        public Orbit Orbit
        {
            get { return _orbit; }
            set { _orbit = value; }
        }

        List<Planet> _planets;

        public List<Planet> Planets
        {
            get { return _planets; }
            set { _planets = value; }
        }

        double _shinines;

        public double Shinines
        {
            get { return _shinines; }
            set { _shinines = value; }
        }

    }
}
