using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Portal.Models
{
    [XmlRoot("SOA")]
    public class Weather_SOA
    {
        
        [XmlElement("sehirler")]
        public List<Weather_Indiv> Cities { get; set; }

    }

    public class Weather_Indiv
    {
        [XmlElement("Bolge")]
        public string Region { get; set; }

        [XmlElement("Peryot")]
        public string Period { get; set; }

        [XmlElement("ili")]
        public string City { get; set; }

        [XmlElement("Durum")]
        public string Status { get; set; }

        [XmlElement("Mak")]
        public string Temperature { get; set; }
    }

}