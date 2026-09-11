using System;
using System.Collections.Generic;
using System.Text;

namespace TRG_Markets.Domain.Entities
{


    public class  Notification

    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Type { get; set; } = string.Empty;
        public string Severity { get; set; } = "Information";
        public int? UserId { get; set; }
        public bool IsArchived { get; set; }

    }
}
