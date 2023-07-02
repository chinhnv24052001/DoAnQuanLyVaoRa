using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.AssetManaments.Dto;
using tmss.AssetManaments.RequestAssetBring.Dto;
using tmss.Master.Asset.Dto;
using tmss.Master.Vender.Dto;

namespace tmss.AssetManaments
{
    public interface IRequestAssetBringAppService: IApplicationService
    {
        Task DeleteById(long id);
        Task<RequestAssetBringSaveDto> LoadById(long id);
        Task<RequestAssetBringSaveDto> LoadUserForCreate();
        Task Save(RequestAssetBringSaveDto input);
        Task<PagedResultDto<RequestAssetBringSelectOutputDto>> LoadAll(RequestAssetBringInputDto input);
        Task<int> CountDraftRequest();
        Task<List<AssetSelectOutputDto>> GetAssetForEdit();
        Task<List<VenderSelectOutputDto>> GetVenderForEdit();
        Task<RequestAssetBringDetailDto> GetRequestAssetBringDetail(long id);
        Task<string> GetUserName();
        Task<bool> TemManagerApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto);
        Task<bool> ManagerApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto);
        Task<bool> AdmApproveOrReject(ApproveOrRejectRequestDto approveOrRejectRequestDto);
        Task<bool> CheckWorkerLearnedSafety(string input);
    }
}
