using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Api_Barcode_Generator.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ZXing;

namespace WebApplication1.Controllers
{

    [ApiController]
    public class BarcodeController : ControllerBase
    {


        [Obsolete]
        private readonly IHostingEnvironment hostingEnvironment;

        [Obsolete]
        public BarcodeController(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }



        [HttpPost]
        [Route("api/Barcode/BarCodeGenerator")]
        [Obsolete]
        public IActionResult BarCodeGenerator(BarcodeDto data)
        {

            BarCodeListGenerator(data);

            return Ok();


        }



        [Obsolete]
        private void BarCodeListGenerator(BarcodeDto data)
        {
            var barcodeWriterPixelData = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.CODE_128,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 80,
                    Width = 280,
                    Margin = 2,

                }
            };


            foreach (var code in data.Code)
            {

                var pixelData = barcodeWriterPixelData.Write(code);

                Bitmap bitmap = new Bitmap(pixelData.Width, pixelData.Height, PixelFormat.Format32bppRgb);

                MemoryStream memoryStream = new MemoryStream();

                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0, pixelData.Pixels.Length);

                bitmap.Save(memoryStream, ImageFormat.Png);

                byte[] byteImage = memoryStream.ToArray();

                SaveBarcode(code, byteImage);

            }
        }



        [Obsolete]
        private void SaveBarcode(string ImageName, byte[] byteImage)
        {
            string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "Images/Barcode");

            string fullimageName = ImageName + ".png";

            string filePath = Path.Combine(uploadsFolder, fullimageName);

            System.IO.File.WriteAllBytes(filePath, byteImage);
        }




    }

}
