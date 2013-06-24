using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NWNUtil;

namespace NWServerAdminPanel.Controllers
{
    public class UploadController : Controller
    {
        List<string> _files;

        //
        // GET: /Upload/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0 && (Path.GetExtension(file.FileName) == ".mod" || Path.GetExtension(file.FileName) == ".erf"))
            {
                // Upload file
                var filename = Path.GetFileName(file.FileName);
                if (filename != null)
                {
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                    file.SaveAs(path);

                    // Extract file
                    var extractor = new ErfExtractor();
                    extractor.ExtractMod(Path.GetFullPath(path), Server.MapPath("~/App_Data/uploads/temp/"));
                }

                // Get area files
                _files = System.IO.Directory.EnumerateFiles(Server.MapPath("~/App_Data/uploads/temp"), "*.are").ToList();
                for (var i = 0; i < _files.Count; i++)
                {
                    _files[i] = Path.GetFileNameWithoutExtension(_files[i]);
                }

                TempData["Areas"] = _files;

                // Progress to next step
                return RedirectToAction("AddAreas");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddAreas()
        {
            _files = TempData["Areas"] as List<string>;

            if (_files != null && _files.Count > 0)
            {
                var nextarea = _files[0];
                var area = new Models.Area()
                    {
                        Are = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + nextarea + ".are"),
                        Gic = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + nextarea + ".gic"),
                        Git = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + nextarea + ".git"),
                        LastModified = DateTime.Now,
                        Oldresref = nextarea,
                        Name = nextarea,
                        Tags = "",
                        Uploaded = DateTime.Now
                    };

                _files.RemoveAt(0);
                TempData["Areas"] = _files;
                return View(area);
            }
            return View();
        }

    }
}
