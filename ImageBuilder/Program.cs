using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            Console.WriteLine("-----------\n");

            byte[] nistFile = GetBytes();
            Console.WriteLine($"Lenght File: {nistFile.Length}");
            byte[] img = nistFile.Skip(1031 + 170).Take(30112).ToArray();
            Console.WriteLine($"Lenght Img: {img.Length}");
            MemoryStream ms = new MemoryStream(img);
            string filePath = $@"Images\{Guid.NewGuid().ToString()}.jpg";
            Image imgFile = Image.FromStream(ms);
            imgFile.Save(filePath);

            Console.WriteLine("\n-----------");
            Console.WriteLine("Finish.");
            Console.ReadKey();
        }

        static byte[] GetBytes()
        {
            string rootPath = @"Nist\IRQ_ONE_RESULT.nist";
            string pathFile = $@"{Path.GetFullPath(rootPath)}";
            return File.ReadAllBytes(pathFile);
        }
    }
}
