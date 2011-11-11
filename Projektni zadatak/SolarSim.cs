using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projektni_zadatak.HelpEntities;
using Projektni_zadatak.Bodies;
using System.Xml.Serialization;

namespace Projektni_zadatak
{
    public partial class SolarSim : Form
    {
        private List<StarSystem> systems = new List<StarSystem>();
        //List<PlanetData> planets;
        GlobalData global;
        Space sp;
        //int counter = 0;
        //int planet = 0;

        // podešavanje centra
        Position3D home = new Position3D(0, 0, 0);
        Position3D lookAt = new Position3D(0, 0, 0);

        public SolarSim()
        {
            InitializeComponent();

            XmlSerializer xsg = new XmlSerializer(typeof(GlobalData));
            System.IO.Stream str = System.IO.File.OpenRead("system.xml");
            global = (GlobalData)xsg.Deserialize(str);
            str.Close();

            XmlSerializer xsc = new XmlSerializer(typeof(CommandBinding));
            str = System.IO.File.OpenRead("controls.xml");
            Extensions.binding = (CommandBinding)xsc.Deserialize(str);
            str.Close();

            mainTimer.Interval = global.Interval;
            mainTimer.Enabled = false;
            //eye = new Position3D(global.Eye.X, global.Eye.Y, global.Eye.Z); 
            
            nudFOV.DataBindings.Add(new Binding("value", global, "FOV"));
            nudInterval.DataBindings.Add(new Binding("value", mainTimer, "Interval"));
            nudGlobalScale.DataBindings.Add(new Binding("value", global, "GlobalScale"));
            nudDistanceScale.DataBindings.Add(new Binding("value", global, "DistanceScale"));
            nudMoonDistanceFix.DataBindings.Add(new Binding("value", global, "MoonDistanceFix"));
            nudPlanetScale.DataBindings.Add("value", global, "PlanetScale");
            nudX.DataBindings.Add("value", global.Eye, "X");
            nudY.DataBindings.Add("value", global.Eye, "Y");
            nudZ.DataBindings.Add("value", global.Eye, "Z");
            chbDrawCoordinates.DataBindings.Add(new Binding("checked", global, "DrawCoordinates"));          

            #region arhiva
            //XmlSerializer xmls = new XmlSerializer(typeof(List<PlanetData>));
            //planets = (xmls.Deserialize(System.IO.File.OpenRead("planets.xml")) as List<PlanetData>);
            //planets = new List<PlanetData>();
            /*
            // ovo je sve za testiranje jel radi

            // merkur 1,000 puta umanjen, 100,000 približen
            PlanetData planet = new PlanetData();
            planet.Color = Color.DarkGray.toSphereColor();
            planet.DaysForRevolution = 87.96;
            planet.DistanceFromCenter = 580;
            planet.EclipseAngleX = 0;
            planet.EclipseAngleZ = 0;
            planet.HoursForRotation = 15;
            planet.Name = "Merkur";
            planet.Radius = 2.44;
            planet.Reflectivity = 0.8;
            planet.StartAngle = -150;
            planet.TiltAngleX = 0;
            planet.TiltAngleZ = 0;

            planets.Add(planet);

            // venera 1,000 puta manja, 100,000 puta bliza
            planet = new PlanetData();
            planet.Color = Color.SeaShell.toSphereColor();
            planet.DaysForRevolution = 224.7;
            planet.DistanceFromCenter = 1082;
            planet.EclipseAngleX = 0;
            planet.EclipseAngleZ = 0;
            planet.HoursForRotation = 15;
            planet.Name = "Venus";
            planet.Radius = 6.05;
            planet.Reflectivity = 0.3;
            planet.StartAngle = -150;
            planet.TiltAngleX = 0;
            planet.TiltAngleZ = 0;

            planets.Add(planet);

            //Sphere veneraSp = new Sphere(6.05, Color.SeaShell.toSphereColor(), new Position3D(new SpatialAngle(Math.PI / 2d, -Math.PI / 2d), 1082), "Venus");
            //Movement veneraMov = new Movement(224.7, 15, new Path(), new SpatialAngle(0, 0));
            //Planet venera = new Planet(veneraSp, 0.3, veneraMov, new SpatialAngle(0, Math.PI));

            // zemlja 1,000 puta manja, 100,000 puta bliza
            planet = new PlanetData();
            planet.Color = Color.Aqua.toSphereColor();
            planet.DaysForRevolution = 365;
            planet.DistanceFromCenter = 1500;
            planet.EclipseAngleX = 0;
            planet.EclipseAngleZ = 0;
            planet.HoursForRotation = 24;
            planet.Name = "Earth";
            planet.Radius = 6.38;
            planet.Reflectivity = 0.9;
            planet.StartAngle = -150;
            planet.TiltAngleX = 0;
            planet.TiltAngleZ = 23;


            // zemlja 1,000 puta manja, 100,000 puta bliza
            //Sphere earthsp = new Sphere(6.38, Color.Aqua.toSphereColor(), new Position3D(new SpatialAngle(Math.PI / 2, Math.PI / 3), 1500), "Earth");
            //Movement earthMov = new Movement(365, 24, new Path(), new SpatialAngle(0, 0));
            //Planet earth = new Planet(earthsp, 1, earthMov, new SpatialAngle(-0.3, 0));
            //earth.Satelites.Add(mjesec);


            // venera 1,000 puta manja, 100,000 puta bliza
            PlanetData moon = new PlanetData();
            moon.Color = Color.SeaShell.toSphereColor();
            moon.DaysForRevolution = 1;
            moon.DistanceFromCenter = 7.6;
            moon.EclipseAngleX = 0;
            moon.EclipseAngleZ = 0;
            moon.HoursForRotation = 24;
            moon.Name = "Moon";
            moon.Radius = 1.74;
            moon.Reflectivity = 0.3;
            moon.StartAngle = -150;
            moon.TiltAngleX = 0;
            moon.TiltAngleZ = 0;

            planet.Moons = new List<PlanetData>();
            planet.Moons.Add(moon);

            planets.Add(planet);
            // mjesec 1,000 puta umanjen, 50.000 puta blizi
            //Sphere mj = new Sphere(1.74, Color.Gray.toSphereColor(), new Position3D(new SpatialAngle(Math.PI / 2, Math.PI / 2 - 2f), 7.6), "Moon");
            //Movement mjMov = new Movement(1, 24, new Path(), new SpatialAngle(0, Math.PI / 6));
            //Satelite mjesec = new Satelite(mj, 0.1, mjMov, new SpatialAngle(0, 0));

            

            // mars 1,000 puta manji, 100,000 puta blizi
            PlanetData planet = new PlanetData();
            planet.Color = Color.Red.toSphereColor();
            planet.DaysForRevolution = 686;
            planet.DistanceFromCenter = 2280;
            planet.EclipseAngleX = 0;
            planet.EclipseAngleZ = 0;
            planet.HoursForRotation = 25;
            planet.Name = "Mars";
            planet.Radius = 3.4;
            planet.Reflectivity = 0.8;
            planet.StartAngle = -150;
            planet.TiltAngleX = 0;
            planet.TiltAngleZ = 0;

            planets.Add(planet);

            //Sphere marssp = new Sphere(3.4, Color.Red.toSphereColor(), new Position3D(new SpatialAngle(Math.PI / 2, -Math.PI / 3), 2280), "Mars");
            //Movement marsMov = new Movement(686, 25, new Path(), new SpatialAngle(0, 0));
            //Planet mars = new Planet(marssp, 1, marsMov, new SpatialAngle(0, 0));


            // jupiter 1,000 puta manji, 100,000 puta blizi
            planet = new PlanetData();
            planet.Color = Color.Chocolate.toSphereColor();
            planet.DaysForRevolution = 4329;
            planet.DistanceFromCenter = 7780;
            planet.EclipseAngleX = 0;
            planet.EclipseAngleZ = 0;
            planet.HoursForRotation = 10;
            planet.Name = "Jupiter";
            planet.Radius = 71.5;
            planet.Reflectivity = 0.8;
            planet.StartAngle = -150;
            planet.TiltAngleX = 0;
            planet.TiltAngleZ = 0;

            planets.Add(planet);

            //Sphere jupitersp = new Sphere(71.5, Color.Chocolate.toSphereColor(), new Position3D(new SpatialAngle(Math.PI / 2, -Math.PI + 1f), 7780), "Jupiter");
            //Movement jupiterMov = new Movement(4329, 10, new Path(), new SpatialAngle(0, 0));
            //Planet jupiter = new Planet(jupitersp, 1, jupiterMov, new SpatialAngle(0, 0));

            // sunce 10,000 puta manje
            StarData sun = new StarData();
            sun.Color = Color.Gold.toSphereColor();
            sun.DaysForRevolution = 0;
            sun.DistanceFromCenter = 0;
            sun.EclipseAngleX = 0;
            sun.EclipseAngleZ = 0;
            sun.HoursForRotation = 0;
            sun.Name = "Sunce";
            sun.Planets = planets;
            sun.Radius = 69.6;
            sun.Shinines = 0.5;
            sun.StartAngle = 0;
            sun.TiltAngleX = 0;
            sun.TiltAngleZ = 0;

            Star sunce = CreateStarFromData(sun);
             //sunčev sistem:
            ssystem.Star = sunce;
            systems.Add(ssystem);
            */

           
            #endregion

            systems = new List<StarSystem>();
            systems.Add(CreateSystemFromData(global.SystemData));

        }

