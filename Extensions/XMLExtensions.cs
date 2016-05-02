using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DLeh.Util.Extensions
{
    public static class XMLExtensions
    {
        public static string GetXElementValueOrNull(this XElement xel)
        {
            if (xel == null)
                return null;
            return xel.Value;
        }

        /// <param name="path">example: root/parent/child </param>
        public static XElement GetElementByPath(this XElement xel, string path)
        {
            var paths = path.Split('/');
            var workingXel = xel;
            foreach (var p in paths)
            {
                workingXel = workingXel.Element(p);
                if (workingXel == null)
                    return null;
            }
            return workingXel;
        }
        public static XElement GetElementByPath(this XDocument xmlDoc, string path)
        {
            var paths = path.Split('/');
            var workingXel = xmlDoc.Root;
            foreach (var p in paths)
            {
                if (p == xmlDoc.Root.Name)
                    continue;
                workingXel = workingXel.Element(p);
                if (workingXel == null)
                    return null;
            }
            return workingXel;
        }

        public static string GetXElementValueByPath(this XElement xel, string path)
        {
            return xel.GetElementByPath(path).GetXElementValueOrNull();
        }

        public static string GetXElementValueByPath(this XDocument xml, string path)
        {
            return xml.GetElementByPath(path).GetXElementValueOrNull();
        }
    }
}
