using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using NWNUtil;
using NWServerAdminPanel.Models;

namespace NWServerAdminPanel.Controllers
{
    public class UploadController : Controller
    {
        private Models.AreaDbContext db = new Models.AreaDbContext();
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
            // Get session data that was passed via TempData
            _files = TempData["Areas"] as List<string>;
            //      Get list of processedfiles, if null, init new list
            var processedFiles = TempData["ProcessedAreas"] as List<Area> ?? new List<Area>();

            // Populate dropdown list
            var areaList = db.Areas.ToList();
            var areaDropDown = areaList.Select(item => new SelectListItem
                {
                    Text = item.Name, Value = item.ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            var selectList = new SelectList(areaDropDown, "Value", "Text");
            ViewBag.AreaList = selectList;

            // Process next file in list
            if (_files != null && _files.Count > 0)
            {
                var nextarea = _files[0];
                var area = new Area()
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
                // Remove from list so we know it's processed
                _files.RemoveAt(0);
                // Add to list of processed areas
                processedFiles.Add(area);

                // Pass Tempdata onto next view
                TempData["ProcessedAreas"] = processedFiles;
                TempData["Areas"] = _files;

                // Add count of areas and how many processed (example: Area 1 of 3)
                ViewBag.CurrentArea = processedFiles.Count;
                ViewBag.TotalAreas = processedFiles.Count + _files.Count;

                return View(area);
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddAreas(Models.Area area, string AreaList)
        {
            var areas = TempData["Areas"] as List<string>;
            var processedAreas = TempData["ProcessedAreas"] as List<Area>;

            if (ModelState.IsValid)
            {
                // If "--Add New Area--"
                if (AreaList == "")
                {
                    // Check that we can access git, gic, and are data
                    if (processedAreas != null)
                    {
                        area.Gic = processedAreas[processedAreas.Count - 1].Gic;
                        area.Git = processedAreas[processedAreas.Count - 1].Git;
                        area.Are = processedAreas[processedAreas.Count - 1].Are;
                    }
                    else
                    {
                        throw new FileNotFoundException("Could not access gic, git or are data.");
                    }

                    // Add new area
                    db.Areas.Add(area);
                    db.SaveChanges();

                    // Process next item or go to cleanup if no more items
                    if (areas.Count == 0)
                    {
                        return RedirectToAction("UploadDone");
                    }
                    return RedirectToAction("AddAreas");
                }
                // Else update pre-existing area with new gic, git, are data

                // Get area to be updated
                var updateArea = db.Areas.Find(Convert.ToInt32(AreaList));
                // Update git, gic and are data ensuring that our new values aren't null
                if (processedAreas != null)
                {
                    updateArea.Gic = processedAreas[processedAreas.Count - 1].Gic;
                    updateArea.Git = processedAreas[processedAreas.Count - 1].Git;
                    updateArea.Are = processedAreas[processedAreas.Count - 1].Are;
                    updateArea.LastModified = DateTime.Now;
                }
                // Save area back to db
                db.Entry(updateArea).State = EntityState.Modified;
                db.SaveChanges();

                // Process next item or go to cleanup if no more items
                if (areas.Count == 0)
                {
                    return RedirectToAction("UploadDone");
                }
                return RedirectToAction("AddAreas");
            }
            return View(area);
        }

        public ActionResult UploadDone()
        {
            // Get list of files in temp folder
            var deleteList = System.IO.Directory.EnumerateFiles(Server.MapPath("~/App_Data/uploads/temp"));
            // Delete files in temp folder
            foreach (var item in deleteList)
            {
                System.IO.File.Delete(item);
            }
            var areas = TempData["ProcessedAreas"] as List<Area>;
            ViewBag.Areas = areas;
            return View();
        }
    }
}