        private void initializeSpace()
        {
            if (sp == null)
            {
                sp = new Space();
            }
            else
            {
                sp.Show();
            }
            if (systems[0] != null)
            {
                systems[0] = CreateSystemFromData(global.SystemData);
            }
            sp.Fov = global.FOV;
            sp.Eye = new Eye(new Position3D(global.Eye.X, global.Eye.Y, global.Eye.Z), lookAt, new SpatialAngle(0, Math.PI / 2));
            sp.SolarSim = this;
            sp.drawCoordinates = global.DrawCoordinates;
            sp.wireModel = global.WireFrames;

            //system.Star = sun;
            //system.Eye = sp.Eye;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initializeSpace();
            mainTimer.Enabled = true;
            sp.DrawSpace(systems);
            this.SendToBack();            
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            foreach (StarSystem ss in systems)
            {
                foreach (Planet pl in ss.Star.Planets)
                {
                    CalculateCurrentPosition(pl);
                }
            }
            sp.MainLoopEvent();
        }

        private void setEyeBehindPlanet(Planet pl)
        {
            sp.Eye.Position.Angle = new SpatialAngle(pl.CurrentPosition.Angle);
            //sp.Eye.Position.Angle.Horizontal += 0.005;
            sp.Eye.Position.Distance = pl.CurrentPosition.Distance + pl.Radius*3;
        }

