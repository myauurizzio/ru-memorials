using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GeoLib
{
    public class Point
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public double Lat { get; set; } //широта
        public double Long { get; set; }
        public string Color { get; set; }

        //public List<PointGeometry> Geometry { get; set; }
        //public List<PointProperties> properties { get; set; }
    }
}
