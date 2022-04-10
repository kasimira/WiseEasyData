using Core.Models.Dashboard;

namespace Core.Contracts
{
    public interface IIndexAppService
    {
        public IndexAppViewModel GetInfo ();

        public IEnumerable<DataPoint> GetDataPoint ();

        StatisticsAppListViewModel GetInfoStatisticsExpenses ();
        StatisticsAppListViewModel GetInfoStatisticsIncomes ();
        StatisticsAppListViewModel GetInfoStatisticsSalaries ();
    }
}
