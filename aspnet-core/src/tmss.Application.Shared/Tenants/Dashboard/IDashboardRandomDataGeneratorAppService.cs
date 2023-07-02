using Abp.Application.Services;
using System.Collections.Generic;
using tmss.Tenants.Dashboard.Dto;

namespace tmss.Tenants.Dashboard
{
    public interface IDashboardRandomDataGeneratorAppService: IApplicationService
    {
        List<SalesSummaryData> GenerateSalesSummaryData(SalesSummaryDatePeriod inputSalesSummaryDatePeriod);
    }
}
