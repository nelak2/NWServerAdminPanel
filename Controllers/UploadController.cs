using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NWNUtil;

namespace NWServerAdminPanel.Controllers
{
    public class UploadController : Controller
    {
        List<string> Files;

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
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), filename);
                file.SaveAs(path);

                // Extract file
                ERFExtractor extractor = new ERFExtractor(" ");
                extractor.extractMod(Path.GetFullPath(path), Server.MapPath("~/App_Data/uploads/temp/"));

                // Get area files
                Files = System.IO.Directory.EnumerateFiles(Server.MapPath("~/App_Data/uploads/temp"), "*.are").ToList();
                for (int i = 0; i < Files.Count; i++)
                {
                    Files[i] = Path.GetFileName(Files[i]);
                }
                TempData["Areas"] = Files;

                // Progress to next step
                return RedirectToAction("SelectAreas");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult SelectAreas()
        {
            Files = TempData["Areas"] as List<string>;

            foreach (var area in Files)
            {
                var NewArea = new Models.Area();
                NewArea.oldresref = Path.GetFileNameWithoutExtension(area);
                NewArea.Uploaded = DateTime.Now;
                NewArea.LastModified = DateTime.Now;
                NewArea.Name = Path.GetFileNameWithoutExtension(area);
                NewArea.Tags = "";

                NewArea.are = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + area);
                NewArea.gic = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + Path.GetFileNameWithoutExtension(area) + ".gic");
                NewArea.git = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/uploads/temp/") + Path.GetFileNameWithoutExtension(area) + ".git");

                var db = new Models.AreaDbContext();

                db.Areas.Add(NewArea);
                db.SaveChanges();
            }


            return View();
        }

    }
}
