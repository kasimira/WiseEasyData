using Infrastructure.Data.Common;
using WiseEasyData.Infrastructure.Data;

namespace Infrastructure.Data.Repositories
{
    public class ApplicatioDbRepository : Repository, IApplicatioDbRepository
    {
        public ApplicatioDbRepository(ApplicationDbContext context)
        {
            Context = context;
        }
    }
}
