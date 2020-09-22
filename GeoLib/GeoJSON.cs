using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GeoLib
{
    public class GeoJSON
    {

        public static List<Point> LoadGeoData(string SourceFileName)
        {
            string j = File.ReadAllText(SourceFileName);

            var features = JObject.Parse(j)["features"].ToList();

            List<Point> Points = new List<Point>();

            foreach (var feature in features)
            {
                Point point = new Point();
                point.Type = feature["type"].ToString();
                //point.Id = Int64.Parse(feature["id"].ToString())+1;
                point.Name = feature["properties"]["description"].ToString();
                point.Color = feature["properties"]["marker-color" +
                    ""].ToString();
                if (point.Name.IndexOf('\n') != -1)
                {
                    point.ShortName = point.Name.Substring(0, point.Name.IndexOf('\n'));
                }
                else
                {
                    point.ShortName = point.Name;
                }

                if (point.ShortName.Length >= 30)
                {
                    point.ShortName = point.ShortName.Substring(0, 30);
                }

                point.Long = Double.Parse(
                    feature["geometry"]["coordinates"][0]
                    .ToString()
                    .Replace('.', ',')
                    );
                point.Lat = Double.Parse(
                    feature["geometry"]["coordinates"][1]
                    .ToString()
                    .Replace('.', ',')
                    );
                
                Points.Add(point);
            }



            return Points;

        }




    }
}
