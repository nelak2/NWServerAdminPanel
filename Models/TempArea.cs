using System;
using System.Data.Entity;

namespace NWServerAdminPanel.Models
{
    public class TempArea
    {
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Resref { get; set; }
        public string Oldresref { get; set; }
        public byte[] Are { get; set; }
        public byte[] Gic { get; set; }
        public byte[] Git { get; set; }
        public DateTime Uploaded { get; set; }
        public DateTime LastModified { get; set; }
    }
}