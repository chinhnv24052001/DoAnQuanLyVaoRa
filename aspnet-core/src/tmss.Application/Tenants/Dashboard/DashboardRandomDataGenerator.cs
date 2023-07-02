using Abp.Domain.Repositories;
using NPOI.SS.Formula.Functions;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using tmss.AssetManaments;
using tmss.Tenants.Dashboard.Dto;

namespace tmss.Tenants.Dashboard
{
    public class DashboardRandomDataGenerator 
    {
        private const string DateFormat = "yyyy-MM-dd";
        private static readonly Random Random;
        public static string[] CountryNames = { "Argentina", "China", "France", "Italy", "Japan", "Netherlands", "Russia", "Spain", "Turkey", "United States"};

        static DashboardRandomDataGenerator()
        {
            Random = new Random();
        }

        public static int GetRandomInt(int min, int max)
        {
            return Random.Next(min, max);
        }

        public static int[] GetRandomArray(int size, int min, int max)
        {
            var array = new int[size];
            for (var i = 0; i < size; i++)
            {
                array[i] = GetRandomInt(min, max);
            }

            return array;
        }

        public static int[] GetRandomPercentageArray(int size)
        {
            if (size == 1)
            {
                return new int[100];
            }

            var array = new int[size];
            var total = 0;
            for (var i = 0; i < size - 1; i++)
            {
                array[i] = GetRandomInt(0, 100 - total);
                total += array[i];
            }

            array[size - 1] = 100 - total;

            return array;
        }


        public static List<MemberActivity> GenerateMemberActivities()
        {
            return new List<MemberActivity>
            {
                new MemberActivity("Brain", tmssConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Jane", tmssConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Tim", tmssConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%"),

                new MemberActivity("Kate", tmssConsts.CurrencySign + GetRandomInt(100, 500), GetRandomInt(10, 100), GetRandomInt(10, 150),
                    GetRandomInt(10, 99) + "%")
            };
        }

        public static List<RegionalStatCountry> GenerateRegionalStat()
        {
            var stats = new List<RegionalStatCountry>();
            for (var i = 0; i < 4; i++)
            {
                var countryIndex = GetRandomInt(0, CountryNames.Length);
                stats.Add(new RegionalStatCountry
                {
                    CountryName = CountryNames[countryIndex],
                    AveragePrice = GetRandomInt(10, 100),
                    Sales = GetRandomInt(10000, 100000),
                    TotalPrice = GetRandomInt(10000, 50000),
                    Change = new List<int>
                    {
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20),
                        GetRandomInt(-20, 20)
                    }
                });
            }

            return stats;
        }
    }
}