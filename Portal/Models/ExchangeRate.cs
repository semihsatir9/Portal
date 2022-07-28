using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Portal.Models
{
    [XmlRoot("Tarih_Date")]
    public class DovizKuruXml_Tarih_Date
    {
        [XmlAttribute("Tarih")]
        public string Tarih { get; set; }

        [XmlElement("Currency")]
        public List<DovizKuruXml_Currency> ParaBirimleri { get; set; }

    }


    public class DovizKuruXml_Currency
    {
        [XmlAttribute("Kod")]
        public string Kod { get; set; }


        [XmlElement("Isim")]
        public string Isim { get; set; }

        [XmlElement("BanknoteBuying")]
        public string BanknoteBuying { get; set; }
    }
}