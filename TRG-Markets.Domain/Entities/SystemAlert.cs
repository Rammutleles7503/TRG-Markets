using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{
  public class SystemAlert
    {
    public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Severity { get; set; } = "Info";
            public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
