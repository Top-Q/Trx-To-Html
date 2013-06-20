using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using EyeOpen.Imaging.Processing;
using ImageCompare;

namespace ImageCompareTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ScreenCapture sc = new ScreenCapture();
            var im = sc.CaptureScreen();
            im.Save("test.png");
            
            ComparableImage imag1 = new ComparableImage(new FileInfo(@"C:\Users\liel_r\Downloads\Runtime - 10022007\img1.png"));
            ComparableImage imag2 = new ComparableImage(new FileInfo(@"C:\Users\liel_r\Downloads\Runtime - 10022007\img2.png"));
            ComparableImage imag3 = new ComparableImage(new FileInfo(@"C:\Users\liel_r\Downloads\Runtime - 10022007\img3.png"));

            
            var sim=   imag1.CalculateSimilarity(imag2);
            Console.WriteLine("imag1 sim to imag2 by -"+ sim*100 +"%");

            var sim2 = imag1.CalculateSimilarity(imag3);
            Console.WriteLine("imag1 sim to imag3 by -" + sim2 * 100 + "%");

        }
    }
}
