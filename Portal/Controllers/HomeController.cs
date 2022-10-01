using Portal.Models;
using System;
using System.Collections;
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
        public dynamic Index()
        {
            



                //this is the exchange rate part
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
            //exchange part ends

            //weather part starts
            HttpClient clientW = new HttpClient();
            var resultW = clientW.GetAsync("https://www.mgm.gov.tr/FTPDATA/analiz/sonSOA.xml");
            var responseW = resultW.Result;
            if (responseW.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseDataW = responseW.Content.ReadAsStringAsync();
                var dataW = responseDataW.Result;

                dataW = dataW.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "");


                XmlSerializer serializer = new XmlSerializer(typeof(Weather_SOA));
                var weatherxml = new Weather_SOA();
                using (StringReader sr = new StringReader(dataW))
                {
                    weatherxml = (Weather_SOA)serializer.Deserialize(sr);
                }

                var xmlW = new XmlDocument();
                xmlW.LoadXml(dataW);

                var SOA = xmlW.GetElementsByTagName("SOA");
                var cities = SOA[0].ChildNodes;
                
                var city = cities[1];
                    if (city.SelectNodes("ili")[0].InnerText == "İSTANBUL")
                    {
                        var v1 = city.SelectNodes("Durum")[0].InnerText.ToString();
                        v1 = v1.Split(',')[0];
                        ViewBag.WeatherStatus = v1;
                        var v2 = city.SelectNodes("Peryot")[0].InnerText.ToString();
                        ViewBag.WeatherPeriod = v2;
                        var v3 = city.SelectNodes("Mak")[0].InnerText.ToString();
                        ViewBag.WeatherTemperature = v3 + "°C";
                        
                    }
                
            }

            //weather part ends
                    
                    return View();

            //
        }


        [Authorize]
        public ActionResult AllUserBirthdays()
        {

            IEnumerable allusers;

            using (var db = new PortakEntities())
            {
                allusers = db.Users.ToArray();
                
            }
           

                return View(allusers);
        }

        public ActionResult ThisMonthBirthday()
        {
            DateTime datetime = DateTime.Now;
            ViewBag.Month = datetime.Month.ToString();
            ViewBag.Day = datetime.Day;
            



            using (var db = new PortakEntities())
            {
                var y = db.Users.FirstOrDefault(x => x.BirthDay.Month.ToString() == datetime.Month.ToString());
                List<User> list = new List<User>();
                

                if (y != null)
                {
                    
                    foreach (var x in db.Users)
                    {
                        if(x.BirthDay.Month.ToString() == datetime.Month.ToString())
                        {
                            list.Add(x);
                            
                        }

                    }

                    return View(list);
                }
                else
                {
                    
                    return RedirectToAction("Index");
                }


            }



            
        }

        
    }
}