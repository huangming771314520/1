namespace ClientFilesUpLoad.Models
{
    public class FileUpLoadModel
    {
        public enum FileUpLoadStateEnum
        {
            未开始 = 0,
            上传中 = 1,
            暂停中 = 2,
            上传成功 = 3,
            上传失败 = 4
        }

        public class ResultModel
        {
            public ResultModel()
            {

            }

            public ResultModel(bool result)
            {
                Result = result;
            }

            public ResultModel(string msg)
            {
                Msg = msg;
            }

            public ResultModel(bool result, string msg)
            {
                Result = result;
                Msg = msg;
            }

            public bool Result { get; set; }

            public string Msg { get; set; }
        }
    }
}
