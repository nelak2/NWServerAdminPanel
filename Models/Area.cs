using System;
using System.Data.Entity;

namespace NWServerAdminPanel.Models
{
    public class Area
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string resref { get; set; }
        public string oldresref { get; set; }
        public byte[] are { get; set; }
        public byte[] gic { get; set; }
        public byte[] git { get; set; }
        public DateTime Uploaded { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class AreaDbContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
    }
}