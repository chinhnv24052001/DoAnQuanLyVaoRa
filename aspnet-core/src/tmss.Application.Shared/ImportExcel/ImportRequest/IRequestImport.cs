using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportAssetImport.Dto;
using tmss.ImportExcel.ImportRequest.Dto;

namespace tmss.ImportExcel.ImportRequest
{
    public interface IRequestImport: ITransientDependency
    {
        Task<RequestImportDto> GetClientRequestFromExcel(byte[] fileBytes, string fileName, int typeRequest, long UserId);

    }
}
