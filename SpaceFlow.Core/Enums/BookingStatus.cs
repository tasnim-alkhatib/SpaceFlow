using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.Enums
{
    public enum BookingStatus
    {
        Pending,    
        Confirmed,  
        Cancelled,  
        Completed   
    }

    public static class BookingStatusExtensions
    {
        public static string GetBadgeClass(this BookingStatus status)
        {
            return status switch
            {
                BookingStatus.Confirmed => "bg-success",
                BookingStatus.Cancelled => "bg-danger",
                BookingStatus.Pending => "bg-warning",
                _ => "bg-secondary"
            };
        }
    }
}
