using BlueprintUpload.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueprintUpload.Entity
{
    public class MessageEntity
    {
        public UploadTypeEnum UploadType { get; set; }

        public MsgTypeEnum MsgType { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }
    }
}
