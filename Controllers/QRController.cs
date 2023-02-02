using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using static QRCoder.SvgQRCode;

namespace qrGen.Controllers
{
    public class QRController : Controller
    {
        public ActionResult Index() { return View(); }

        [HttpPost]
        public IActionResult GenerateQRCode(string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            SvgQRCode qrCode = new SvgQRCode(qrCodeData);
            return Json(new
            {
                qrCode = qrCode.GetGraphic(
                20,
                System.Drawing.Color.Black,
                System.Drawing.Color.White,
                true,
                SizingMode.ViewBoxAttribute,
                null)
            });
        }

        [HttpGet]
        public IActionResult Generate(string text, string format)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            if (format == "jpeg")
            {
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImageAsBase64 = qrCode.GetGraphic(20);
                return new JsonResult(new
                { jpegImage = "data:image/jpeg;base64," + qrCodeImageAsBase64 });
            }
            if (format == "png")
            {
                Base64QRCode qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImageAsBase64 = qrCode.GetGraphic(
                    20, System.Drawing.Color.Black, System.Drawing.Color.Empty, true );
                return new JsonResult(new
                { pngImage = "data:image/png;base64," + qrCodeImageAsBase64 });
            }
            if (format == "svg")
            {
                SvgQRCode qrCode = new SvgQRCode(qrCodeData);
                return new JsonResult(new { svgImage = qrCode.GetGraphic(20) });
            }
            else
            { return BadRequest(); }
        }
    }
}