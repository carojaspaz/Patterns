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
using System.Text.RegularExpressions;
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
            Console.WriteLine("Starting...");
            Console.WriteLine("-----------\n");
            Console.WriteLine("1. CPS-123456789-ERR-AUTH.nist");
            Console.WriteLine("2. CPS-123456789-NO-HIT-AUTH.nist");
            Console.WriteLine("3. CPS-123456789-OK-AUTH.nist");
            Console.WriteLine("4. CPS-ident_123456789_NO_HIT-OK.nist");
            Console.WriteLine("5. CPS-ident_123456789-OK.nist");
            Console.WriteLine("\nChoose a file to send:");
            Console.WriteLine("Select an option between (1-5):");
            ConsoleKeyInfo cki;
            do
            {
                cki = Console.ReadKey(true);
                if (!Char.IsNumber(cki.KeyChar))
                {
                    Console.WriteLine("Not valid option");
                }

            } while (cki.KeyChar < 49 || cki.KeyChar > 53);

            Console.WriteLine("Sending...");

            //Console.WriteLine($"\n{SendingFileXml(getBytesContent(cki.KeyChar, out string fileName), fileName)}");

            GetAttachment();

            Console.WriteLine("\n-----------");
            Console.WriteLine("Finish.");
            Console.ReadKey();

        }       

        /// <summary>
        /// Obtiene los bytes para enviar como adjunto
        /// </summary>
        /// <param name="codeFile"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static byte[] getBytesContent(int codeFile,out string fileName)
        {
            string rootPath = @"Nist\CPS-ident_123456789_NO_HIT-OK.nist";

            switch (codeFile)
            {
                case 49:
                    rootPath = @"Nist\CPS-123456789-ERR-AUTH.nist";
                    break;
                case 50:
                    rootPath = @"Nist\CPS-123456789-NO-HIT-AUTH.nist";
                    break;
                case 51:
                    rootPath = @"Nist\CPS-123456789-OK-AUTH.nist";
                    break;
                case 52:
                    rootPath = @"Nist\CPS-ident_123456789-OK.nist";
                    break;
            }            
            fileName = rootPath.Substring(5);
            string pathFile = $@"{Path.GetFullPath(rootPath)}";            
            return File.ReadAllBytes(pathFile);            
        }        

        /// <summary>
        /// Metodo de envio usando related parts
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static string SendingFileXml(byte[] file, string fileName)
        {
            // Xml request
            requestInfo ri = new requestInfo();
            ri.userId = "G507744";
            ri.dateTime = DateTime.Now.ToString();
            ri.requestId = $"{DateTime.Now.ToString("yyyyMMddHHmm")}";
            ri.station = "127.0.0.1";
            ri.clientId = "UT";
            List<object> par = new List<object>();
            par.Add(ri);
            List<string> ele = new List<string>();
            ele.Add("nistFile");
            Dictionary<string, string> keys = new Dictionary<string, string>();
            XDocument root = SerializeToXml(par, ele, keys, "submitNistFile");            
            byte[] rq = Encoding.UTF8.GetBytes(root.ToString());

            HttpWebRequest request = CreateWebRequest(out string boundary, true);            
            
            using (var stream = request.GetRequestStream())
            {
                var bufferSpace = Encoding.ASCII.GetBytes(Environment.NewLine);
                stream.Write(bufferSpace, 0, bufferSpace.Length);
                var bufferBoundary = Encoding.ASCII.GetBytes($"--{boundary}" + Environment.NewLine);
                stream.Write(bufferBoundary, 0, bufferBoundary.Length);
                var buffer = Encoding.ASCII.GetBytes($"Content-Type: text/xml; charset=UTF-8 {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes($"Content-Transfer-Encoding: 8bit {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes($"Content-ID: <first-part> {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                stream.Write(bufferSpace, 0, bufferSpace.Length);

                // Adjunta la petición en XML
                stream.Write(rq, 0, rq.Length);

                stream.Write(bufferSpace, 0, bufferSpace.Length);
                stream.Write(bufferBoundary, 0, bufferBoundary.Length);
                buffer = Encoding.ASCII.GetBytes($"Content-Type: application/octet-stream {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes($"Content-Transfer-Encoding: binary {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes($"Content-ID: <{fileName}> {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes($@"Content-Disposition: attachment; name=""{fileName}""  {Environment.NewLine}");
                stream.Write(buffer, 0, buffer.Length);
                stream.Write(bufferSpace, 0, bufferSpace.Length);
               
                // Adjunta el archivo en formato Hexadecimal
                var strHexa = BitConverter.ToString(file).Replace("-", "");
                stream.Write(Encoding.UTF8.GetBytes(strHexa), 0, strHexa.Length);
                stream.Write(file, 0, file.Length);

                stream.Write(bufferSpace, 0, bufferSpace.Length);
                var bufferBoundaryEnd = Encoding.ASCII.GetBytes($"--{boundary}--" + Environment.NewLine);
                stream.Write(bufferBoundaryEnd, 0, bufferBoundaryEnd.Length);                
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return ri.requestId;
        }

        static void GetAttachment()
        {            
            List<object> par = new List<object>();            
            List<string> ele = new List<string>();
            Dictionary<string, string> keys = new Dictionary<string, string>();
            keys.Add("requestId", "123456");
            XDocument root = SerializeToXml(par, ele, keys,"getNistFileResponse");
            byte[] rq = Encoding.UTF8.GetBytes(root.ToString());
            HttpWebRequest request = CreateWebRequest(out string boundary, false);
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(rq, 0, rq.Length);
            }            
            try
            {
                using (WebResponse response = request.GetResponse())
                {

                    //var boundaryResponse = GetBoundary(response.Headers.GetValues("Content-Type"));
                    //Console.WriteLine(boundaryResponse);
                    //using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    //{
                    //    while (rd.Peek() > 0)
                    //    {
                    //        string line = rd.ReadLine();
                    //        Console.WriteLine(line);
                    //    }
                    //    //string soapResult = rd.ReadToEnd();
                    //    
                    //    //Console.WriteLine(soapResult);
                    //}
                    using (Stream webStream = response.GetResponseStream())
                    {
                        if (webStream != null)
                        {
                            MultipartParser parser = new MultipartParser(webStream);
                            
                            if (parser.Success)
                            {
                                Console.WriteLine(parser.Filename);
                            }
                            //using (StreamReader responseReader = new StreamReader(webStream))
                            //{
                            //    string responseStr = responseReader.ReadToEnd();
                            //    byte[] byteResponse = Encoding.UTF8.GetBytes(responseStr);
                            //    Console.WriteLine(responseStr);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //----------------------------------------------------
        // Metodos comunes y utilitarios

        /// <summary>
        /// Devuelve un objeto XML que es la petición para el servicio soap
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="elements"></param>
        /// <param name="contentRoot"></param>
        /// <returns></returns>
        static XDocument SerializeToXml(List<object> inputs, List<string> elements, Dictionary<string,string> keys, string contentRoot)
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

            foreach(var key in keys)
            {
                contents.Add(new XElement(key.Key, key.Value));
            }

            XElement xElement = new XElement(tran + contentRoot, contents.ToArray());

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", soapenv.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "tran", tran.NamespaceName),
                new XElement(soapenv + "Header"),
                new XElement(soapenv + "Body", xElement));            
            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", null), root);                                    
            return document;
        }

        /// <summary>
        /// Create a soap webrequest to [Url]
        /// </summary>
        /// <returns></returns>
        static HttpWebRequest CreateWebRequest(out string boundary, bool sendAttachment = false)
        {
            boundary = string.Empty;
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.28.45.207:8088/TransactionMBISWeb");
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.28.46.205:8081/TransactionMBISWeb");
            webRequest.Headers.Add(@"SOAPAction: """"");            
            if (!sendAttachment)
            {
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            }
            else
            {
                boundary = $"boundary-{DateTime.Now.Ticks}";
                webRequest.ContentType = $@"multipart/related; boundary={boundary}; type=""text/xml""; start=""<first-part>""";
            }                        
            webRequest.Method = "POST";
            return webRequest;
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

        static string GetBoundary(string[] header)
        {
            string boundary = string.Empty;
            foreach (var h in header)
            {
                foreach (var ih in h.Split(';'))
                {
                    if (ih.Contains("boundary"))
                    {
                        int init = ih.IndexOf('"') + 1;
                        int count = ih.Length - (init +1);
                        return boundary = ih.Substring(init, count);
                    }
                }
            }
            return boundary;
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

    public static class LocalConstants
    {
        public const string regContentType = @"(Content-Type:)(.*)(?=[\r\n])";
        public const string regSoapenv = @"(?=<soapenv)(.*?)(?<=Envelope>)";
    }

    public class MultipartParser
    {
        private bool _successXml = false;
        private bool _successFileName = false;
        private bool _successFileContent = false;
        public MultipartParser(Stream stream)
        {
            this.Parse(stream, Encoding.UTF8);
        }

        public MultipartParser(Stream stream, Encoding encoding)
        {
            this.Parse(stream, encoding);
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            this.Success = false;

            // Read the stream into a byte array
            byte[] data = ToByteArray(stream);

            // Copy to a string for header parsing
            string content = encoding.GetString(data);
            bool readStream = true;
            while (readStream)
            {
                // The first line should contain the delimiter            
                int delimiterEndIndex = content.IndexOf("\r\n");                
                if (delimiterEndIndex > -1)
                {
                    if(delimiterEndIndex == content.Length || this.Success)
                    {
                        readStream = false;
                    }
                    string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                    content = content.Substring(delimiterEndIndex + 2);
                    
                    if (!_successXml)
                    {
                        // Look xml file                        
                        RegexOptions options = RegexOptions.Singleline;
                        Match m = Regex.Match(delimiter, LocalConstants.regSoapenv, options);
                        if (m.Success)
                        {
                            XmlDocument xmlDocument = new XmlDocument();
                            xmlDocument.LoadXml(delimiter);
                            this._successXml = true;
                        }                                                
                    }
                    else
                    {


                        // Look for filename
                        Regex re = new Regex(@"(?<=name\=\"")(.*?)(?=\"")");
                        Match filenameMatch = re.Match(content);

                        // Did we find the required values?
                        if (false)//filenameMatch.Success)
                        {
                            // Set properties
                            //this.ContentType = contentTypeMatch.Value.Trim();
                            this.Filename = filenameMatch.Value.Trim();

                            // Get the start & end indexes of the file contents
                            int startIndex = 0;// contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                            byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                            int endIndex = IndexOf(data, delimiterBytes, startIndex);

                            int contentLength = endIndex - startIndex;

                            // Extract the file contents from the byte array
                            byte[] fileData = new byte[contentLength];

                            Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);

                            this.FileContents = fileData;
                            this.Success = true;
                        }
                    }
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf<byte>(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public bool Success
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public XDocument SoapResponse { get; private set; }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }
}
