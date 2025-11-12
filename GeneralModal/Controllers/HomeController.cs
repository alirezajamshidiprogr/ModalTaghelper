using GeneralModal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GeneralModal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        // اکشن برای رندر مدال داینامیک
        [HttpGet]
        public IActionResult RenderModal(
            string id = "globalModal",
            string title = "",
            string size = "md",
            string headerHtml = "",
            string bodyHtml = "",
            string footerHtml = "")
        {
            var model = new GenericModalModel
            {
                Id = id,
                Title = title,
                Size = size, // sm, md, lg, xl, xxl
                HeaderHtml = headerHtml,
                BodyHtml = bodyHtml,
                FooterHtml = footerHtml
            };

            return PartialView("_GenericModal", model);
        }

        // مدل مدال
        public class GenericModalModel
        {
            public string Id { get; set; } = "globalModal";
            public string Title { get; set; } = "";
            public string Size { get; set; } = "md"; // sm, md, lg, xl, xxl
            public string BodyHtml { get; set; } = ""; // برای محتوای بادی
            public string HeaderHtml { get; set; } = ""; // برای هدر دلخواه
            public string FooterHtml { get; set; } = ""; // برای فوتر دلخواه
            public bool EnableHeader { get; set; } = true;
            public bool EnableFooter { get; set; } = true;
        }
    }
}