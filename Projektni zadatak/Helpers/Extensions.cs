using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Projektni_zadatak.HelpEntities
{
    public static class Extensions
    {
        public static CommandBinding binding;

        public static float ToFloat(this double source)
        {
            return Convert.ToSingle(source);
        }

        public static SphereColor toSphereColor(this Color source)
        {
            return new SphereColor(source);
        }

        public static Command toCommand(this char source)
        {
            if (source == binding.Coordinates)
                return Command.Coordinates;
            else if (source == binding.DownFast)
                return Command.DownFast;
            else if (source == binding.DownSlow)
                return Command.DownSlow;
            else if (source == binding.Exit)
                return Command.Exit;
            else if (source == binding.LeftFast)
                return Command.LeftFast;
            else if (source == binding.LeftSlow)
                return Command.LeftSlow;
            else if (source == binding.Pause)
                return Command.Pause;
            else if (source == binding.RightFast)
                return Command.RightFast;
            else if (source == binding.RightSlow)
                return Command.RightSlow;
            else if (source == binding.UpFast)
                return Command.UpFast;
            else if (source == binding.UpSlow)
                return Command.UpSlow;
            else if (source == binding.ZoomInFast)
                return Command.ZoomInFast;
            else if (source == binding.ZoomInSlow)
                return Command.ZoomInSlow;
            else if (source == binding.ZoomOutFast)
                return Command.ZoomOutFast;
            else if (source == binding.ZoomOutSlow)
                return Command.ZoomOutSlow;
            else if (source == binding.Wireframe)
                return Command.Wireframe;
            else if (source == binding.Tracking)
                return Command.Tracking;

            return Command.None;
        }
    }
}
