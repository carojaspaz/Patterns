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

            Console.WriteLine("--------------------------");
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
            keys.Add("requestId", "201903251133");
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
                    
                    using (Stream webStream = response.GetResponseStream())
                    {
                        if (webStream != null)
                        {
                            MultipartParser parser = new MultipartParser(webStream);

                            if (parser.SuccessWithError)
                            {
                                Console.WriteLine("Error");
                                Console.WriteLine("--------------------------");
                                XElement rootTmp = new XElement(stripNS(parser.SoapResponse.Root));
                                XDocument doc = new XDocument(rootTmp);
                                Console.WriteLine(doc.Root.Element("Body").Element("getNistFileResponseResponse").Element("responseInfo").Element("requestId"));
                                //Serializer serializer = new Serializer();
                                //var res = serializer.Deserialize<getNistFileResponseResponse>(parser.SoapResponse);
                                //Console.WriteLine("--------------------------");
                                //Console.WriteLine(res.requestInfo.dateTime);

                            }
                            else if (parser.Success)
                            {
                                Console.WriteLine("Success");
                                Console.WriteLine("--------------------------");
                                Console.WriteLine(parser.SoapResponse);
                                Console.WriteLine("--------------------------");
                                Console.WriteLine(parser.Filename);
                                Console.WriteLine("--------------------------");
                                Console.WriteLine(parser.FileContents);
                                using (var fs = new FileStream(parser.Filename, FileMode.Create, FileAccess.Write))
                                {
                                    fs.Write(parser.FileContents, 0, parser.FileContents.Length);                                    
                                }
                            }
                            else
                            {
                                Console.WriteLine("Server Error");
                                Console.WriteLine("--------------------------");
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
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://192.168.0.20:8088/TransactionMBISWeb");
            //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.28.45.207:8088/TransactionMBISWeb");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://172.28.46.205:8081/TransactionMBISWeb");
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
    }

    public class requestInfo
    {
        public string requestId { get; set; }
        public string userId { get; set; }
        public string clientId { get; set; }
        public string station { get; set; }
        public string dateTime { get; set; }
    }

    public class responseInfo
    {
        public string requestId { get; set; }
        public DateTime responseDateTime { get; set; }
        public string state { get; set; }        
    }


    public class getNistFileResponseResponse
    {
        public requestInfo requestInfo { get; set; }
    }

    public static class LocalConstants
    {
        public const string regBoundary = @"(?=--)(.*)";
        public const string regSoapenv = @"(?=<soap:Envelope)(.*?)(?<=soap:Envelope>)";        
        public const string regFileName = @"(?<=Content-ID: <)(.*)(?=>)";
        public const string regInitNist = @"(?=1\.0?1)(.*)";
    }

    public class MultipartParser
    {
        private bool _successXml = false;
        private bool _successFileName = false;
        private bool _successFileContent = false;
        private bool _successBoundary = false;
        private string _boundary = string.Empty;
        public MultipartParser(Stream stream)
        {
            this.Parse(stream, Encoding.ASCII);
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
            int counterAttachment = 0;
            int lastCounterAttachment = 0;
            while (readStream)
            {
                // The first line should contain the delimiter            
                int delimiterEndIndex = content.IndexOf("\r\n");                
                if (delimiterEndIndex > -1)
                {                    
                    string delimiter = content.Substring(0, content.IndexOf("\r\n"));
                    lastCounterAttachment = counterAttachment;
                    counterAttachment += delimiter.Length;
                    content = content.Substring(delimiterEndIndex + 2);
                    if (content.Length == 0)
                    {
                        readStream = false;
                    }
                    if (!_successBoundary)
                    {
                        // Look xml file                        
                        RegexOptions options = RegexOptions.Multiline;
                        Match m = Regex.Match(delimiter, LocalConstants.regBoundary, options);
                        if (m.Success)
                        {
                            _boundary = m.Value;
                            this._successBoundary = true;
                        }
                    }
                    else
                    {
                        if (!_successXml)
                        {
                            // Look xml file                        
                            RegexOptions options = RegexOptions.Singleline;
                            Match m = Regex.Match(delimiter, LocalConstants.regSoapenv, options);
                            if (m.Success)
                            {                                
                                TextReader tr = new StringReader(delimiter);
                                SoapResponse = XDocument.Load(tr);                                
                                this._successXml = true;
                                SuccessWithError = true;
                            }
                        }
                        else
                        {
                            if (!_successFileName)
                            {
                                RegexOptions options = RegexOptions.Multiline;
                                Match m = Regex.Match(delimiter, LocalConstants.regFileName, options);
                                if (m.Success)
                                {
                                    Filename = m.Value;
                                    this._successFileName = true;
                                    SuccessWithError = true;
                                }
                            }
                            else
                            {
                                if (!_successFileContent)
                                {
                                    RegexOptions options = RegexOptions.Multiline;
                                    Match m = Regex.Match(delimiter, LocalConstants.regInitNist, options);
                                    if (m.Success)
                                    {                                        
                                        int startIndex = lastCounterAttachment + Filename.Length + 5;                                        

                                        // Extract the file contents from the byte array
                                        int lengthFile = data.Length - (startIndex + _boundary.Length + 4);
                                        byte[] fileData = new byte[lengthFile];

                                        Buffer.BlockCopy(data, startIndex, fileData, 0, lengthFile);
                                        
                                        this.FileContents = fileData;
                                        _successFileContent = true;
                                        SuccessWithError = false;
                                    }
                                }
                            }
                        }
                    }                    
                }
                else
                {
                    readStream = false;
                }
                Success = _successXml && _successFileName && _successFileContent;
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
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
           
            }
        }

        public bool Success
        {
            get;
            private set;
        }       

        public bool SuccessWithError
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

    public class Serializer
    {
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(input))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
    }
}
