using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessBlueprintUpload.Models
{
    public class UpLoadStateModel
    {

    }

    public enum UpLoadStateEnum
    {
        未开始 = 0,
        上传中 = 1,
        暂停中 = 2,
        上传成功 = 3,
        上传失败 = 4
    }

}
