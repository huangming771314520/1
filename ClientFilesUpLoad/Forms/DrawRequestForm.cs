using ClientFilesUpLoad.Helpers;
using ClientFilesUpLoad.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientFilesUpLoad.Forms
{
    public partial class DrawRequestForm : Form
    {
        private TaskInfoForm parentForm = new TaskInfoForm();//父级窗体
        private string dRequestCode = string.Empty;
        private string dirPath = string.Empty;//文件目录
        private string filePath = string.Empty;//文件路径
        private string fileName = string.Empty;//文件名称

        public DrawRequestForm(TaskInfoForm _form, DataGridViewRow _dgr)
        {
            parentForm = _form;
            dRequestCode = _dgr.Cells["dRequestCode"].Value.ToString();

            CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();

            btnUpLoad.Enabled = false;
        }

        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "DWG文件|*.dwg;*.DWG|ZIP文件--测试使用|*.zip;*.ZIP|TXT文件--测试使用|*.txt;*.TXT";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePath = dialog.FileName;
                fileName = dialog.SafeFileName;
                dirPath = filePath.Substring(0, filePath.Length - fileName.Length - 1);

                txtFilePath.Text = filePath;
                lblFileName.Text = dialog.SafeFileName;

                FileInfo info = new FileInfo(filePath);
                lblFileSize.Text = Math.Ceiling(info.Length / 1024.0) >= 1024 ? ((Math.Ceiling(info.Length / 1024.0) / 1024).ToString("F2") + "MB") : (Math.Ceiling(info.Length / 1024.0).ToString("F2") + "KB");

                btnUpLoad.Enabled = true;
            }
        }

        private void BtnUpLoad_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(delegate
            {
                FileStream fStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader bReader = new BinaryReader(fStream);
                long fileLength = fStream.Length;
                string md5str = Common.GetStreamMd5(filePath);
                string newFileName = md5str + Path.GetExtension(fileName);

                var webFileResume = HttpHelper.GetTByUrl<ResultModel>(string.Format(@"http://{0}/api/Mms/SYS_FileManage/GetResumFile?fileName={1}", Program.API, newFileName));

                if (webFileResume.Result)
                {
                    WebClient webClient = new WebClient();
                    int byteSize = 64 * 1024;
                    long cruuent = Convert.ToInt64(webFileResume.Data);
                    bool isUploadOK = true;

                    try
                    {
                        byte[] data;
                        if (cruuent > 0)
                        {
                            fStream.Seek(cruuent, SeekOrigin.Current);
                        }

                        for (; cruuent <= fileLength; cruuent += byteSize)
                        {
                            if (cruuent + byteSize > fileLength)
                            {
                                data = new byte[Convert.ToInt64(fileLength - cruuent)];
                                bReader.Read(data, 0, Convert.ToInt32(fileLength - cruuent));
                            }
                            else
                            {
                                data = new byte[byteSize];
                                bReader.Read(data, 0, byteSize);
                            }

                            try
                            {
                                webClient.Headers.Remove(HttpRequestHeader.ContentRange);
                                webClient.Headers.Add(HttpRequestHeader.ContentRange, "bytes " + cruuent + "-" + (cruuent + byteSize) + "/" + fileLength);

                                byte[] byRemoteInfo = webClient.UploadData(string.Format(@"http://{0}/api/Mms/SYS_FileManage/PostRsume?filename={1}", Program.API, newFileName), "post", data);
                                string sRemoteInfo = Encoding.Default.GetString(byRemoteInfo);

                                ResultModel result = JsonConvert.DeserializeObject<ResultModel>(sRemoteInfo);

                                if (result.Result)
                                {
                                    if (cruuent + byteSize > fileLength)
                                    {
                                        prgUpLoad.Value = 100;
                                        lblPercent.Text = "100%";
                                    }
                                    else
                                    {
                                        prgUpLoad.Value = Convert.ToInt32((cruuent + byteSize) * 100 / fileLength);
                                        lblPercent.Text = ((double)(cruuent + byteSize) / fileLength).ToString("p");
                                    }
                                }
                                else
                                {
                                    //MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    Common.MessageBoxShow(this, result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    isUploadOK = false;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Common.MessageBoxShow(this, ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                isUploadOK = false;
                                break;
                            }
                        }

                        if (isUploadOK)
                        {
                            ResultModel setDataResult = HttpHelper.PostTByUrl<ResultModel>(string.Format(@"http://{0}/api/Mms/SYS_FileManage/PostSetFileManageData", Program.API), new
                            {
                                BindCode = dRequestCode,
                                FileName = fileName,
                                MD5Code = md5str
                            });

                            if (setDataResult.Result)
                            {
                                //MessageBox.Show("上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                Common.MessageBoxShow(this, "上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                            }
                            else
                            {
                                //MessageBox.Show(string.Format(@"上传失败！\n{0}", setDataResult.Msg), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                Common.MessageBoxShow(this, string.Format(@"上传失败！\n{0}", setDataResult.Msg), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Common.MessageBoxShow(this, ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    finally
                    {
                        fStream.Close();
                        bReader.Close();
                    }
                }
                else
                {
                    fStream.Close();
                    bReader.Close();
                    //MessageBox.Show(webFileResume.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Common.MessageBoxShow(this, webFileResume.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }))
            {
                IsBackground = true
            };
            thread.Start();
        }

        private void BtnKillTask_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("敬请期待...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Common.MessageBoxShow(this, "敬请期待...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }





    }


}
