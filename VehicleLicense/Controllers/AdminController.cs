using Core.Db;
using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System.ComponentModel;

namespace VehicleLicense.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public static string DocumentSavePath = "PdfPrints";


        public AdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var details = _context.CarLicense.Where(i => i.Id != 0).ToList();
            return View(details);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveLicenseInfo(LicenseViewModel licensedata)
        {
            if (licensedata == null)
            {
                TempData["Message"] = "Fill the license form correctly";
                return RedirectToAction("Index");
            }
            var carLicense = new CarLicense();
            carLicense.OwnerName = licensedata.OwnerName;
            carLicense.Address = licensedata.Address;
            carLicense.RegNo = licensedata.RegNo;
            carLicense.EngineNo = licensedata.EngineNo;
            carLicense.ChassisNo = licensedata.ChassisNo;
            carLicense.VehicleMake = licensedata.VehicleMake;
            carLicense.VehicleColor = licensedata.VehicleColor;
            carLicense.VehicleModel = licensedata.VehicleModel;
            carLicense.LicenseFee = licensedata.LicenseFee;
            carLicense.TransactionDate = licensedata.TransactionDate;
            carLicense.IssuedDate = licensedata.IssuedDate;
            carLicense.ExpiryDate = licensedata.ExpiryDate;
            carLicense.Date = licensedata.Date;
            carLicense.Pin = licensedata.Pin;
            carLicense.GovtNo = licensedata.GovtNo;
            carLicense.FileNo = licensedata.FileNo;
            carLicense.SmsFee = licensedata.SmsFee;
            _context.Add(carLicense);
            _context.SaveChanges();
            TempData["Message"] = "Submit successfully";
            return RedirectToAction("Index", "Admin");
        }
        public IActionResult GetLicense()
        {
            var details = _context.CarLicense.Where(i => i.Id != 0).ToList();
            return View(details);
        }

        public IActionResult LicenseView()
        {
            var details = _context.CarLicense.ToList();
            return View(details);
        }

        public IActionResult PrintView(int licenseId)
        {
            var details = _context.CarLicense.FirstOrDefault(o => o.Id == licenseId);
            return View(details);
        }

        public IActionResult Download(int licenseId)
        {
            var baseUrl = "https://" + HttpContext.Request.Host;
            var site = $"{baseUrl}/Admin/PrintView?licenseId={licenseId}";
            var path = CreateFileServices(site, isLandScape: true);
            var url = "https://" + HttpContext.Request.Host + "/" + path;
            return Redirect(url);
        }
        public string CreateFileServices(string site, bool useLessMargin = false, bool isLandScape = false)
        {
            var folder = DocumentSavePath;
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            string pathString = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }
            var date = Guid.NewGuid();
            var path = uploadsFolder + "\\" + $"{date}.pdf";
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.MarginBottom = useLessMargin ? -1 : 20;
            converter.Options.MarginTop = useLessMargin ? 1 : 20; ;
            converter.Options.AllowContentHeightResize = true;
            converter.Options.ColorSpace = PdfColorSpace.RGB;
            if (isLandScape)
            {
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
            }

            PdfDocument doc = converter.ConvertUrl(site);
            doc.Save(path);
            doc.Close();
            var pathParts = path.Split('\\');
            var folderIndex = Array.IndexOf(pathParts, "PdfPrints");
            var extractedPath = string.Join("/", pathParts.Skip(folderIndex));
            return extractedPath;
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login", "Account");
        }
    }
}
