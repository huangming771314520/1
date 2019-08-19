using ClientFilesUpLoad.Helpers;
using ClientFilesUpLoad.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ClientFilesUpLoad.Forms
{
    public partial class FilesInfoForm : Form
    {
        private TaskInfoForm parentForm = new TaskInfoForm();//父级窗体
        private List<DesignTaskResultModel.DataModel> DesignTaskResultData = new List<DesignTaskResultModel.DataModel>();//设计任务类型结果
        private long taskID;
        private long projectID;
        private string crojectName = string.Empty;
        private long crojectDetailID;
        private string contractCode = string.Empty;
        private int designType;
        private DateTime? actualFinishTime = null;
        private string appPath = Application.StartupPath;//程序目录
        private string bomDirPath = string.Empty;//xml文件目录
        private string bomFilePath = string.Empty;//xml文件路径
        private string bomFileName = string.Empty;//xml文件名称
        private List<RootNewModel.ProductModel.DocModel> docModels = new List<RootNewModel.ProductModel.DocModel>();
        private List<string> docFilePaths = new List<string>();
        private Queue<KeyValuePair<int, string>> taskQueue = new Queue<KeyValuePair<int, string>>();
        private bool isUploadXML = true;

        /// <summary>
        /// 构造函数 - 初始化窗体
        /// </summary>
        /// <param name="_form"></param>
        /// <param name="_dgr"></param>
        public FilesInfoForm(TaskInfoForm _form, DataGridViewRow _dgr, List<DesignTaskResultModel.DataModel> designTaskResultData)
        {
            parentForm = _form;
            DesignTaskResultData = designTaskResultData;

            taskID = Convert.ToInt64(_dgr.Cells["TaskID"].Value);
            projectID = Convert.ToInt64(_dgr.Cells["ProjectID"].Value);
            crojectName = _dgr.Cells["ProjectName"].Value.ToString();
            crojectDetailID = Convert.ToInt64(_dgr.Cells["ProjectDetailID"].Value);
            contractCode = _dgr.Cells["ContractCode"].Value.ToString();
            designType = string.IsNullOrEmpty(_dgr.Cells["DesignType"].Value.ToString()) ? 0 : Convert.ToInt32(_dgr.Cells["DesignType"].Value);
            actualFinishTime = _dgr.Cells["ActualFinishTime"].Value == null ? (DateTime?)null : Convert.ToDateTime(_dgr.Cells["ActualFinishTime"].Value.ToString());

            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            DoubleBufferListView.DoubleBufferedListView(lvFileInfo, true);
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilesInfoForm_Load(object sender, EventArgs e)
        {
            cmbDesignTaskResult.DataSource = DesignTaskResultData;
            cmbDesignTaskResult.DisplayMember = "Text";
            cmbDesignTaskResult.ValueMember = "Value";

            lvFileInfo.SmallImageList = new ImageList() { ImageSize = new Size(1, 30) };
            btnUpLoad.Enabled = false;
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XML文件|*.xml;*.XML";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                bomFilePath = dialog.FileName;
                bomFileName = dialog.SafeFileName;
                bomDirPath = bomFilePath.Substring(0, bomFilePath.Length - bomFileName.Length - 1);

                txtBomFilePath.Text = dialog.SafeFileName;
                docModels.Clear();
                lvFileInfo.Items.Clear();

                //新建设计项目
                if (designType.Equals(1))
                {
                    RootNewModel model = GetProductDataByLocalXML(dialog.FileName);

                    for (int i = 0; i < docModels.Count; i++)
                    {
                        string docFilePathA = string.Format("{0}\\{1}\\{2}", bomDirPath, bomFileName.Substring(0, bomFileName.LastIndexOf('.')), docModels[i].Filename);
                        string docFilePathB = string.Format("{0}\\{1}", bomDirPath, docModels[i].Filename);
                        if (File.Exists(docFilePathA))
                        {
                            docFilePaths.Add(docFilePathA);
                            taskQueue.Enqueue(new KeyValuePair<int, string>(i, docFilePathA));
                            FileInfo info = new FileInfo(docFilePathA);

                            ListViewItem lvi = new ListViewItem((i + 1).ToString());

                            lvi.SubItems.Add(docModels[i].Filename);
                            lvi.SubItems.Add(Math.Ceiling(info.Length / 1024.0) >= 1024 ? ((Math.Ceiling(info.Length / 1024.0) / 1024).ToString("F2") + "MB") : (Math.Ceiling(info.Length / 1024.0).ToString("F2") + "KB"));
                            lvi.SubItems.Add("0.00%");
                            lvi.SubItems.Add(FileUpLoadModel.FileUpLoadStateEnum.未开始.ToString());

                            lvi.BackColor = i % 2 == 0 ? Color.FromArgb(250, 250, 250) : Color.FromArgb(247, 247, 247);
                            lvi.Tag = docModels[i];

                            lvFileInfo.Items.Add(lvi);
                        }
                        else if (File.Exists(docFilePathB))
                        {
                            docFilePaths.Add(docFilePathB);
                            taskQueue.Enqueue(new KeyValuePair<int, string>(i, docFilePathB));
                            FileInfo info = new FileInfo(docFilePathB);

                            ListViewItem lvi = new ListViewItem((i + 1).ToString());
                            lvi.SubItems.Add(docModels[i].Filename);
                            lvi.SubItems.Add(Math.Ceiling(info.Length / 1024.0) >= 1024 ? ((Math.Ceiling(info.Length / 1024.0) / 1024).ToString("F2") + "MB") : (Math.Ceiling(info.Length / 1024.0).ToString("F2") + "KB"));
                            lvi.SubItems.Add("0.00%");
                            lvi.SubItems.Add(FileUpLoadModel.FileUpLoadStateEnum.未开始.ToString());

                            lvi.BackColor = i % 2 == 0 ? Color.FromArgb(250, 250, 250) : Color.FromArgb(247, 247, 247);
                            lvi.Tag = docModels[i];

                            lvFileInfo.Items.Add(lvi);
                        }
                        else
                        {
                            MessageBox.Show("文件不完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtBomFilePath.Text = string.Empty;
                            txtBomFilePath.Focus();
                            return;
                        }
                    }
                }
                //设计更改申请
                else if (designType.Equals(2))
                {
                    RootChangeModel model = GetDocDataByLocalXML(dialog.FileName);

                    for (int i = 0; i < model.Doc.Count; i++)
                    {
                        string docFilePathA = string.Format("{0}\\{1}\\{2}", bomDirPath, bomFileName.Substring(0, bomFileName.LastIndexOf('.')), model.Doc[i].Filename);
                        string docFilePathB = string.Format("{0}\\{1}", bomDirPath, model.Doc[i].Filename);
                        if (File.Exists(docFilePathA))
                        {
                            docFilePaths.Add(docFilePathA);
                            FileInfo info = new FileInfo(docFilePathA);

                            ListViewItem lvi = new ListViewItem((i + 1).ToString());

                            lvi.SubItems.Add(model.Doc[i].Filename);
                            lvi.SubItems.Add(Math.Ceiling(info.Length / 1024.0) >= 1024 ? ((Math.Ceiling(info.Length / 1024.0) / 1024).ToString("F2") + "MB") : (Math.Ceiling(info.Length / 1024.0).ToString("F2") + "KB"));
                            lvi.SubItems.Add("0.00%");
                            lvi.SubItems.Add(FileUpLoadModel.FileUpLoadStateEnum.未开始.ToString());

                            lvi.BackColor = i % 2 == 0 ? Color.FromArgb(250, 250, 250) : Color.FromArgb(247, 247, 247);
                            lvi.Tag = model.Doc[i];

                            lvFileInfo.Items.Add(lvi);
                        }
                        else if (File.Exists(docFilePathB))
                        {
                            docFilePaths.Add(docFilePathB);
                            FileInfo info = new FileInfo(docFilePathB);

                            ListViewItem lvi = new ListViewItem((i + 1).ToString());
                            lvi.SubItems.Add(model.Doc[i].Filename);
                            lvi.SubItems.Add(Math.Ceiling(info.Length / 1024.0) >= 1024 ? ((Math.Ceiling(info.Length / 1024.0) / 1024).ToString("F2") + "MB") : (Math.Ceiling(info.Length / 1024.0).ToString("F2") + "KB"));
                            lvi.SubItems.Add("0.00%");
                            lvi.SubItems.Add(FileUpLoadModel.FileUpLoadStateEnum.未开始.ToString());

                            lvi.BackColor = i % 2 == 0 ? Color.FromArgb(250, 250, 250) : Color.FromArgb(247, 247, 247);
                            lvi.Tag = model.Doc[i];

                            lvFileInfo.Items.Add(lvi);
                        }
                        else
                        {
                            MessageBox.Show("文件不完整！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtBomFilePath.Text = string.Empty;
                            txtBomFilePath.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("到底是新建设计项目呢还是设计更改申请呢？\n不懂、不懂...", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    btnUpLoad.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 点击上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpLoad_Click(object sender, EventArgs e)
        {
            if (docFilePaths.Count <= 0)
            {
                MessageBox.Show(string.Format(@"从『{0}』中未读出数据", bomFileName), "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (cmbDesignTaskResult.SelectedValue.Equals("-1"))
            {
                MessageBox.Show(string.Format(@"请选择设计任务结果类型"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            new Task(() =>
            {
                int taskNum = 5;
                Task[] tasks = new Task[taskNum];
                for (int i = 0; i < taskNum; i++)
                {
                    tasks[i] = new Task(() =>
                    {
                        UploadFile();
                    });
                    tasks[i].Start();
                }
                Task.WaitAll(tasks);

                if (isUploadXML)
                {
                    UploadXML();
                }

            }).Start();

        }


        /// <summary>
        /// 结束任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnKillTask_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要结束当前任务吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.OK)
            {
                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProductDataUpKillDate?taskID={1}", Program.API, taskID);
                var result = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                if (result.Result)
                {
                    MessageBox.Show("更新实际结束时间成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                    Close();
                }
                else
                {
                    MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        /// <summary>
        /// ListView列表自适应列宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvFileInfo_SizeChanged(object sender, EventArgs e)
        {
            lvFileInfo.Columns[1].Width = lvFileInfo.Width - 306 - 24;
        }

        /// <summary>
        /// 取任务 上传图纸文件
        /// </summary>
        private void UploadFile()
        {
            while (taskQueue.Count > 0)
            {
                KeyValuePair<int, string> keyValuePair = taskQueue.Dequeue();
                int index = keyValuePair.Key;
                string fileName = Path.GetFileName(keyValuePair.Value);

                if (File.Exists(keyValuePair.Value))
                {
                    FileStream fStream = new FileStream(keyValuePair.Value, FileMode.Open, FileAccess.Read);
                    BinaryReader bReader = new BinaryReader(fStream);
                    long fileLength = fStream.Length;
                    //string md5str = Common.GetStreamMd5(keyValuePair.Value);
                    //string newFileName = md5str + Path.GetExtension(fileName);

                    lvFileInfo.Items[index].SubItems[4].Text = FileUpLoadModel.FileUpLoadStateEnum.上传中.ToString();

                    var webFileResume = HttpHelper.GetTByUrl<ResultModel>(string.Format(@"http://{0}/api/Mms/WinFormClient/GetResumFile?fileName={1}", Program.API, fileName));

                    if (webFileResume.Result)
                    {
                        WebClient webClient = new WebClient();
                        int byteSize = 64 * 1024;
                        long cruuent = Convert.ToInt64(webFileResume.Data);

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

                                    byte[] byRemoteInfo = webClient.UploadData(string.Format(@"http://{0}/api/Mms/WinFormClient/PostRsume?filename={1}", Program.API, fileName), "post", data);
                                    ResultModel result = JsonConvert.DeserializeObject<ResultModel>(Encoding.Default.GetString(byRemoteInfo));

                                    if (result.Result)
                                    {
                                        if (cruuent + byteSize > fileLength)
                                        {
                                            lvFileInfo.Items[index].SubItems[4].Text = FileUpLoadModel.FileUpLoadStateEnum.上传成功.ToString();
                                            lvFileInfo.Items[index].SubItems[3].Text = "100%";
                                            lvFileInfo.Items[index].BackColor = Color.FromArgb(240, 255, 240);
                                        }
                                        else
                                        {
                                            lvFileInfo.Items[index].SubItems[3].Text = ((double)(cruuent + byteSize) / fileLength).ToString("p");
                                        }
                                    }
                                    else
                                    {
                                        lvFileInfo.Items[index].SubItems[4].Text = FileUpLoadModel.FileUpLoadStateEnum.上传失败.ToString();
                                        lvFileInfo.Items[index].BackColor = Color.FromArgb(255, 240, 240);
                                        MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        isUploadXML = false;
                                        break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    isUploadXML = false;
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            isUploadXML = false;
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
                        MessageBox.Show(webFileResume.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        isUploadXML = false;
                        return;
                    }
                }
                else
                {
                    //lvFileInfo.Items[index].SubItems[4].Text = @"文件丢失";
                    //lvFileInfo.Items[index].BackColor = Color.FromArgb(255, 240, 240);
                }
            }
        }

        /// <summary>
        /// 上传xml文件
        /// </summary>
        private void UploadXML()
        {
            try
            {
                using (FileStream fs = File.Open(bomFilePath, FileMode.OpenOrCreate))
                {
                    byte[] tempBytes = new byte[(int)fs.Length];
                    fs.Read(tempBytes, 0, tempBytes.Length);
                    fs.Close();

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://{0}/api/Mms/WinFormClient/PostProductDataByLocalXML?taskID={1}&projectID={2}&projectName={3}&projectDetailID={4}&contractCode={5}&designType={6}&designTaskResultType={7}&designTaskResultText={8}",
                    Program.API, taskID, projectID, crojectName, crojectDetailID, contractCode, designType, cmbDesignTaskResult.SelectedValue, cmbDesignTaskResult.Text));
                    request.Method = "post";
                    request.ContentLength = tempBytes.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(tempBytes, 0, tempBytes.Length);
                    newStream.Close();

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

                    var result = JsonConvert.DeserializeObject<ResultModel>(reader.ReadToEnd());

                    if (result.Result)
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                if (actualFinishTime == null)
                                {
                                    DialogResult dialogResult = MessageBox.Show("上传成功！\n是否立即结束任务？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (dialogResult == DialogResult.OK)
                                    {
                                        string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProductDataUpKillDate?taskID={1}", Program.API, taskID);
                                        var killTask = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                                        if (!killTask.Result)
                                        {
                                            MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        }
                                    }
                                    parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                                    Close();
                                }
                                else
                                {
                                    string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProductDataUpKillDate?taskID={1}", Program.API, taskID);
                                    var result_killDate = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                                    if (result_killDate.Result)
                                    {
                                        MessageBox.Show("上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                                        Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show(string.Format("文件上传成功！\n但是，更新实际结束时间失败\n详情：{0}", result.Msg), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                            }));
                        }
                        else
                        {
                            if (actualFinishTime == null)
                            {
                                DialogResult dialogResult = MessageBox.Show("上传成功！\n是否立即结束任务？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (dialogResult == DialogResult.OK)
                                {
                                    string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProductDataUpKillDate?taskID={1}", Program.API, taskID);
                                    var killTask = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                                    if (!killTask.Result)
                                    {
                                        MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                                Close();
                            }
                            else
                            {
                                string url = string.Format(@"http://{0}/api/Mms/WinFormClient/GetProductDataUpKillDate?taskID={1}", Program.API, taskID);
                                var result_killDate = Helpers.HttpHelper.GetTByUrl<ResultModel>(url);
                                if (result_killDate.Result)
                                {
                                    MessageBox.Show("上传成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    parentForm.TaskInfoForm_Load(new object(), new EventArgs());
                                    Close();
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("文件上传成功！\n但是，更新实际结束时间失败\n详情：{0}", result.Msg), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(result.Msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("在上传xml时遇到意外的错误！\n详情：{0}", ex.Message), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 读取XML文件 -- 新建设计项目
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public RootNewModel GetProductDataByLocalXML(string strXmlFilePath)
        {
            try
            {
                //xml文件读取的对象
                RootNewModel result = new RootNewModel();

                //文档对象
                XmlDocument doc = new XmlDocument();

                //加载xml文件
                //doc.Load(strXmlFilePath);

                string strXML = File.ReadAllText(strXmlFilePath, Encoding.UTF8);
                int explain_S = strXML.IndexOf("<?");
                int explain_E = strXML.IndexOf("?>");
                string strExplain = string.Empty;
                if (!explain_S.Equals(-1))
                {
                    strExplain = strXML.Substring(explain_S, explain_E + 2);
                    strXML = strXML.Replace(strExplain, strExplain.ToLower());
                }

                doc.LoadXml(strXML);

                //查root根节点
                XmlNode rootNode = GetNode(doc, "/ROOT", "找不到【ROOT】！");

                //查询项目节点（项目有一个，所以是对象）
                XmlNode projectNode = GetNode(doc, "/ROOT/PROJECT", "找不到【PROJECT】！");

                //保存项目属性信息
                result.Project.Projectid = GetAttrValue(projectNode, "PROJECTID");
                result.Project.Projectname = GetAttrValue(projectNode, "PROJECTNAME");

                //获取产品节点（产品有多个，所以是集合）
                XmlNodeList ProductNodes = GetNodeList(doc, "/ROOT//PRODUCT", "找不到【PRODUCT】！");

                //遍历产品
                foreach (XmlNode itemA in ProductNodes)
                {
                    //实例化一个产品对象
                    RootNewModel.ProductModel productModel = new RootNewModel.ProductModel();

                    //获取这个产品下的所有DOC文档节点
                    XmlNodeList DOCNodesA = GetNodeList(itemA, "DOC");
                    //获取这个产品下的所有Part零件节点
                    XmlNodeList PartNodesA = GetNodeList(itemA, "PART");

                    //获取产品信息（属性值）
                    productModel.Code = GetAttrValue(itemA, "CODE");
                    productModel.Code1 = GetAttrValue(itemA, "CODE1");
                    productModel.Version = GetAttrValue(itemA, "VERSION");
                    productModel.Name = GetAttrValue(itemA, "NAME");
                    productModel.Quantity = GetAttrValue(itemA, "QUANTITY");
                    productModel.Material = GetAttrValue(itemA, "MATERIAL");
                    productModel.Sigweight = GetAttrValue(itemA, "SIGWEIGHT");
                    productModel.TotWeight = GetAttrValue(itemA, "TOTWEIGHT");
                    productModel.Remark = GetAttrValue(itemA, "REMARK");
                    productModel.FaHCode = GetAttrValue(itemA, "FAHCODE");

                    //遍历产品文档
                    foreach (XmlNode itemB1 in DOCNodesA)
                    {
                        RootNewModel.ProductModel.DocModel docModel = new RootNewModel.ProductModel.DocModel()
                        {
                            Code = GetAttrValue(itemB1, "CODE"),
                            Code1 = GetAttrValue(itemB1, "CODE1"),
                            Name = GetAttrValue(itemB1, "NAME"),
                            Version = GetAttrValue(itemB1, "VERSION"),
                            Gcname = GetAttrValue(itemB1, "GCNAME"),
                            Page = GetAttrValue(itemB1, "PAGE"),
                            Totpage = GetAttrValue(itemB1, "TOTPAGE"),
                            Filename = GetAttrValue(itemB1, "FILENAME")
                        };
                        productModel.Doc.Add(docModel);

                        docModels.Add(docModel);
                    }

                    //遍历所有一级Part零件
                    foreach (XmlNode itemB2 in PartNodesA)
                    {
                        //当前零件的父级节点，即当前产品节点
                        XmlNode productNode = itemB2.ParentNode;

                        //通过递归方法获取当前零件下的所有Part零件以及DOC文档
                        productModel.Part.Add(GetPart(itemB2));
                    }

                    //将当前产品信息添加到xml对象的产品里
                    result.Product.Add(productModel);
                }

                btnUpLoad.Enabled = true;
                btnUpLoad.Focus();

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("XML文件错误！\n详情：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnUpLoad.Enabled = false;
                docModels.Clear();
                return new RootNewModel();
            }
        }

        #region
        private XmlNode GetNode(XmlDocument document, string xpath, string errmsg = null)
        {
            XmlNode node = document.SelectSingleNode(xpath);
            if (node == null)
            {
                if (string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception(string.Format("找不到【{0}】！", xpath));
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            else
            {
                return node;
            }
        }
        private XmlNode GetNode(XmlNode node, string xpath, string errmsg = null)
        {
            XmlNode newnode = node.SelectSingleNode(xpath);
            if (newnode == null)
            {
                if (string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception(string.Format("找不到【{0}】！", xpath));
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            else
            {
                return newnode;
            }
        }
        private XmlNodeList GetNodeList(XmlDocument document, string xpath, string errmsg = null)
        {
            XmlNodeList nodeList = document.SelectNodes(xpath);
            if (nodeList == null)
            {
                if (string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception(string.Format("找不到【{0}】！", xpath));
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            else
            {
                return nodeList;
            }
        }
        private XmlNodeList GetNodeList(XmlNode node, string xpath, string errmsg = null)
        {
            string[] content = new string[] { "PART", "DOC" };
            foreach (XmlNode item in node.ChildNodes)
            {
                if (!content.Contains(item.Name))
                {
                    throw new Exception(string.Format("节点名不匹配，【{0}】是未知的\n--------------------\n{1}", item.Name, item.OuterXml.Substring(0, item.OuterXml.IndexOf('>') + 1)));
                }
            }

            XmlNodeList nodeList = node.SelectNodes(xpath);
            if (nodeList == null)
            {
                if (string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception(string.Format("找不到【{0}】！", xpath));
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            else
            {
                return nodeList;
            }
        }
        private string GetAttrValue(XmlNode node, string attrname, string errmsg = null)
        {
            XmlAttribute attr = node.Attributes[attrname];
            if (attr == null)
            {
                if (string.IsNullOrEmpty(errmsg))
                {
                    throw new Exception(string.Format("找不到【{0}】！\n--------------------\n{1}", attrname, node.OuterXml.Substring(0, node.OuterXml.IndexOf('>') + 1)));
                }
                else
                {
                    throw new Exception(errmsg);
                }
            }
            else
            {
                return attr.Value;
            }
        }
        #endregion

        /// <summary>
        /// 递归-获取Part零件节点下的所有Part零件以及DOC文档
        /// </summary>
        /// <param name="partNode">Part零件节点</param>
        /// <returns></returns>
        public RootNewModel.ProductModel.PartModel GetPart(XmlNode partNode)
        {
            //try
            //{
            RootNewModel.ProductModel.PartModel result = new RootNewModel.ProductModel.PartModel();

            XmlNodeList DOCNodes = GetNodeList(partNode, "DOC");
            XmlNodeList PartNodes = GetNodeList(partNode, "PART");

            result.Code = GetAttrValue(partNode, "CODE");
            result.Code1 = GetAttrValue(partNode, "CODE1");
            result.Version = GetAttrValue(partNode, "VERSION");
            result.Name = GetAttrValue(partNode, "NAME");
            result.Quantity = GetAttrValue(partNode, "QUANTITY");
            result.Material = GetAttrValue(partNode, "MATERIAL");
            result.Sigweight = GetAttrValue(partNode, "SIGWEIGHT");
            result.TotWeight = GetAttrValue(partNode, "TOTWEIGHT");
            result.Remark = GetAttrValue(partNode, "REMARK");
            result.FaHCode = GetAttrValue(partNode, "FAHCODE");
            result.PartType = GetAttrValue(partNode, "PARTTYPE");

            foreach (XmlNode item in DOCNodes)
            {
                RootNewModel.ProductModel.DocModel docModel = new RootNewModel.ProductModel.DocModel()
                {
                    Code = GetAttrValue(item, "CODE"),
                    Code1 = GetAttrValue(item, "CODE1"),
                    Name = GetAttrValue(item, "NAME"),
                    Version = GetAttrValue(item, "VERSION"),
                    Gcname = GetAttrValue(item, "GCNAME"),
                    Page = GetAttrValue(item, "PAGE"),
                    Totpage = GetAttrValue(item, "TOTPAGE"),
                    Filename = GetAttrValue(item, "FILENAME")
                };
                result.Doc.Add(docModel);

                docModels.Add(docModel);
            }

            foreach (XmlNode item in PartNodes)
            {
                result.Part.Add(GetPart(item));
            }

            return result;
            //}
            //catch
            //{
            //    MessageBox.Show("XML文件错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            //    btnUpLoad.Enabled = false;
            //    return new RootNewModel.ProductModel.PartModel();
            //}
        }

        /// <summary>
        /// 读取XML文件 -- 设计更改申请
        /// </summary>
        /// <param name="strXmlFilePath"></param>
        /// <returns></returns>
        public RootChangeModel GetDocDataByLocalXML(string strXmlFilePath)
        {
            try
            {
                //xml文件读取的对象
                RootChangeModel result = new RootChangeModel();

                //文档对象
                XmlDocument doc = new XmlDocument();

                //加载xml文件
                //doc.Load(strXmlFilePath);

                string strXML = File.ReadAllText(strXmlFilePath, Encoding.UTF8);
                doc.LoadXml(strXML.ToLower());

                //查询文档节点
                XmlNode changeQNode = doc.SelectSingleNode("/root/changeq");
                XmlNode changeNode = doc.SelectSingleNode("/root/change");

                //保存文档属性信息
                result.ChangeQ = new RootChangeModel.FileModel() { FileName = changeQNode.Attributes["filename"].Value };
                result.Change = new RootChangeModel.FileModel() { FileName = changeNode.Attributes["filename"].Value };

                //获取Doc节点（Doc节点有多个，所以是集合）
                XmlNodeList DocNodes = doc.SelectNodes("/root//doc");

                //遍历Doc
                foreach (XmlNode itemA in DocNodes)
                {
                    //实例化一个Doc对象
                    RootChangeModel.DocModel docModel = new RootChangeModel.DocModel()
                    {
                        Code = itemA.Attributes["code"].Value,
                        Code1 = itemA.Attributes["code1"].Value,
                        Name = itemA.Attributes["name"].Value,
                        Version = itemA.Attributes["version"].Value,
                        Gcname = itemA.Attributes["gcname"].Value,
                        Page = itemA.Attributes["page"].Value,
                        Totpage = itemA.Attributes["totpage"].Value,
                        Filename = itemA.Attributes["filename"].Value,
                        Part = new List<RootChangeModel.DocModel.PartModel>()
                    };
                    //获取所有Part零件节点
                    XmlNodeList PartNodes = itemA.SelectNodes("part");

                    //遍历产品文档
                    foreach (XmlNode itemB1 in PartNodes)
                    {
                        docModel.Part.Add(new RootChangeModel.DocModel.PartModel()
                        {
                            Code = itemB1.Attributes["code"].Value,
                            Code1 = itemB1.Attributes["code1"].Value,
                            Name = itemB1.Attributes["name"].Value,
                            Material = itemB1.Attributes["material"].Value,
                            Sigweight = itemB1.Attributes["sigweight"].Value,
                            TotWeight = itemB1.Attributes["totweight"].Value,
                            Remark = itemB1.Attributes["remark"].Value
                        });
                    }

                    //将当前Doc信息添加到xml对象的Doc里
                    result.Doc.Add(docModel);
                }

                btnUpLoad.Enabled = true;
                btnUpLoad.Focus();

                return result;
            }
            catch (Exception ex)
            {
                string err = ex.ToString();
                MessageBox.Show("XML文件错误！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                btnUpLoad.Enabled = false;
                return new RootChangeModel();
            }

        }

    }

    public static class DoubleBufferListView
    {
        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedListView(this ListView lv, bool flag)
        {
            Type lvType = lv.GetType();
            PropertyInfo pi = lvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(lv, flag, null);
        }

    }
}
