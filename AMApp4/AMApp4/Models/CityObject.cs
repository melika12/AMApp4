using System;
using System.Collections.Generic;
using System.Text;

namespace AMApp4.Models
{
    public class CityInfo
    {
        public int id { get; set; }
        public string wikiDataId { get; set; }
        public string type { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string region { get; set; }
        public string regionCode { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }

    public class Data
    {
        public CityInfo[] data { get; set; }
        public Metadata metadata { get; set; }

    }

    public class Metadata
    {
        public int currentOffset { get; set; }
        public int totalCount { get; set; }
    }
}
