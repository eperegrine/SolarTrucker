using System;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionManager
{
    public class Resolution
    {
        public int Width = 640;
        public int Height = 480;

        public Resolution(int width = 640, int height = 480)
        {
            Width = width;
            Height = height;
        }

        public bool isCurrentResolution()
        {
            
            return Screen.width == Width
                   && Screen.height == Height;
        }

        public override bool Equals(object ores)
        {
            if (ores == null) return false;
            Resolution res = ores as Resolution;
            if (res == null) return false;
            return res.Height == Height && res.Width == Width;
        }

        public override string ToString()
        {
            return string.Format("{0} x {1}", Width, Height);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Width, Height).GetHashCode();
        }
    }

    public static List<Resolution> getResolutions()
    {
        List<Resolution> resolutions = new List<Resolution>();
        foreach (UnityEngine.Resolution ures in Screen.resolutions)
        {
            Resolution res = new Resolution(ures.width, ures.height);
            if (!resolutions.Contains(res))
            {
                resolutions.Add(res);
            }
        }

        return resolutions;
    }
}