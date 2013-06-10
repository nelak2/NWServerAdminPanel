using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NWServerAdminPanel.Models;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NWServerAdminPanel.Controllers
{
    public class ServersController : Controller
    {
        private ServerDBContext db = new ServerDBContext();
        private UdpClient query;
        //
        // GET: /Servers/

        public ActionResult Index()
        {
            string ServerName = "Offline";
            int MinLevel = 0;
            int MaxLevel = 0;
            int CurrentPlayers = 0;
            int MaxPlayers = 0;
            int AllowLocalCharacters = 0;
            int PvPMode = 0;
            int PlayerPauseEnabled = 0;
            int OnlyOneParty = 0;
            int EnforceLegalCharacters = 0;
            int ItemLevelRestrictions = 0;
            int Unknown = 0;
            string ModuleName = "Offline";

            byte[] bnes = new byte[8] { 66, 78, 69, 83, 255, 19, 0, 0 };
            byte[] bnxi = new byte[6] { 66, 78, 88, 73, 255, 19 };
            byte[] reply;
            var replyEndpoint = new IPEndPoint(IPAddress.Any, 0);

            var server = db.Servers.ToList();

            try
            {
                // Check that server is up and for server name
                query = new UdpClient(5120);
                query.Send(bnes, bnes.Length, server[0].NWNServerIP, server[0].Port);

                reply = query.Receive(ref replyEndpoint);

                ServerName = Encoding.ASCII.GetString(reply, 9, reply[8]);

                // Query extra info from server
                query.Send(bnxi, bnxi.Length, server[0].NWNServerIP, server[0].Port);
                reply = query.Receive(ref replyEndpoint);

                MinLevel = reply[8];
                MaxLevel = reply[9];
                CurrentPlayers = reply[10];
                MaxPlayers = reply[11];
                AllowLocalCharacters = reply[12];
                PvPMode = reply[13];
                PlayerPauseEnabled = reply[14];
                OnlyOneParty = reply[15];
                EnforceLegalCharacters = reply[16];
                ItemLevelRestrictions = reply[17];
                Unknown = reply[18];
                ModuleName = Encoding.ASCII.GetString(reply, 20, reply[19]);
            }
            catch
            {
                // No need to handle catch as all variables are initialized with default values
            }
            finally
            {
                query.Close();
            }

            ViewBag.serverName = ServerName;
            ViewBag.minLevel = MinLevel;
            ViewBag.maxLevel = MaxLevel;
            ViewBag.currentPlayers = CurrentPlayers;
            ViewBag.maxPlayers = MaxPlayers;
            ViewBag.allowLocalCharacters = AllowLocalCharacters;
            ViewBag.pvPMode = PvPMode;
            ViewBag.playerPauseEnabled = PlayerPauseEnabled;
            ViewBag.onlyOneParty = OnlyOneParty;
            ViewBag.enforceLegalCharacters = EnforceLegalCharacters;
            ViewBag.itemLevelRestrictions = ItemLevelRestrictions;
            ViewBag.unknown = Unknown;
            ViewBag.moduleName = ModuleName;

            return View(db.Servers.ToList());
        }

        //
        // GET: /Servers/Details/5

        public ActionResult Details(int id = 0)
        {
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        //
        // GET: /Servers/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Servers/Create

        [HttpPost]
        public ActionResult Create(Server server)
        {
            if (ModelState.IsValid)
            {
                db.Servers.Add(server);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(server);
        }

        //
        // GET: /Servers/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        //
        // POST: /Servers/Edit/5

        [HttpPost]
        public ActionResult Edit(Server server)
        {
            if (ModelState.IsValid)
            {
                db.Entry(server).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(server);
        }

        //
        // GET: /Servers/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Server server = db.Servers.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        //
        // POST: /Servers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Server server = db.Servers.Find(id);
            db.Servers.Remove(server);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}