using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Mockservice
{
    class Program
    {
        static void Main(string[] args)
        {
            //Envelope soapenvelope = new Envelope();
            //soapenvelope.Header = new Header();
            //soapenvelope.Body = new Body();
            //string pathFile = $@"c:\tmp\{Guid.NewGuid().ToString()}.xml";
            //XmlDocument doc = SerializeToXmlDocument(soapenvelope);
            //doc.Save(pathFile);


            //Serialization<parameter>.DeserializeFromXmlFile(pathFile);
            requestInfo ri = new requestInfo();
            ri.userId = "G507744";
            ri.dateTime = DateTime.Now.ToString();
            ri.requestId = "100000012";
            ri.station = "127.0.0.1";
            ri.clientId = "UT";
            List<object> par = new List<object>();
            par.Add(ri);
            List<string> ele = new List<string>();
            //ele.Add("nistFile");
            //submitFile02(SerializeToXml(par, ele, "submitNistFile"));
            submitFile01();   
            
        }

        static void GetAttachment(XElement root)
        {
            HttpWebRequest request = CreateWebRequest(false);
            XmlDocument soapEnvelope = new XmlDocument();
            using (var xmlReader = root.CreateReader())
            {
                soapEnvelope.Load(xmlReader);
            };
            soapEnvelope.LoadXml(root.ToString());            
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelope.Save(stream);                
            }
            //RESPONSE
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream webStream = response.GetResponseStream())
                    {
                        if (webStream != null)
                        {
                            using (StreamReader responseReader = new StreamReader(webStream))
                            {
                                string responseStr = responseReader.ReadToEnd();
                                byte[] byteResponse = Encoding.UTF8.GetBytes(responseStr);                               
                            }
                        }
                    }
                }                                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    

        static void submitFile01()
        {
            AfisIdemia.RequestInfo ri = new AfisIdemia.RequestInfo();
            ri.clientId = "G507744";
            ri.dateTime = DateTime.Now.ToString();
            ri.requestId = "100000012";
            ri.station = "127.0.0.1";
            ri.userId = "UT";
            AfisIdemia.submitNistFileRequest rq = new AfisIdemia.submitNistFileRequest();
            rq.requestInfo = ri;
            rq.nistFile = getBytes();
            AfisIdemia.TransactionClient client = new AfisIdemia.TransactionClient();
            string fileName = Guid.NewGuid().ToString();
            Console.WriteLine("Invoke submitnistfile...");
            try
            {
                using (new OperationContextScope(client.InnerChannel))
                {
                    using (var content = new MultipartFormDataContent("-----" + fileName))
                    {
                        content.Add(new StreamContent(new MemoryStream(getBytes())), $"fs-{fileName}", $"fs-{fileName}.nist");
                        
                        var rs = client.submitNistFile(rq);
                        Console.WriteLine("Service completed:");
                        Console.WriteLine($"Response: {rs.state}");
                    }
                    
                }                                
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        static void submitFile02(XElement root)
        {            
            HttpWebRequest request = CreateWebRequest(false);            
            XmlDocument soapEnvelope = new XmlDocument();
            using (var xmlReader = root.CreateReader())
            {
                soapEnvelope.Load(xmlReader);
            };
            
            soapEnvelope.LoadXml(root.ToString());
            byte[] boundaryBytes = getBytes();
            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelope.Save(stream);                
                stream.Write(boundaryBytes, 0, boundaryBytes.Length);
            }
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();
                        Console.WriteLine(soapResult);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
                

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        static HttpWebRequest CreateWebRequest(bool file)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.28.46.205:8081/TransactionMBISWeb/");             
            webRequest.Headers.Add(@"SOAP:Action");
            if (file) { webRequest.ContentType = "text/xml;charset=\"utf-8\""; } else { webRequest.ContentType = "application/octet-stream"; }
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        static byte[] getBytes()
        {
            string pathFile = @"c:\nist\cps\CPS-123456789-ERR-AUTH.nist";
            return File.ReadAllBytes(pathFile);
        }

        static XmlDocument SerializeToXmlDocument(object input)
        {
            XmlSerializer ser = new XmlSerializer(input.GetType(),"soapenv");

            XmlDocument xd = null;            

            using (MemoryStream memStm = new MemoryStream())
            {
                ser.Serialize(memStm, input);

                memStm.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                using (var xtr = XmlReader.Create(memStm, settings))
                {
                    xd = new XmlDocument();                                        
                    xd.DocumentElement.SetAttribute("xmlns:soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                    xd.DocumentElement.SetAttribute("xmlns:tran", "http://transaction.ws.adapter.edi.bodega.morphotrak.com/");
                    xd.Load(xtr);
                }
            }

            return xd;
        }

        static XElement SerializeToXml()
        {
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace tran = "http://transaction.ws.adapter.edi.bodega.morphotrak.com/";


            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "tran", tran.NamespaceName),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body",
                    new XElement(tran + "getNistFileResponse",
                        new XElement("requestId", 10))));
            //new XElement(tran + "submitNistFile", contents.ToArray())));
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            Console.WriteLine(document);
            document.Save("tes.xml");
            return root;
        }

        static XElement SerializeToXml(List<object> inputs, List<string> elements, string contentRoot)
        {            
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace tran = "http://transaction.ws.adapter.edi.bodega.morphotrak.com/";

            List<XElement> contents = new List<XElement>();
            foreach (var input in inputs)
            {
                XmlSerializer serializer = new XmlSerializer(input.GetType());
                var doc = new XDocument();                
                using (XmlWriter writer = doc.CreateWriter())
                {                    
                    serializer.Serialize(writer, input);                    
                }
                contents.Add(stripNS(doc.Root));
            }
            foreach (var ele in elements)
            {
                contents.Add(new XElement(ele));
            }

            XElement xElement = new XElement(tran + contentRoot, contents.ToArray());

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "tran", tran.NamespaceName),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body", xElement));
                    //new XElement(tran + "submitNistFile", contents.ToArray())));
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", null), root);
            Console.WriteLine(document);
            document.Save("tes.xml");
            return root;
        }

        static XElement stripNS(XElement root)
        {
            return new XElement(
                root.Name.LocalName,
                root.HasElements ?
                    root.Elements().Select(el => stripNS(el)) :
                    (object)root.Value
            );
        }
    }

    public class requestInfo
    {
        public string requestId { get; set; }
        public string userId { get; set; }
        public string clientId { get; set; }
        public string station { get; set; }
        public string dateTime { get; set; }
    }

    public class Envelope
    {
        public Header Header { get; set; }
        public Body Body { get; set; }
    }
    public class Header { }
    public class Body
    {

    }
    public static class Serialization<T> where T : class
    {

        public static T DeserializeFromXmlFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            DataContractSerializer deserializer = new DataContractSerializer(typeof(T));

            using (Stream stream = File.OpenRead(fileName))
            {
                return (T)deserializer.ReadObject(stream);
            }
        }
    }

    [Serializable]
    public class parameter
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string label { get; set; }

        [DataMember]
        public string unit { get; set; }

        [DataMember]
        public component thisComponent { get; set; }

        public parameter() { }
    }

    [Serializable]
    public class component
    {
        [DataMember]
        public string type { get; set; }

        //[DataMember]
        //public List<attribute> attributes { get; set; }
    }

    [Serializable]
    public class attribute
    {
        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string displayed { get; set; }

        [DataMember]
        public string add_remove { get; set; }

        [DataMember]
        public string ccypair { get; set; }

        [DataMember]
        public List<int> item { get; set; }
    }
}
