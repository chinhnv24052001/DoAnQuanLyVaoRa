using System;
using System.Collections.Generic;
using System.Text;

namespace tmss.ImportExcel.ImportMstAsset.Dto
{
    public class MstAssetImportDto
    {
        public string AssetName { get; set; }
        public string AssetGroupName { get; set; }
        public string Validate { get; set; }
        public long AssetGroupId { get; set; }
    }
}
