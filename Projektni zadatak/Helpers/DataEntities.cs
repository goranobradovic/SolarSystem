using System.Drawing;
using System.Collections.Generic;
using System;
using System.ComponentModel;


namespace Projektni_zadatak.HelpEntities
{
    [Serializable]
    [System.ComponentModel.Bindable(System.ComponentModel.BindableSupport.Yes)]
    public class GlobalData
    {
        private SystemData _systemData;

        public SystemData SystemData
        {
            get { return _systemData; }
            set { _systemData = value; }
        }
        private LookFrom _eye;

        public LookFrom Eye
        {
            get { return _eye; }
            set { _eye = value; }
        }
        private float _fov;

        public float FOV
        {
            get { return _fov; }
            set { _fov = value; }
        }
        private int _interval;

        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        private float _globalScale;

        public float GlobalScale
        {
            get { return _globalScale; }
            set { _globalScale = value; }
        }
        private float _distanceScale;

        public float DistanceScale
        {
            get { return _distanceScale; }
            set { _distanceScale = value; }
        }
        private float _starScale;

        public float StarScale
        {
            get { return _starScale; }
            set { _starScale = value; }
        }
        private float _planetScale;

        public float PlanetScale
        {
            get { return _planetScale; }
            set { _planetScale = value; }
        }
        private float _moonDistanceFix;

        public float MoonDistanceFix
        {
            get { return _moonDistanceFix; }
            set { _moonDistanceFix = value; }
        }
        private bool _drawCoordinates;

        public bool DrawCoordinates
        {
            get { return _drawCoordinates; }
            set { _drawCoordinates = value; }
        }

        private bool _wireFrames;

        public bool WireFrames
        {
            get { return _wireFrames; }
            set { _wireFrames = value; }
        }

        private bool _tracking;

        public bool Tracking
        {
            get { return _tracking; }
            set { _tracking = value; }
        }

    }

    [Serializable]
    public struct SystemData
    {
        public StarData Star;
    }


    [Serializable]
    public struct StarData
    {
        public String Name;
        public double Radius;
        public SphereColor Color;
        public double StartAngle;
        public double DistanceFromCenter;
        public double DaysForRevolution;
        public double HoursForRotation;
        public double EclipseAngleX;
        public double EclipticInclination;
        public double EclipticEccentrity;
        public double Shinines;
        public double TiltAngleX;
        public double TiltAngleZ;
        public List<PlanetData> Planets;
    }

    [Serializable]
    public struct PlanetData
    {
        public String Name;
        public double Radius;
        public SphereColor Color;
        public double StartAngle;
        public double DaysForRevolution;
        public double DistanceFromCenter;
        public double HoursForRotation;
        public double EclipticAngleX;
        public double EclipticInclination;
        public double EclipticEccentrity;
        public double Reflectivity;
        public double TiltAngleX;
        public double TiltAngleUP;
        public List<PlanetData> Satelites;
        public List<Ring> Rings;
    }

    [Serializable]
    public class SphereColor
    {
        public float Red;
        public float Green;
        public float Blue;
        public float Alpha;

        public SphereColor()
        {
            Red = Green = Blue = 0;
        }

        public SphereColor(float red, float green, float blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public SphereColor(Color color)
        {
            Red = color.R / 255f;
            Green = color.G / 255f;
            Blue = color.B / 255f;
            Alpha = color.A / 255f;
        }
    }

    public class LookFrom
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private int z;

        public int Z
        {
            get { return z; }
            set { z = value; }
        }
    }

    public class Ring
    {
        float innerRadius;

        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }
        float outerRadius;

        public float OuterRadius
        {
            get { return outerRadius; }
            set { outerRadius = value; }
        }
    }

    public class CommandBinding
    {
        public char UpFast;
        public char UpSlow;
        public char DownFast;
        public char DownSlow;
        public char ZoomInFast;
        public char ZoomInSlow;
        public char ZoomOutFast;
        public char ZoomOutSlow;
        public char LeftFast;
        public char LeftSlow;
        public char RightFast;
        public char RightSlow;
        public char Pause;
        public char Coordinates;
        public char Wireframe;
        public char Tracking;
        public char Exit;
    }

    public enum Command
    {
        UpFast,
        UpSlow,
        DownFast,
        DownSlow,
        ZoomInFast,
        ZoomInSlow,
        ZoomOutFast,
        ZoomOutSlow,
        LeftFast,
        LeftSlow,
        RightFast,
        RightSlow,
        Pause,
        Coordinates,
        Wireframe,
        Tracking,
        Exit,
        None
    }
}