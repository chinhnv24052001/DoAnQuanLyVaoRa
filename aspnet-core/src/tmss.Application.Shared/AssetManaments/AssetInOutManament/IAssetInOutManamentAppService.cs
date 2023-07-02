using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments.AssetInOutManament.Dto;
using System.Drawing;
using System.Drawing;

namespace tmss.AssetManaments.AssetInOutManament
{
    public interface IAssetInOutManamentAppService: IApplicationService
    {
        Task<PagedResultDto<AssetInOutSelectOutputDto>> LoadAllAssetInOut(RequestAssetInOutInputDto input);
        Task<PagedResultDto<EmployeesInOutSelectOutputDto>> LoadAllEmployeesInOut(RequestAssetInOutInputDto input);
        Task <List<AssetInOutSelectOutputDto>> LoadAllAssetsInOutToCheck(RequestAssetInOutInputDto input);
        Task <List<EmployeesInOutSelectOutputDto>> LoadAllEmployeesInOutCheck(RequestAssetInOutInputDto input);
        Task<PagedResultDto<RequestAssetInOutSelectOutputDto>> LoadAllRequestAssetInOut(RequestAssetInOutInputDto input);
        Task<long> CheckInOutAsset(long input, long noteId, string stringInOut);
        Task<long> CheckInOutEmployees(long input, long noteId, string stringInOut);
        Task CheckInOutRequest(long input, bool IsRequestAssetIn, string stringInOut);
        Task<byte[]> CreateAssetBarCode(int teamItem, long input);
        Task<string> GetInfoBarCode(int teamItem, long input);
        Task<QRCodeResultDto> ReadQRCode(string input);
    }
}