        private void CalculateCurrentPosition(Planet pl)
        {
            pl.CurrentPosition.Angle.Horizontal += pl.Orbit.RevolutionAnglePerHour;
            pl.CurrentRotationAngle += pl.Rotation.RotationAnglePerHour;
            double ecc = (Math.Pow(Math.Sin(pl.CurrentPosition.Angle.Horizontal)+1, 3) * pl.Orbit.Ecliptic.Eccentricity * 0.5);
            pl.CurrentPosition.Distance = pl.Orbit.Ecliptic.MeanRadius * (1 + ecc);
            foreach (Planet s in pl.Satelites)
            {
                CalculateCurrentPosition(s);
            }
        }

        private StarSystem CreateSystemFromData(SystemData data)
        {
            float globalScale = global.GlobalScale;
            float starScale = globalScale / global.StarScale;
            float distanceScale = globalScale / global.DistanceScale;
            float planetScale = globalScale / global.PlanetScale;
            float moonDistSc = global.MoonDistanceFix;

            StarSystem ssystem = new StarSystem();
            ssystem.Star = CreateStarFromData(global.SystemData.Star, distanceScale, starScale, planetScale, moonDistSc);

            return ssystem;
        }

        private Star CreateStarFromData(StarData data, float distanceSc, float StarSc, float planetSc, float moonDistSc)
        {
            Orbit orbit = new Orbit(data.DaysForRevolution, new Ecliptic(data.DistanceFromCenter,data.EclipticEccentrity), new SpatialAngle(data.EclipseAngleX, data.EclipticInclination), data.StartAngle);
            Sphere sphere = new Sphere(data.Radius / StarSc, data.Color, new Position3D(new SpatialAngle(0, data.StartAngle), data.DistanceFromCenter / distanceSc), data.Name);
            Star star = new Star(sphere, data.Shinines);
            star.Orbit = orbit;

            if (data.Planets != null)
            {
                foreach (PlanetData planet in data.Planets)
                {
                    star.Planets.Add(CreatePlanetFromData(planet, distanceSc, planetSc, moonDistSc));
                }
            }

            return star;
        }

