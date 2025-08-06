using System.Collections.Generic;

namespace TNKDxf.Handles
{
    public class CommandResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<Notification> Notifications { get; set; }
        public string Resultado { get; set; }
    }

}
