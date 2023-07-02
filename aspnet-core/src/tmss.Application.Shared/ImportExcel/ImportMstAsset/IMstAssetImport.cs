using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportEmployeesLearnedSafety.Dto;
using tmss.ImportExcel.ImportMstAsset.Dto;

namespace tmss.ImportExcel.ImportMstAsset
{
    public interface IMstAssetImport : ITransientDependency
    {
        Task<List<MstAssetImportDto>> GetAssetFromExcel(byte[] fileBytes, string fileName);
    }
}
