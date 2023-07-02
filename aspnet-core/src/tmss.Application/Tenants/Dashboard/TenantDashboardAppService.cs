using Abp.Auditing;
using Abp.Authorization;
using System.Collections.Generic;
using System;
using tmss.AssetManaments;
using tmss.Authorization;
using tmss.Tenants.Dashboard.Dto;
using Abp.Domain.Repositories;
using System.Linq;

namespace tmss.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : tmssAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<AioRequestAssetInOut, long> _aioRequestAssetInOut;
        private readonly IRepository<AioRequestPeopleInOut, long> _aioRequestPeopleInOut;

        public TenantDashboardAppService(IRepository<AioRequestAssetInOut, long> aioRequestAssetInOut, IRepository<AioRequestPeopleInOut, long> aioRequestPeopleInOut)
        {
            _aioRequestAssetInOut = aioRequestAssetInOut;
            _aioRequestPeopleInOut = aioRequestPeopleInOut;
        }


        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return new GetTopStatsOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500)
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        //char get total by day
        public int countAssetByDate(DateTime date)
        {
            var countAssetInOut = _aioRequestAssetInOut.GetAll().ToList().Where(p => ((p.OutDateTime ?? DateTime.Now).Day == date.Day && (p.OutDateTime ?? DateTime.Now).Month == date.Month) && (p.OutDateTime ?? DateTime.Now).Year == date.Year
            || ( (p.InDateTime??DateTime.Now).Day == date.Day && (p.InDateTime ?? DateTime.Now).Month == date.Month && (p.InDateTime ?? DateTime.Now).Year == date.Year)).Count();
            return countAssetInOut;
        }

        public int countEmployeesByDate(DateTime date)
        {
            var countPeopleInOut = _aioRequestPeopleInOut.GetAll().ToList().Where(p => ((p.OutDateTime ?? DateTime.Now).Day == date.Day && (p.OutDateTime ?? DateTime.Now).Month == date.Month) && (p.OutDateTime ?? DateTime.Now).Year == date.Year
            || ((p.InDateTime ?? DateTime.Now).Day == date.Day && (p.InDateTime ?? DateTime.Now).Month == date.Month && (p.InDateTime ?? DateTime.Now).Year == date.Year)).Count();
            return countPeopleInOut;
        }

        //char get total by month
        public int countAssetByMonth(DateTime date)
        {
            var countAssetInOut = _aioRequestAssetInOut.GetAll().ToList().Where(p => ((p.OutDateTime ?? DateTime.Now).Month == date.Month) && (p.OutDateTime ?? DateTime.Now).Year == date.Year
            || ((p.InDateTime ?? DateTime.Now).Day == date.Day && (p.InDateTime ?? DateTime.Now).Month == date.Month && (p.InDateTime ?? DateTime.Now).Year == date.Year)).Count();
            return countAssetInOut;
        }

        public int countEmployeesByMonth(DateTime date)
        {
            var countPeopleInOut = _aioRequestPeopleInOut.GetAll().ToList().Where(p => ((p.OutDateTime ?? DateTime.Now).Month == date.Month) && (p.OutDateTime ?? DateTime.Now).Year == date.Year
            || ((p.InDateTime ?? DateTime.Now).Day == date.Day && (p.InDateTime ?? DateTime.Now).Month == date.Month && (p.InDateTime ?? DateTime.Now).Year == date.Year)).Count();
            return countPeopleInOut;
        }

        public List<SalesSummaryData> GenerateSalesSummaryData(SalesSummaryDatePeriod inputSalesSummaryDatePeriod)
        {
            List<SalesSummaryData> data = null;
            string DateFormat = "yyyy-MM-dd";

            switch (inputSalesSummaryDatePeriod)
            {
                case SalesSummaryDatePeriod.Daily:

                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddDays(-5).ToString(DateFormat), countAssetByDate(DateTime.Now.AddDays(-5)), countEmployeesByDate(DateTime.Now.AddDays(-5))),
                        new SalesSummaryData(DateTime.Now.AddDays(-4).ToString(DateFormat), countAssetByDate(DateTime.Now.AddDays(-4)), countEmployeesByDate(DateTime.Now.AddDays(-4))),
                        new SalesSummaryData(DateTime.Now.AddDays(-3).ToString(DateFormat), countAssetByDate(DateTime.Now.AddDays(-3)), countEmployeesByDate(DateTime.Now.AddDays(-3))),
                        new SalesSummaryData(DateTime.Now.AddDays(-2).ToString(DateFormat), countAssetByDate(DateTime.Now.AddDays(-2)), countEmployeesByDate(DateTime.Now.AddDays(-2))),
                        new SalesSummaryData(DateTime.Now.AddDays(-1).ToString(DateFormat), countAssetByDate(DateTime.Now.AddDays(-1)), countEmployeesByDate(DateTime.Now.AddDays(-1))),
                    };

                    break;
                case SalesSummaryDatePeriod.Weekly:
                    var lastYear = DateTime.Now.AddYears(-1).Year;
                    data = new List<SalesSummaryData>
                    {
                        //new SalesSummaryData(lastYear + " W4", Random.Next(1000, 2000),
                        //    Random.Next(100, 999)),
                        //new SalesSummaryData(lastYear + " W3", Random.Next(1000, 2000),
                        //    Random.Next(100, 999)),
                        //new SalesSummaryData(lastYear + " W2", Random.Next(1000, 2000),
                        //    Random.Next(100, 999)),
                        //new SalesSummaryData(lastYear + " W1", Random.Next(1000, 2000),
                        //    Random.Next(100, 999))
                    };

                    break;
                case SalesSummaryDatePeriod.Monthly:
                    data = new List<SalesSummaryData>
                    {
                        new SalesSummaryData(DateTime.Now.AddMonths(-4).ToString("yyyy-MM"), countAssetByMonth(DateTime.Now.AddMonths(-4)), countEmployeesByMonth(DateTime.Now.AddMonths(-4))),
                        new SalesSummaryData(DateTime.Now.AddMonths(-3).ToString("yyyy-MM"), countAssetByMonth(DateTime.Now.AddMonths(-3)), countEmployeesByMonth(DateTime.Now.AddMonths(-3))),
                        new SalesSummaryData(DateTime.Now.AddMonths(-2).ToString("yyyy-MM"), countAssetByMonth(DateTime.Now.AddMonths(-2)), countEmployeesByMonth(DateTime.Now.AddMonths(-2))),
                        new SalesSummaryData(DateTime.Now.AddMonths(-1).ToString("yyyy-MM"), countAssetByMonth(DateTime.Now.AddMonths(-1)), countEmployeesByMonth(DateTime.Now.AddMonths(-1))),
                    };

                    break;
            }

            return data;
        }

        //Employees, Aset in-out
        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = GenerateSalesSummaryData(input.SalesSummaryDatePeriod);

            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = 500,
                Growth = 1000,
                Revenue = 1500,
                TotalSales = 2000
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }
    }
}