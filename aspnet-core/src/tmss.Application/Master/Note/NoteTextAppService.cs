using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmss.Master.Note.Dto;
using tmss.Master.Vender.Dto;

namespace tmss.Master.Note
{
    public class NoteTextAppService : tmssAppServiceBase, INoteAppService
    {
        private readonly IRepository<MstNote, long> _noteAppService;

        public NoteTextAppService(IRepository<MstNote, long> noteAppService)
        {
            _noteAppService = noteAppService;
        }
        public async Task DeleteById(long id)
        {
            await _noteAppService.DeleteAsync(id);
        }
        public async Task<PagedResultDto<NoteSelectOutputDto>> LoadAll(NoteInputDto input)
        {
            var listAssetGroup = from a in _noteAppService.GetAll()
                                  .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), p => p.NoteText.Contains(input.Filter))
                                 select new NoteSelectOutputDto()
                                 {
                                     Id = a.Id,
                                     NoteText = a.NoteText
                                 };
            var result = listAssetGroup.Skip(input.SkipCount).Take(input.MaxResultCount);
            var assetGroupCount = listAssetGroup.Count();
            return new PagedResultDto<NoteSelectOutputDto>
                (assetGroupCount,
                result.ToList()
                );
        }

        public async Task<NoteSelectOutputDto> LoadById(long id)
        {
            var assetGroup = _noteAppService.GetAll()
               .Select(p => new NoteSelectOutputDto { Id = p.Id, NoteText = p.NoteText })
               .FirstOrDefault(p => p.Id == id);
            return assetGroup;
        }

        public async Task Save(NoteSaveDto noteSave)
        {
            if (await _noteAppService.FirstOrDefaultAsync(noteSave.Id) == null)
            {
                MstNote mstNote = new MstNote();
                mstNote.Id = noteSave.Id;
                mstNote.NoteText = noteSave.NoteText;
                mstNote.CreationTime = DateTime.Now;
                mstNote.CreatorUserId = AbpSession.UserId;
                await _noteAppService.InsertAsync(mstNote);
            }
            else
            {
                MstNote mstNote = await _noteAppService.FirstOrDefaultAsync(noteSave.Id);
                mstNote.NoteText = noteSave.NoteText;
                mstNote.LastModificationTime = DateTime.Now;
                mstNote.LastModifierUserId = AbpSession.UserId;
                await _noteAppService.UpdateAsync(mstNote);
            }
        }

        public async Task<List<NoteSelectOutputDto>> GetNoteTextForSelect()
        {
            var listNote = _noteAppService.GetAll()
                     .Select(p => new NoteSelectOutputDto
                     {
                         NoteText = p.NoteText,
                         Id = p.Id,
                     });
            return listNote.ToList();
        }
    }
}
