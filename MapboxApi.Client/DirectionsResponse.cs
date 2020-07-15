using System;
using System.Collections.Generic;

namespace MapboxApi.Client
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Leg
    {
        public string Summary { get; set; }
        public List<object> Steps { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public double Weight { get; set; }
    }

    public class Geometry
    {
        public List<List<double>> Coordinates { get; set; }
        public string Type { get; set; }
    }

    public class Route
    {
        public string Weight_name { get; set; }
        public List<Leg> Legs { get; set; }
        public Geometry Geometry { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
        public double Weight { get; set; }
    }

    public class Waypoint
    {
        public double Distance { get; set; }
        public string Name { get; set; }
        public List<double> Location { get; set; }
    }

    public class DirectionsResponse
    {
        public List<Route> Routes { get; set; }
        public List<Waypoint> Waypoints { get; set; }
        public string Code { get; set; }
        public string Uuid { get; set; }
    }
}