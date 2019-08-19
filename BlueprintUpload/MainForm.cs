using BlueprintUpload.Entity;
using BlueprintUpload.Helpers;
using BlueprintUpload.Manage;
using Fleck;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BlueprintUpload
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var server = new WebSocketServer("ws://0.0.0.0:46000");

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                    {
                        MsgType = MsgTypeEnum.Info,
                        Msg = "已连接到图纸上传服务！"
                    }));
                };
                socket.OnClose = () =>
                {
                    socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                    {
                        MsgType = MsgTypeEnum.Info,
                        Msg = "图纸上传服务已关闭！"
                    }));
                    socket.Close();
                };
                socket.OnMessage = msg =>
                {
                    try
                    {
                        UploadInfoEntity uploadInfo = JsonConvert.DeserializeObject<UploadInfoEntity>(msg);
                        OpenFileDialog dlg = new OpenFileDialog { Multiselect = true };
                        List<FileInfoEntity> fileInfos = new List<FileInfoEntity>();
                        MessageEntity uploadFileMsg = null;
                        bool isNext = false;

                        Invoke(new MethodInvoker(() =>
                        {
                            this.TopMost = true;

                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                this.TopMost = false;
                                //this.WindowState = FormWindowState.Minimized;
                                this.ShowInTaskbar = false;

                                List<string> fileNames = dlg.FileNames.ToList();

                                #region 原始的方法，可实现
                                /*
                                if (FtpHelper.UploadMultiFile(fileNames, ref fileInfos))
                                {
                                    uploadFileMsg = new MessageEntity()
                                    {
                                        UploadType = uploadInfo.UploadType,
                                        MsgType = MsgTypeEnum.Info,
                                        Msg = "文件上传完成，正在写入数据信息！"
                                    };
                                    isNext = true;
                                }
                                else
                                {
                                    uploadFileMsg = new MessageEntity()
                                    {
                                        UploadType = uploadInfo.UploadType,
                                        MsgType = MsgTypeEnum.Question,
                                        Msg = "文件上传失败！"
                                    };
                                }
                                */
                                #endregion

                                #region 新的方法

                                var uploadResult = FtpHelper.UploadMultiFile(fileNames);

                                if (uploadResult.Result)
                                {
                                    uploadFileMsg = new MessageEntity()
                                    {
                                        UploadType = uploadInfo.UploadType,
                                        MsgType = MsgTypeEnum.Info,
                                        Msg = "文件上传完成，正在写入数据信息！"
                                    };
                                    fileInfos = uploadResult.Data;
                                    isNext = true;
                                }
                                else
                                {
                                    uploadFileMsg = new MessageEntity()
                                    {
                                        UploadType = uploadInfo.UploadType,
                                        MsgType = MsgTypeEnum.Question,
                                        Msg = uploadResult.Msg
                                    };
                                }

                                #endregion
                            }
                            else
                            {
                                this.TopMost = false;

                                uploadFileMsg = new MessageEntity()
                                {
                                    UploadType = uploadInfo.UploadType,
                                    MsgType = MsgTypeEnum.Info,
                                    Msg = "未选择文件！"
                                };
                            }
                        }));

                        if (!isNext)
                        {
                            socket.Send(JsonConvert.SerializeObject(uploadFileMsg));
                            return;
                        }

                        //工艺图纸上传
                        if (uploadInfo.UploadType == UploadTypeEnum.ProcessBlueprint)
                        {
                            var model = JsonConvert.DeserializeObject<ProcessBlueprintEntity>(uploadInfo.Data.ToString());

                            var obj = new
                            {
                                UserCode = model.UserCode,
                                UserName = model.UserName,
                                PBomID = model.BomID,
                                FileInfos = fileInfos
                            };
                            string url = $"http://{ConfigInfo.API}/api/Mms/MES_BN_ProductProcessRoute/PostUpdateProcessFigureByBlueprintUpload";
                            var result = HttpHelper.PostTByUrl<ResultModel>(url, obj);

                            if (result.Result)
                            {
                                socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                                {
                                    UploadType = UploadTypeEnum.ProcessBlueprint,
                                    MsgType = MsgTypeEnum.Success,
                                    Data = new
                                    {
                                        UserInfo = new
                                        {
                                            UserCode = model.UserCode,
                                            UserName = model.UserName
                                        },
                                        FileInfos = fileInfos
                                    },
                                    Msg = "文件上传成功！"
                                }));
                            }
                            else
                            {
                                socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                                {
                                    UploadType = UploadTypeEnum.ProcessBlueprint,
                                    MsgType = MsgTypeEnum.Question,
                                    Data = new
                                    {
                                        UserInfo = new
                                        {
                                            UserCode = model.UserCode,
                                            UserName = model.UserName
                                        }
                                    },
                                    Msg = result.Msg
                                }));
                            }

                        }

                        //工艺卡片编制图纸上传
                        if (uploadInfo.UploadType == UploadTypeEnum.PRouteBlueprint)
                        {
                            var model = JsonConvert.DeserializeObject<PRouteBlueprintEntity>(uploadInfo.Data.ToString());
                            string tempID = ((long)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1))).TotalSeconds).ToString();

                            string url = $"http://{ConfigInfo.API}/api/Mms/MES_BN_ProductProcessRoute/PostUpLoadPRouteBlueprint";
                            ResultModel result = HttpHelper.PostTByUrl<ResultModel>(url, new
                            {
                                Files = fileInfos,
                                User = new
                                {
                                    UserCode = model.UserCode,
                                    UserName = model.UserName
                                },
                                SourceID = model.ID,
                                TempID = tempID,
                                Type = (int)model.Type
                            });

                            if (result.Result)
                            {
                                socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                                {
                                    UploadType = UploadTypeEnum.PRouteBlueprint,
                                    MsgType = MsgTypeEnum.Success,
                                    Data = new
                                    {
                                        UserInfo = new
                                        {
                                            UserCode = model.UserCode,
                                            UserName = model.UserName
                                        },
                                        FileInfos = fileInfos,
                                        SourceID = tempID
                                    },
                                    Msg = "文件上传成功！"
                                }));
                            }
                            else
                            {
                                socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                                {
                                    UploadType = UploadTypeEnum.PRouteBlueprint,
                                    MsgType = MsgTypeEnum.Question,
                                    Data = new
                                    {
                                        UserInfo = new
                                        {
                                            UserCode = model.UserCode,
                                            UserName = model.UserName
                                        }
                                    },
                                    Msg = result.Msg
                                }));
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        socket.Send(JsonConvert.SerializeObject(new MessageEntity()
                        {
                            MsgType = MsgTypeEnum.Error,
                            Msg = ex.Message
                        }));
                    }
                };
            });

        }
    }
}