        private Planet CreatePlanetFromData(PlanetData data, float distanceScale, float planetScale, float moonDistSc)
        {
            Sphere sphere = new Sphere(data.Radius / planetScale, data.Color, data.Name);
            Orbit orbit = new Orbit(data.DaysForRevolution, new Ecliptic(data.DistanceFromCenter/distanceScale, data.EclipticEccentrity), new SpatialAngle(data.EclipticAngleX * Math.PI / 180f, data.EclipticInclination * Math.PI / 180f), data.StartAngle*Math.PI/180f);
            Rotation rotation = new Rotation(data.HoursForRotation, 0, new SpatialAngle(data.TiltAngleX * Math.PI / 180f, data.TiltAngleUP * Math.PI / 180f));
            Planet planet = new Planet(sphere, data.Reflectivity, rotation, orbit, CreateRingsFromData(data.Rings, planetScale));

            if (data.Satelites != null)
            {
                foreach (PlanetData moon in data.Satelites)
                {
                    planet.Satelites.Add(CreatePlanetFromData(moon, distanceScale/moonDistSc, planetScale, moonDistSc));
                }
            }
            return planet;
        }

        private List<Projektni_zadatak.Bodies.Ring> CreateRingsFromData(List<Projektni_zadatak.HelpEntities.Ring> list, float RingScale)
        {
            if (list != null)
            {
                List<Projektni_zadatak.Bodies.Ring> rings = new List<Projektni_zadatak.Bodies.Ring>();
                foreach (Projektni_zadatak.HelpEntities.Ring r in list)
                {
                    rings.Add(new Projektni_zadatak.Bodies.Ring(r.InnerRadius/RingScale, r.OuterRadius/RingScale));
                }
                return rings;
            }
            else return null;
        }


        private static void saveControls()
        {
            //CommandBinding controls = new CommandBinding();
            //controls.DownFast = 'E';
            //controls.DownSlow = 'e';
            //controls.LeftFast = 'A';
            //controls.LeftSlow = 'a';
            //controls.Pause = 'p';
            //controls.RightFast = 'D';
            //controls.RightSlow = 'd';
            //controls.UpFast = 'Q';
            //controls.UpSlow = 'q';
            //controls.ZoomInFast = 'W';
            //controls.ZoomInSlow = 'w';
            //controls.ZoomOutFast = 'S';
            //controls.ZoomOutSlow = 's';
            XmlSerializer xs = new XmlSerializer(typeof(CommandBinding));
            System.IO.FileStream stream = System.IO.File.Create("controls.xml");
            xs.Serialize(stream, Extensions.binding);
            stream.Close();
        }

        public void keybFunc(byte by, int a, int b)
        {
            char c = (char) by;
            if (c >= '1' && c <= '9')
            {
                    Planet pl = FindBody(c - '1');
                    if (pl != null)
                    {
                        //sp.Eye.LookAt = pl.CurrentPosition;
                        sp.Eye.LookingAt = pl;
                    }
            }
            else if (c == '0')
            {
                //sp.Eye.LookAt = systems[0].Star.CurrentPosition;
                sp.Eye.LookingAt = systems[0].Star;
            }
            else if (c >= '!' && c <= ')')
            {
                Planet pl = FindBody(c - '!');
                if (pl != null)
                {
                    //setEyeBehindPlanet(pl);
                    sp.Eye.LookingFrom = pl;
                }
            }
            else if (c == '=')
            {
                sp.Eye.LookingFrom = systems[0].Star;
            }
            else if (c == '+')
            {
                if (Convert.ToInt32(nudInterval.Value) > 5 )
                mainTimer.Interval = Convert.ToInt32(nudInterval.Value-= 5) ;
            }
            else if (c == '-')
            {
                mainTimer.Interval = Convert.ToInt32(nudInterval.Value+= 5) ;
            }


            Command com = c.toCommand();
            processCommand(com);
        }

