using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Projektni_zadatak.Bodies;

namespace Projektni_zadatak.HelpEntities
{
    public class SpatialAngle
    {
        double _horizontal;
        double _vertical;

        public SpatialAngle()
        {
            Horizontal = Vertical = 0;
        }

        public SpatialAngle(SpatialAngle sp)
        {
            Horizontal = sp.Horizontal;
            Vertical = sp.Vertical;
        }

        public SpatialAngle(double horizontal, double vertical)
        {
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public double Vertical
        {
            get { return _vertical; }
            set 
            {

                if (value > Math.PI)
                {
                    value -= 2 * Math.PI;
                }
                else if (value < -Math.PI)
                {
                    value += Math.PI * 2;
                }
                _vertical = value; 
            }
        }

        public double Horizontal
        {
            get { return _horizontal; }
            set
            {
                if (value > Math.PI)
                {
                    value -= 2 * Math.PI;
                }
                else if (value < -Math.PI)
                {
                    value += Math.PI * 2;
                }
                _horizontal = value; 
            }
        }
    }

    public class Position3D
    {
        double _dist;
        SpatialAngle _angle;

        public Position3D()
        {
            Angle = new SpatialAngle();
            Distance = 0;
        }

        public Position3D(double x, double y, double z)
        {
            _angle = new SpatialAngle(0, 0);
            Distance = Math.Sqrt(x * x + y * y + z * z);
            if (Distance > 0)
            {
                Angle.Vertical = Math.Asin(x / Distance);
                Angle.Horizontal = Math.Asin(-z / (Math.Acos(Angle.Vertical) * Distance));
            }
            else
            {
                Angle.Horizontal = Angle.Vertical = 0;
            }
        }

        public Position3D(SpatialAngle angle, double distanceFromCenter)
        {
            _angle = angle;
            Distance = distanceFromCenter;
        }

        public SpatialAngle Angle
        {
            get 
            {
                if (_angle == null)
                {
                    _angle = new SpatialAngle(0, 0);
                }
                return _angle; 
            }
            set { _angle = value; }
        }

        public double Distance
        {
            get { return _dist; }
            set 
            {
                if (value < 0)
                {
                    value = -value;
                }
                _dist = value; 
            }
        }

        public double X
        {
            get
            {
                return Math.Sin(Angle.Vertical) * Distance;
            }
        }

        public double Y
        {
            get
            {
                return Math.Sin(Angle.Horizontal) * Math.Cos(Angle.Vertical) * Distance;
            }
        }

        public double Z
        {
            get
            {
                return - Math.Cos(Angle.Horizontal) * Math.Cos(Angle.Vertical) * Distance;
            }
        }
    }

    public class Ecliptic
    {
        double _meanRadius;
        double _eccentricity;

        public Ecliptic(double meanDistance, double eccentricity)
        {
            MeanRadius = meanDistance;
            Eccentricity = eccentricity;
        }

        public double MeanRadius
        {
            get { return _meanRadius; }
            set { _meanRadius = value; }
        }

        public double Eccentricity
        {
            get { return _eccentricity; }
            set { _eccentricity = value; }
        }
    }

    public class Orbit
    {
        double _revolutionAnglePerHour;
        Ecliptic _ecliptic;
        SpatialAngle _up;
        double _startAngle;

        public double StartAngle
        {
            get { return _startAngle; }
            set
            {
                if (value > Math.PI)
                {
                    value -= Math.PI * 2;
                }
                else if (value < -Math.PI)
                {
                    value += Math.PI * 2;
                }
                _startAngle = value;
            }
        }

        public Orbit()
        {
            DaysForRevolution = 1;
            Ecliptic = new Ecliptic(0, 0);
            Up = new SpatialAngle(0, 0);
            StartAngle = 0;
        }

        public Orbit(double daysForRevolution, Ecliptic ecliptic, SpatialAngle up, double startAngle)
        {
            DaysForRevolution = daysForRevolution;
            Ecliptic = ecliptic;
            Up = up;
            StartAngle = startAngle;
        }

        public SpatialAngle Up
        {
            get
            {
                if (_up == null)
                {
                    _up = new SpatialAngle(0, 0);
                }
                return _up;
            }
            set { _up = value; }
        }

        public Ecliptic Ecliptic
        {
            get { return _ecliptic; }
            set { _ecliptic = value; }
        }

        public double DaysForRevolution
        {
            get { return 2*Math.PI/24.0/_revolutionAnglePerHour; }
            set { _revolutionAnglePerHour = 2*Math.PI/value/24.0; }
        }

        public double RevolutionAnglePerHour
        {
            get { return _revolutionAnglePerHour; }
        }
    }

    public class Rotation
    {
        double _rotationAnglePerHour;
        double _precessionPerHour;
        SpatialAngle _axisTilt;

        public Rotation(double hoursForRotation, double daysForPrecession, SpatialAngle axisTilt)
        {
            HoursForRotation = hoursForRotation;
            DaysForPrecession = daysForPrecession;
            AxisTilt = axisTilt;
        }

        public SpatialAngle AxisTilt
        {
            get { return _axisTilt; }
            set { _axisTilt = value; }
        }

        public double HoursForRotation
        {
            get { return 2 * Math.PI / _rotationAnglePerHour; }
            set { _rotationAnglePerHour = 2 * Math.PI / value; }
        }

        public double RotationAnglePerHour
        {
            get { return _rotationAnglePerHour; }
            set { _rotationAnglePerHour = value; }
        }

        public double PrecessionPerHour
        {
            get { return _precessionPerHour; }
            set { _precessionPerHour = value; }
        }

        public double DaysForPrecession
        {
            get { return 2 * Math.PI / 24.0 / _precessionPerHour; }
            set { _precessionPerHour = 2 * Math.PI / value / 24.0; }
        }

    }

    public class Eye
    {
        Position3D _position;
        Position3D _lookAt;
        SpatialAngle _up;
        Sphere _lookingAt;
        Sphere _lookingFrom;


        public Sphere LookingAt
        {
            get { return _lookingAt; }
            set { _lookingAt = value; }
        }

        public Sphere LookingFrom
        {
            get { return _lookingFrom; }
            set { _lookingFrom = value; }
        }

        public Eye()
        {
            Position = new Position3D();
            LookAt = new Position3D();
            Up = new SpatialAngle();
        }

        public Eye(Position3D position, Position3D lookAt, SpatialAngle up)
        {
            _position = position;
            _lookAt = lookAt;
            _up = up;
            LookingAt = null;
            LookingFrom = null;
        }

        public SpatialAngle Up
        {
            get { return _up; }
            set { _up = value; }
        }

        public Position3D LookAt
        {
            get { return _lookAt; }
            set 
            { 
                _lookAt = value;
                LookingAt = null;
            }
        }

        public Position3D Position
        {
            get { return _position; }
            set 
            { 
                _position = value;
                LookingFrom = null;
            }
        }
    }
}
