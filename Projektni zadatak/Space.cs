using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tao.FreeGlut;
using Tao.OpenGl;
using Projektni_zadatak.HelpEntities;
using Projektni_zadatak.Bodies;

namespace Projektni_zadatak
{
    public class Space
    {
        private List<StarSystem> _systems;
        private Eye _eye;
        private double _fov;
        public SolarSim SolarSim;
        public Boolean drawCoordinates;
        public Boolean wireModel;
        public Boolean tracking;
        private int sx, sy;
        private List<BackgroundStar> stars;

        public double Fov
        {
            get { return _fov; }
            set { _fov = value; }
        }

        public Eye Eye
        {
            get { return _eye; }
            set 
            { 
                _eye = value;
            }
        }

        public void Stop()
        {
            Glut.glutHideWindow();
            Glut.glutMainLoopEvent();
        }

        public void Show()
        {
            Glut.glutShowWindow();
        }

        public void DrawSpace(List<StarSystem> systems)
        {
            _systems = systems;

            // Start the main loop.  glutMainLoop never returns.
            //Glut.glutEnterGameMode();
            //Glut.glutFullScreen();

            stars = new List<BackgroundStar>();
            Random r = new Random();
            int f = Convert.ToInt32(systems[0].Star.Planets.Last().Orbit.Ecliptic.MeanRadius);
            for (int i = 0; i < 200; i++)
            {
                Position3D p = new Position3D(new SpatialAngle(r.Next(), r.Next()), r.Next(5*f, 10*f));
                stars.Add(new BackgroundStar(Convert.ToInt32(p.X), Convert.ToInt32(p.Y), Convert.ToInt32(p.Z)));
            }
            Glut.glutMainLoop();
        }


        public Space()
        {
            initialize();
        }


        public void initialize()
        {
            // Need to double buffer for animation
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);

            // Create and position the graphics window
            Glut.glutInitWindowPosition(100, 100);
            Glut.glutInitWindowSize(800, 600);
            Glut.glutCreateWindow("Solar system");

            OpenGLInit();

            // Set up the callback function for resizing windows
            Glut.glutReshapeFunc(new Glut.ReshapeCallback(resizeFunc));

            // Callback for graphics image redrawing
            Glut.glutDisplayFunc(new Glut.DisplayCallback(displayFunc));

            // Callback for keyboard event
            Glut.glutKeyboardFunc(new Glut.KeyboardCallback(keybFunc));
        
            //Glut.glutKeyboardUpFunc(new Glut.KeyboardUpCallback(keybFunc));
            Glut.glutMouseFunc(new Glut.MouseCallback(mouseFunc));
     
            Glut.glutMotionFunc(new Glut.MotionCallback(moutionFunc));
            //Glut.glutEnterGameMode();
        }

        public void MainLoopEvent()
        {
            Glut.glutMainLoopEvent();
        }

        #region private methods

        void OpenGLInit()
        {
            Gl.glClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            //glClearColor( 1.0, 1.0, 1.0, 0.0 );
            Gl.glEnable(Gl.GL_LIGHTING); // omogucavamo izracun sijena
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glLightModeli(Gl.GL_LIGHT_MODEL_TWO_SIDE, Gl.GL_TRUE);
            Gl.glEnable(Gl.GL_DEPTH_TEST); 
            //Gl.glClearDepth(1f);          // Set the Depth buffer value (ranges[0,1])

            //Gl.glShadeModel(Gl.GL_FLAT);
            Gl.glShadeModel(Gl.GL_SMOOTH);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            //float[] global_ambient = { 0.2f, 0.2f, 0.2f, 1.0f };
            //Gl.glLightModelfv(Gl.GL_LIGHT_MODEL_AMBIENT, global_ambient);
        }

        private void SetEyePosition()
        {
            Position3D position = _eye.Position;
            Position3D lookAt = _eye.LookAt;
            Position3D vUp = new Position3D(0, 0, 0);
            vUp.Distance = 1;
            vUp.Angle = _eye.Up;
            Glu.gluLookAt(position.X, position.Y, position.Z, lookAt.X, lookAt.Y, lookAt.Z, vUp.X, vUp.Y, vUp.Z);
        }

