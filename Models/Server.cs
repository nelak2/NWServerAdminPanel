using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NWServerAdminPanel.Models
{
    public class Server
    {
        public int ID { get; set; }
        public string NWNServerIP { get; set; }
        public int Port { get; set; }
        public string SSHHostName { get; set; }
        public int SSHPort { get; set; }
        public string SSHUserName { get; set; }
        public string SSHPassword { get; set; }
    }

    public class ServerDBContext : DbContext
    {
        public DbSet<Server> Servers { get; set; }
    }
}