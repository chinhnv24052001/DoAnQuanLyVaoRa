using Abp.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.ImportExcel.ImportWorkerReport.Dto;

namespace tmss.ImportExcel.ImportWorkerReport
{
    public interface IWorkerRequestImport : ITransientDependency
    {
        Task<List<WorkerRequestImportDto>> GetWorkerRequestFromExcel(byte[] fileBytes, string fileName);
    }
}