        void resizeFunc(int w, int h)
        {
            float aspectRatio;
            h = (h == 0) ? 1 : h;
            w = (w == 0) ? 1 : w;
            Gl.glViewport(0, 0, w, h);	// View port uses whole window
 
            //Calculate aspect ratio for the viewing frustrum.
            // In general, the aspect ratio in gluPerspective should match the aspect ratio of the associated viewport.
            aspectRatio = (float)w / (float)h;  // display without distorssion

            // Set up the projection view matrix (not very well!)
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
            Glu.gluPerspective(Fov, aspectRatio, 1, 999999);
            //Gl.glOrtho(-300, 300, -400, 400, -500, 500);
            Gl.glShadeModel(Gl.GL_SMOOTH);

            // Select the Modelview matrix
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        void displayFunc()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();

            SetEyePosition();

            DrawStars();

            foreach (StarSystem ss in _systems)
            {
                DrawStar(ss.Star);
            }
 
            Gl.glFlush();
            Glut.glutSwapBuffers();
            Glut.glutPostRedisplay();
        }

        private void DrawStars()
        {

            SetMaterialForSun(new SphereColor(1,1,1), 2);

            Random r = new Random();
            foreach (BackgroundStar s in stars)
            {
                Gl.glPushMatrix();
                Gl.glTranslated(s.X, s.Y, s.Z);
                Glut.glutWireSphere(r.Next(35, 38), 30, 30);
                Gl.glPopMatrix();
            }

            ReSetMaterialForSun();
        }