        private void processCommand(Command com)
        {
            switch (com)
            {
                case Command.UpFast:
                    sp.Eye.Position.Angle.Vertical += Math.PI / (sp.Eye.Position.Distance);
                    break;
                case Command.UpSlow:
                    sp.Eye.Position.Angle.Vertical += Math.PI / (sp.Eye.Position.Distance / global.DistanceScale);
                    break;
                case Command.DownFast:
                    sp.Eye.Position.Angle.Vertical -= Math.PI / (sp.Eye.Position.Distance);
                    break;
                case Command.DownSlow:
                    sp.Eye.Position.Angle.Vertical -= Math.PI / (sp.Eye.Position.Distance / global.DistanceScale);
                    break;
                case Command.ZoomInFast:
                    sp.Eye.Position.Distance *= 0.9;
                    break;
                case Command.ZoomInSlow:
                    sp.Eye.Position.Distance *= 0.99;
                    break;
                case Command.ZoomOutFast:
                    sp.Eye.Position.Distance *= 1.1;
                    break;
                case Command.ZoomOutSlow:
                    sp.Eye.Position.Distance *= 1.01;
                    break;
                case Command.LeftFast:
                    sp.Eye.Position.Angle.Horizontal -= Math.PI / (sp.Eye.Position.Distance);
                    break;
                case Command.LeftSlow:
                    sp.Eye.Position.Angle.Horizontal -= Math.PI / (sp.Eye.Position.Distance / global.DistanceScale);
                    break;
                case Command.RightFast:
                    sp.Eye.Position.Angle.Horizontal += Math.PI / (sp.Eye.Position.Distance);
                    break;
                case Command.RightSlow:
                    sp.Eye.Position.Angle.Horizontal += Math.PI / (sp.Eye.Position.Distance / global.DistanceScale);
                    break;
                case Command.Pause:
                    mainTimer.Enabled = !mainTimer.Enabled;
                    break;
                case Command.Coordinates:
                    ToggleCoordinates();
                    break;
                case Command.Wireframe:
                    ToggleWireframe();
                    break;
                case Command.Tracking:
                    ToggleTracking();
                    break;
                case Command.Exit:
                    sp.Stop();
                    this.BringToFront();
                    break;
                case Command.None:
                    break;
                default:
                    break;
            }
        }

        private void ToggleTracking()
        {
            global.Tracking = sp.tracking = !sp.tracking;
        }

        private void ToggleWireframe()
        {
            global.WireFrames = sp.wireModel = !sp.wireModel;
        }

        private void ToggleCoordinates()
        {
            chbDrawCoordinates.Checked = global.DrawCoordinates = sp.drawCoordinates = !global.DrawCoordinates;
        }

        private Planet FindBody(int p)
        {
            try
            {
                Planet pl = systems[0].Star.Planets[p];
                return pl;
            }
            catch
            {
                return null;
            }
        }

        // save current parameter value to xml
        private void SaveSettings()
        {
            XmlSerializer xs = new XmlSerializer(typeof(GlobalData));
            System.IO.FileStream stream = System.IO.File.Create("system.xml");
            xs.Serialize(stream, global);
        }

        private void SolarSim_Load(object sender, EventArgs e)
        {
            // fill form with default parameter settings

            nudFOV.Value = decimal.Parse(global.FOV.ToString());
            nudInterval.Value = decimal.Parse(global.Interval.ToString());
            nudGlobalScale.Value = decimal.Parse(global.GlobalScale.ToString());
            nudDistanceScale.Value = decimal.Parse(global.DistanceScale.ToString());
            nudMoonDistanceFix.Value = decimal.Parse(global.MoonDistanceFix.ToString());
            nudPlanetScale.Value = decimal.Parse(global.PlanetScale.ToString());
            if (global.DrawCoordinates == true) chbDrawCoordinates.Checked = true;
            else  chbDrawCoordinates.Checked = false;            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void tpBaseSettings_Enter(object sender, EventArgs e)
        {
            btnSave.Visible = true;
        }

        private void tpBaseSettings_Leave(object sender, EventArgs e)
        {
            btnSave.Visible = false;
        }
        
    }
}
