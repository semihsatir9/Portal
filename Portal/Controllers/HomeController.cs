using Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Main Page";
            HttpClient client = new HttpClient();
            var result = client.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml");
            var response = result.Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseData = response.Content.ReadAsStringAsync();
                var data = responseData.Result;

                data = data.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");
                data = data.Replace("<?xml-stylesheet type=\"text/xsl\" href=\"isokur.xsl\"?>", "");

                XmlSerializer serializer = new XmlSerializer(typeof(DovizKuruXml_Tarih_Date));
                var dovizkuruxml = new DovizKuruXml_Tarih_Date();
                using (StringReader sr = new StringReader(data))
                {
                    dovizkuruxml = (DovizKuruXml_Tarih_Date)serializer.Deserialize(sr);
                }
                // xml formatında tek tek almak
                var xml = new XmlDocument();
                xml.LoadXml(data);

                var Tarih_Date = xml.GetElementsByTagName("Tarih_Date");
                var parabirimleri = Tarih_Date[0].ChildNodes;
                for (int i = 0; i < parabirimleri.Count; i++)
                {
                    var para = parabirimleri[i];
                    if (para.Attributes.GetNamedItem("Kod").InnerText == "USD")
                    {
                        var degeri = decimal.Parse(para.SelectNodes("BanknoteBuying")[0].InnerText);
                        ViewBag.USD = degeri;
                    }
                    if (para.Attributes.GetNamedItem("Kod").InnerText == "EUR")
                    {
                        var degeri = decimal.Parse(para.SelectNodes("BanknoteBuying")[0].InnerText);
                        ViewBag.EUR = degeri;
                    }

                }
            }




            return View();
        }

       

        
    }
}