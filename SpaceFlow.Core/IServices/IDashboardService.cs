using SpaceFlow.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceFlow.Core.IServices
{
    public interface IDashboardService
    {
        Task<AdminDashboardDto> GetStatsAsync();
    }
}
