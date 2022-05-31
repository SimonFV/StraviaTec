using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Globalization;

namespace ApiServer
{
    public class GpxCenter
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
            lat = (listLat.Count > 0 ? listLat.Average() : 0.0).ToString();
            lon = (listLon.Count > 0 ? listLon.Average() : 0.0).ToString();
            return new List<string>() { lat, lon };
        }
    }
}