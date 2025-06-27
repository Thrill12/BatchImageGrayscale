using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace BatchImageGrayscale
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string originFolder;
            string destinationFolder;

            if (args.Length == 2)
            {
                originFolder = args[0];
                destinationFolder = args[1];
            }
            else
            {
                Console.WriteLine("Usage: BatchImageGrayscale <originFolder> <destinationFolder>");
                return;
            }

            if (!Directory.Exists(originFolder))
            {
                Console.WriteLine("Directory does not exist: " + originFolder);
                return;
            }

            if (!Directory.Exists(destinationFolder))
            {
                // Create directory
                Directory.CreateDirectory(destinationFolder);
            }

            DirectoryInfo info = new DirectoryInfo(originFolder);

            int totalFiles = info.GetFiles().Length;
            int doneCounter = 0;

            foreach (var file in info.EnumerateFiles())
            {
                Console.WriteLine();
                Console.Write("Converting " + file.Name + "...");

                Bitmap bitmap = new Bitmap(file.FullName);

                try
                {              
                    bitmap = ConvertFileToGrayscale(bitmap);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't parse file " + file.Name + ". Error: " + ex.Message);
                }             

                // Save the image

                string destinationFilePath = Path.Combine(destinationFolder, file.Name);
                bitmap.Save(destinationFilePath, ImageFormat.Png);

                doneCounter++;

                Console.Write(" done. " + doneCounter + "/" + totalFiles + " files processed.");
            }
        }

        private static Bitmap ConvertFileToGrayscale(Bitmap bitmap)
        {
            Bitmap grayBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            // Convert to grayscale
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);
                    // Grayscale matrix
                    int grayScale = (int)((pixelColor.R * 0.3) + (pixelColor.G * 0.59) + (pixelColor.B * 0.11));
                    Color grayColor = Color.FromArgb(pixelColor.A, grayScale, grayScale, grayScale);
                    grayBitmap.SetPixel(x, y, grayColor);
                }
            }

            bitmap = grayBitmap;
            return bitmap;
        }
    }
}