        private void DrawStar(Star star)
        {
            //sačuvaj poziciju
            Gl.glPushMatrix();

            rotateSphereOrbit(star.Orbit);

            goToPosition(star.CurrentPosition);
            if (Eye.LookingAt == star)
            {
                Eye.LookAt = new Position3D(star.CurrentPosition.Angle, star.CurrentPosition.Distance);
                if (tracking)
                {
                    Eye.LookingAt = star;
                }
            }
            if (Eye.LookingFrom == star)
            {
                Eye.Position = new Position3D(star.CurrentPosition.Angle, star.CurrentPosition.Distance);
                Eye.Position.Distance += star.Radius * 1.5;
                if (tracking)
                {
                    Eye.LookingFrom = star;
                }
            }

            foreach (Planet pl in star.Planets)
            {
                DrawPlanet(pl, star);           
            }
            
            // Create light components
            float[] ambientLight = { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] diffuseLight = { 0.7f, 0.7f, 0.7f, 1.0f };
            float[] specularLight = { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] position = getFloatArrayOfCoordinates(star.CurrentPosition);

            // Assign created components to GL_LIGHT0
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, ambientLight);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, diffuseLight);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_SPECULAR, specularLight);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, position);

            //Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light);
            
            SetMaterialForSun(star.Color, star.Shinines.ToFloat());

            if (drawCoordinates)
            {
                drawCoordinateSystem(star.Radius);
            }
            
            Gl.glMaterialf(Gl.GL_FRONT, Gl.GL_SHININESS, star.Shinines.ToFloat());

            drawSphere(star);

            ReSetMaterialForSun();

            // vrati poziciju
            Gl.glPopMatrix();
        }

        private static float[] getFloatArrayOfCoordinates(Position3D position)
        {
            float[] pos = { position.X.ToFloat(), position.Y.ToFloat(), position.Z.ToFloat(), 1.0f };
            return pos;
        }

        private void DrawPlanet(Planet planet, Star star)
        {
            Gl.glPushMatrix();

            rotateSphereOrbit(planet.Orbit);

            goToCenterOfOrbit(planet.Orbit);

            goToPosition(planet.CurrentPosition);

            if (drawCoordinates)
            {
                SetMaterialForSun(planet.Color, 1);
                Gl.glRasterPos3f((planet.Radius*1.5).ToFloat(), 0, 0);
                text(planet.Name);
                ReSetMaterialForSun();
            }

            if (Eye.LookingAt == planet)
            {
                SpatialAngle planetAngle = new SpatialAngle(
                    planet.CurrentPosition.Angle.Horizontal + planet.Orbit.Up.Horizontal,
                    planet.CurrentPosition.Angle.Vertical + planet.Orbit.Up.Vertical);
                Eye.LookAt = new Position3D(planetAngle, planet.CurrentPosition.Distance);
                if (tracking)
                {
                    Eye.LookingAt = planet;
                }
            }
            if (Eye.LookingFrom == planet)
            {
                SpatialAngle planetAngle = new SpatialAngle(
                    planet.CurrentPosition.Angle.Horizontal + planet.Orbit.Up.Horizontal + star.Orbit.Up.Horizontal,
                    planet.CurrentPosition.Angle.Vertical + planet.Orbit.Up.Vertical + star.Orbit.Up.Vertical);
                Eye.Position = new Position3D(planetAngle, planet.CurrentPosition.Distance + planet.Radius * 5);
                if (tracking)
                {
                    Eye.LookingFrom = planet;
                }
            }
            
            rotatePlanetAxis(planet);

            foreach (Planet s in planet.Satelites)
            {
                DrawPlanet(s, star);
            }


            Gl.glRotated(planet.CurrentRotationAngle * 180 / Math.PI, 1, 0, 0);

            SetMaterialForPlanet(planet, star.Shinines.ToFloat());

            if (drawCoordinates)
            {
                drawCoordinateSystem(planet.Radius);
            }

            drawSphere(planet);
            if (planet.Rings != null)
            {
                SetMaterialForPlanet(planet, (star.Shinines / 10f).ToFloat());
                Gl.glRotated(90, 0, 1, 0);
                foreach (Projektni_zadatak.Bodies.Ring r in planet.Rings)
                {
                    drawDisc(r);
                }
            }
            //Glut.glutWireCone(planet.Radius, planet.Radius, 30, 30);
            ReSetMaterialForPlanet();
            Gl.glPopMatrix();
        }

        private void drawSphere(Sphere sphere)
        {
            if (wireModel)
            {
                Glut.glutWireSphere(sphere.Radius, 30, 30);
            }
            else
            {
                Glut.glutSolidSphere(sphere.Radius, 50, 50);
            }
        }

        private void drawDisc(Projektni_zadatak.Bodies.Ring r)
        {
            try
            {
                if (wireModel)
                {
                    Glut.glutWireTorus(r.OuterRadius - r.InnerRadius, r.OuterRadius, 2, 40);
                }
                else
                {
                    //Glut.glutSolidTorus(r.OuterRadius - r.InnerRadius, r.OuterRadius, 2, 40);
                    float increment = 2*(r.OuterRadius - r.InnerRadius)/r.OuterRadius;
                    for (float a = r.InnerRadius; a < r.OuterRadius; a += increment)
                    {
                        Glut.glutWireTorus(increment/6, a, 1, 80);
                    }

                }
                //Glut.glutSolidCylinder(p.Radius * 2.5, 0.5, 10, 4);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void goToCenterOfOrbit(Orbit orbit)
        {
            //goToPosition(new Position3D(new SpatialAngle(0, 0), orbit.Ecliptic.Eccentricity * orbit.Ecliptic.MeanRadius));
        }

        private void keybFunc(byte by, int a, int b)
        {
            SolarSim.keybFunc(by, a, b);
        }

        private void mouseFunc(int button, int released, int x, int y)
        {
            if (button == 2 && released == 0)
            {
                Star s = _systems[0].Star;
                if (Eye.LookAt == null)
                {
                    Eye.LookingAt = s;
                }
                else if (Eye.LookingAt is Planet)
                {
                    int i = s.Planets.IndexOf(Eye.LookingAt as Planet);
                    Eye.LookingAt = s.Planets[(i + 1) % s.Planets.Count];
                }
                else
                {
                    Eye.LookingAt = s.Planets[0];
                }
            }
        }

        private void moutionFunc(int x, int y)
        {
            if (Math.Abs(sy - y) < 100)
            {
                // rotiraj se oko sunca
                _eye.Position.Angle.Vertical -= (sy - y)/500f;
            }
            if (Math.Abs(sx - x) < 100)
            {
                // rotiraj se uz/niz sunce
                _eye.Position.Angle.Horizontal += (sx - x) / 500f;
            }
            sy = y;
            sx = x;
        }

        private static void SetMaterialForSun(SphereColor col, float shinines)
        {
            float red = col.Red, green = col.Green, blue = col.Blue;
            Gl.glColor3d(red, green, blue);
            float[] mat_sun_dif = { red, green, blue, (1.0).ToFloat() };
            float[] mat_sun_spec = { red, green, blue, (1.0).ToFloat() };
            float[] mat_sun_amb = { red, green, blue, (1.0).ToFloat() };
            float[] mat_sun_em = { red*shinines, green*shinines, blue*shinines, (1.0).ToFloat() };
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_sun_dif);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, mat_sun_spec);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, mat_sun_amb);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, mat_sun_em);
        }

        private static void ReSetMaterialForSun()
        {
            SphereColor c = new SphereColor();
            Gl.glColor3d(c.Red, c.Green, c.Blue);
            float[] mat_sun_dif = { c.Red, c.Green, c.Blue, (1.0).ToFloat() };
            float[] mat_sun_spec = { c.Red, c.Green, c.Blue, (1.0).ToFloat() };
            float[] mat_sun_amb = { c.Red, c.Green, c.Blue, (1.0).ToFloat() };
            float[] mat_sun_em = { c.Red, c.Green, c.Blue, (1.0).ToFloat() };
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_sun_dif);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, mat_sun_spec);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, mat_sun_amb);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_EMISSION, mat_sun_em);
        }

        private static void SetMaterialForPlanet(Planet planet, float sunShinines)
        {
            float reflectivity = planet.Reflectivity.ToFloat();
            SphereColor col = planet.Color;
            float br = reflectivity * sunShinines;
            float red = col.Red, green = col.Green, blue = col.Blue;
            Gl.glColor3d(red, green, blue);
            float[] mat_pla_dif = { red, green, blue, (1.0).ToFloat() };
            float[] mat_pla_amb = { red / 10f, green / 10f, blue / 10f, (1.0).ToFloat() };
            float[] mat_pla_spec = { red * br, green * br, blue * br, (1.0).ToFloat() };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, mat_pla_dif);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, mat_pla_spec);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, mat_pla_amb);
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_REFLECTION_MAP_EXT, reflectivity);
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, mat_pla_dif);
        }

        private static void ReSetMaterialForPlanet()
        {
            float reflectivity = 0;
            float[] mat_pla_res = { 0f, 0f, 0f, (1.0).ToFloat() };
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_DIFFUSE, mat_pla_res);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_SPECULAR, mat_pla_res);
            Gl.glMaterialfv(Gl.GL_FRONT_AND_BACK, Gl.GL_AMBIENT, mat_pla_res);
            Gl.glMaterialf(Gl.GL_FRONT_AND_BACK, Gl.GL_REFLECTION_MAP_EXT, reflectivity);
            //Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_EMISSION, mat_pla_dif);
        }

        private static void rotatePlanetAxis(Planet planet)
        {
            Gl.glRotated(planet.Rotation.AxisTilt.Vertical * 180 / Math.PI, 0, 1, 0);
            Gl.glRotated(planet.Rotation.AxisTilt.Horizontal * 180 / Math.PI, 1, 0, 0);
        }
        
        private static void rotateSphereOrbit(Orbit orbit)
        {
            Gl.glRotated(orbit.Up.Vertical * 180 / Math.PI, 0, 1, 0);
            Gl.glRotated(orbit.Up.Horizontal * 180 / Math.PI, 1, 0, 0);
        }
        
        private static void drawCoordinateSystem(double radius)
        {
            double coordinateLength = radius * 1.5;
            Gl.glColor3d(1.0, 1.0, 1.0);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(-coordinateLength, 0.0, 0.0);
            Gl.glVertex3d(+coordinateLength, 0.0, 0.0);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(0.0, +coordinateLength, 0.0);
            Gl.glVertex3d(0.0, -coordinateLength, 0.0);
            Gl.glEnd();
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3d(0.0, 0.0, +coordinateLength);
            Gl.glVertex3d(0.0, 0.0, -coordinateLength);
            Gl.glEnd();
        }

        private static void goToPosition(Position3D p)
        {
            Gl.glTranslated(p.X, p.Y, p.Z);
        }

        private static void text(string c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                // Render bitmap character
                Glut.glutBitmapCharacter(Glut.GLUT_BITMAP_TIMES_ROMAN_10, c[i]);
            }
        }   
      
        #endregion

        private class BackgroundStar
        {
            public int X;
            public int Y;
            public int Z;

            public BackgroundStar(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

    }
}
