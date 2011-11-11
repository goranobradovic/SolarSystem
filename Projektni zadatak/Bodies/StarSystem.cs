using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Projektni_zadatak.HelpEntities;

namespace Projektni_zadatak.Bodies
{
    public class StarSystem
    {
        Position3D _position;
        SpatialAngle _angle;
        Star _star;

        public Star Star
        {
            get { return _star; }
            set { _star = value; }
        }

        public SpatialAngle Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public Position3D Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
