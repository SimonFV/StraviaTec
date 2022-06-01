using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace ApiServer
{
    public static class GpxCenter
    {
        public static List<string> getCenter(string gpxFile)
        {
            using var reader = XmlReader.Create(gpxFile);
            reader.MoveToContent();
            string lat = "";
            string lon = "";
            List<double> listLat = new();
            List<double> listLon = new();

            while (reader.Read())
            {
                string result = reader.NodeType switch
                {
                    XmlNodeType.Element when reader.Name == "trkpt" ||
                     reader.Name == "wpt" => $"{reader.GetAttribute("lat")};{reader.GetAttribute("lon")}\n",
                    _ => ""
                };
                if (result != "")
                {
                    var format = new NumberFormatInfo();
                    format.NegativeSign = "-";
                    format.NumberDecimalSeparator = ".";
                    string[] values = result.Split(';');
                    listLat.Add(Double.Parse(values[0], format));
                    listLon.Add(Double.Parse(values[1], format));
                }
            }
            lat = (listLat.Count > 0 ? Math.Round((Decimal)(listLat.Average()), 6) : 0.0M).ToString().Replace(',', '.'); ;
            lon = (listLon.Count > 0 ? Math.Round((Decimal)(listLon.Average()), 6) : 0.0M).ToString().Replace(',', '.'); ;
            return new List<string>() { lon, lat };
        }
    }
}