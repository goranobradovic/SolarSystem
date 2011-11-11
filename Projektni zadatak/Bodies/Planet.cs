using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projektni_zadatak.HelpEntities;
using System.Drawing;

namespace Projektni_zadatak.Bodies
{
    public class Planet : Sphere
    {
        Orbit _orbit;
        List<Planet> _satelites;
        double _currentRotationAngle;
        double _reflectivity;
        Rotation _rotation;
        public List<Ring> Rings;

        public Planet():base()
        {
            Reflectivity = 0;
            Orbit = new Orbit();
            Satelites = new List<Planet>();
            CurrentRotationAngle = 0;
        }

        public Planet(Sphere sp, double reflectivity, Rotation rotation, Orbit orbit, List<Ring> rings):base(sp)
        {
            Reflectivity = reflectivity;
            Orbit = orbit;
            Satelites = new List<Planet>();
            CurrentRotationAngle = 0;
            Rotation = rotation;
            double ecc = (Math.Pow(Math.Sin(Orbit.StartAngle) + 1, 3) * Orbit.Ecliptic.Eccentricity * 0.5);
            double dist = Orbit.Ecliptic.MeanRadius * (1 + ecc);
            CurrentPosition = new Position3D(new SpatialAngle(Orbit.StartAngle, 0), dist);
            Rings = rings;
        }

        public Rotation Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public double CurrentRotationAngle
        {
            get { return _currentRotationAngle; }
            set 
            {
                if (value > Math.PI)
                {
                    value -= 2 * Math.PI;
                }
                else if (value < -Math.PI)
                {
                    value += 2 * Math.PI;
                }
                _currentRotationAngle = value;
            }
        }


        public double Reflectivity
        {
            get { return _reflectivity; }
            set { _reflectivity = value; }
        }


        public List<Planet> Satelites
        {
            get { return _satelites; }
            set { _satelites = value; }
        }

        public Orbit Orbit
        {
            get { return _orbit; }
            set { _orbit = value; }
        }
    }

    public class Ring
    {
        public float InnerRadius;
        public float OuterRadius;

        public Ring(float innerRadius, float outerRadius)
        {
            InnerRadius = innerRadius;
            OuterRadius = outerRadius;
        }
    }
}
