using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportAssetImport.Dto;

namespace tmss.ImportExcel.ImportAssetImport
{
    public interface IAssetRequestImport: ITransientDependency
    {
        Task<List<AssetRequestImportDto>> GetAssetRequestFromExcel(byte[] fileBytes, string fileName, string check);
        Task deleteFile(string folder);
    }
}
