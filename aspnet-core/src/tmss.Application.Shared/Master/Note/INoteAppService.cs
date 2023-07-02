using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.AssetGroup.Dto;
using tmss.Master.Note.Dto;

namespace tmss.Master.Note
{
    public interface INoteAppService : IApplicationService
    {
        Task DeleteById(long id);
        Task<NoteSelectOutputDto> LoadById(long id);
        Task Save(NoteSaveDto assetGroupSave);
        Task<PagedResultDto<NoteSelectOutputDto>> LoadAll(NoteInputDto input);
        Task<List<NoteSelectOutputDto>> GetNoteTextForSelect();
    }
}
