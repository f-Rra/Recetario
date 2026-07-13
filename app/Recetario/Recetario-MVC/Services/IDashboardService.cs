using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> ObtenerResumenAsync();
}
