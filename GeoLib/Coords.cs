using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GeoLib
{

    public class Coords
    {
        /// <summary>
        /// Calculates Great Circle distance between two points on Earth surface (in meters)
        /// Approximately.
        /// </summary>
        /// <param name="lat1">First point latitude</param>
        /// <param name="lat2">Second point latitude</param>
        /// <param name="long1">First point longitude</param>
        /// <param name="long2">Second point longitude</param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lat2, double long1, double long2)
        {
            //https://www.kobzarev.com/programming/calculation-of-distances-between-cities-on-their-coordinates/

            long EarthRadius = 6367450; // Earth radius in meters = (6378100 equatorial + 6356800 polar) / 2 

            // convert degrees to radians
            lat1 = lat1 * Math.PI / 180; // latutude 1 (широта 1)
            lat2 = lat2 * Math.PI / 180; // latutude 2 (широта 2)

            long1 = long1 * Math.PI / 180; // longitude 1 (долгота 1)
            long2 = long2 * Math.PI / 180; // longitude 2 (долгота 2)

            // calculate latitudes sinuses and cosinuses and longitudes delta
            double clat1 = Math.Cos(lat1);
            double clat2 = Math.Cos(lat2);
            double slat1 = Math.Sin(lat1);
            double slat2 = Math.Sin(lat2);

            double delta = long2 - long1;

            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);

            // great circle count
            double y = Math.Sqrt(
                Math.Pow(clat2 * sdelta, 2f)
                +
                Math.Pow(clat1 * slat2 - slat1 * clat2 * cdelta, 2f)
                );
            double x = slat1 * slat2 + clat1 * clat2 * cdelta;

            double ad = Math.Atan2(y, x);

            double dist = ad * EarthRadius;

            return dist;

        }

        /// <summary>
        /// Calculates Great Circle distance between two points on Earth surface (in meters)
        /// Approximately.
        /// Overloads public double GetDistance(double lat1, double lat2, double long1, double long2)
        /// </summary>
        /// <param name="point1">First GeoLib.Point object</param>
        /// <param name="point2">Second GeoLib.Point object</param>
        /// <returns></returns>
        public static double GetDistance(Point point1, Point point2)
        {
            return GetDistance(point1.Lat, point2.Lat, point1.Long, point2.Long);
        }

        public static Dictionary<long, double> GetClosest(Point point, List<Point> points)
        {
            Dictionary<long, double> closest = new Dictionary<long, double>();

            foreach (Point p in points)
            {
                if(p.Id == point.Id)
                {
                    continue;
                }

                double distance = GetDistance(point, p);

                if (closest.Count < 10)
                {
                    closest.Add(p.Id, distance);
                    Debug.WriteLine($"{point.Lat}/{point.Long}\n{p.Lat}/{p.Long}");
                    Debug.WriteLine($"+ {p.Id}/{distance} <10\n");
                }
                else
                {
                    //long key = closest.FirstOrDefault(x => x.Value > distance).Key;
                    List<double> d = new List<double>();
                    d.AddRange(closest.Values);
                    d.Sort();
                    if (d[d.Count - 1] > distance)
                    {
                        long key = closest.FirstOrDefault(x => x.Value == d[d.Count - 1]).Key;
                        Debug.WriteLine($"{point.Lat}/{point.Long}\n{p.Lat}/{p.Long}");
                        Debug.WriteLine($"- {key}/{closest[key]}");
                        closest.Remove(key);
                        closest.Add(p.Id, distance);
                        //Debug.WriteLine($"{point.Lat}/{point.Long}\n{p.Lat}/{p.Long}");
                        Debug.WriteLine($"+ {p.Id}/{distance}\n");
                    }
                }

            }



            return closest;

        }

    }
}
