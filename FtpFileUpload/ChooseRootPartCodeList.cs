using FtpFileUpload.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpFileUpload
{
    public partial class ChooseRootPartCodeList : Form
    {
        public ChooseRootPartCodeList()
        {
            InitializeComponent();
        }

        public static readonly int PageSize = 10;

        private void ChooseRootPartCodeList_Load(object sender, EventArgs e)
        {
            int TotalPage = 0;
            var list = GetDynamicPageList("ChooseProjectPart", 1, PageSize, out TotalPage)
                .Select(a => new
                {
                    ContractCode = a.ContractCode,
                    ProjectName = a.ProjectName,
                    ProductName = a.ProductName,
                    ProductType = a.ProductType,
                    taskText = a.taskText,
                    Model = a.Model,
                    Specifications = a.Specifications,
                    Quantity = a.Quantity,
                    PartFigureCode = a.PartFigureCode,
                    TaskDescription = a.TaskDescription,
                    PartName = a.PartName,
                    PartCode = a.PartCode
                }).ToList();
            dgvProjectPart.DataSource = list;
            PageInfo.Text = "1";
            PageTotal.Text = Math.Ceiling(Convert.ToDouble(TotalPage) / Convert.ToDouble(PageSize)).ToString();
        }

        public List<dynamic> GetDynamicPageList(string XmlName, int PageIndex, int PageSize, out int TotalPage, object Params = null)
        {
            string ExtendParams = "";
            if (Params != null)
            {
                var types = Params.GetType();
                PropertyInfo[] PropertyList = types.GetProperties();
                foreach (var item in PropertyList)
                {
                    string Name = item.Name;
                    var Value = item.GetValue(Params, null);
                    //if (Value == null) { continue; }
                    ExtendParams += "&" + Name + "=" + Value.ToString();
                }
            }

            string StrParams = string.Format("?xmlName={0}&page={1}&rows={2}", XmlName, PageIndex, PageSize);
            StrParams = StrParams + ExtendParams;

            string url = string.Format(@"http://{0}/api/Mms/Home/GetCommonDialogData{1}", Program.API, StrParams);

            var result = Helpers.HttpHelper.GetTByUrl<dynamic>(url);
            List<dynamic> allRows = result.rows.ToObject<List<dynamic>>();
            int total = result.total;
            TotalPage = total;
            return allRows;
        }

        private void dgvProjectPart_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                string PartCode = dgvProjectPart.Rows[e.RowIndex].Cells["PartCode"].Value.ToString();
                ProcessBomList.PartCode = PartCode;
                Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string ContractCode = txtContractCode.Text;
            string Model = txtModel.Text;
            string PartFigureCode = txtPartFigureCode.Text;
            string ProjectName = txtProjectName.Text;
            string Spec = txtSpec.Text;
            int TotalPage = 0;
            var list = GetDynamicPageList("ChooseProjectPart", 1, PageSize, out TotalPage,
                new
                {
                    ContractCode = ContractCode,
                    ProjectName = ProjectName,
                    Model = Model,
                    PartFigureCode = PartFigureCode,
                    Spec = Spec
                }).Select(a => new
                {
                    ContractCode = a.ContractCode,
                    ProjectName = a.ProjectName,
                    ProductName = a.ProductName,
                    ProductType = a.ProductType,
                    taskText = a.taskText,
                    Model = a.Model,
                    Specifications = a.Specifications,
                    Quantity = a.Quantity,
                    PartFigureCode = a.PartFigureCode,
                    TaskDescription = a.TaskDescription,
                    PartName = a.PartName,
                    PartCode = a.PartCode
                }).ToList();
            dgvProjectPart.DataSource = list;


            PageTotal.Text = Math.Ceiling(Convert.ToDouble(TotalPage) / Convert.ToDouble(PageSize)).ToString();
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            int PageIndex = Convert.ToInt32(PageInfo.Text);
            if (PageIndex == 1)
            {
                MessageBox.Show("已经是第一页了!");
            }
            else
            {
                PageInfo.Text = (PageIndex - 1).ToString();

                string ContractCode = txtContractCode.Text;
                string Model = txtModel.Text;
                string PartFigureCode = txtPartFigureCode.Text;
                string ProjectName = txtProjectName.Text;
                string Spec = txtSpec.Text;
                int TotalPage = 0;
                var list = GetDynamicPageList("ChooseProjectPart", PageIndex - 1, PageSize, out TotalPage,
                    new
                    {
                        ContractCode = ContractCode,
                        ProjectName = ProjectName,
                        Model = Model,
                        PartFigureCode = PartFigureCode,
                        Spec = Spec
                    }).Select(a => new
                    {
                        ContractCode = a.ContractCode,
                        ProjectName = a.ProjectName,
                        ProductName = a.ProductName,
                        ProductType = a.ProductType,
                        taskText = a.taskText,
                        Model = a.Model,
                        Specifications = a.Specifications,
                        Quantity = a.Quantity,
                        PartFigureCode = a.PartFigureCode,
                        TaskDescription = a.TaskDescription,
                        PartName = a.PartName,
                        PartCode = a.PartCode
                    }).ToList();
                dgvProjectPart.DataSource = list;
            }
        }

        private void btnAfterPage_Click(object sender, EventArgs e)
        {
            int PageIndex = Convert.ToInt32(PageInfo.Text);
            int PageTotal1 = Convert.ToInt32(PageTotal.Text);
            if (PageIndex + 1 > PageTotal1)
            {
                MessageBox.Show("已经是最后一页了!");
            }
            else
            {
                PageInfo.Text = (PageIndex + 1).ToString();
                string ContractCode = txtContractCode.Text;
                string Model = txtModel.Text;
                string PartFigureCode = txtPartFigureCode.Text;
                string ProjectName = txtProjectName.Text;
                string Spec = txtSpec.Text;
                int TotalPage = 0;
                var list = GetDynamicPageList("ChooseProjectPart", PageIndex + 1, PageSize, out TotalPage,
                    new
                    {
                        ContractCode = ContractCode,
                        ProjectName = ProjectName,
                        Model = Model,
                        PartFigureCode = PartFigureCode,
                        Spec = Spec
                    }).Select(a => new
                    {
                        ContractCode = a.ContractCode,
                        ProjectName = a.ProjectName,
                        ProductName = a.ProductName,
                        ProductType = a.ProductType,
                        taskText = a.taskText,
                        Model = a.Model,
                        Specifications = a.Specifications,
                        Quantity = a.Quantity,
                        PartFigureCode = a.PartFigureCode,
                        TaskDescription = a.TaskDescription,
                        PartName = a.PartName,
                        PartCode = a.PartCode
                    }).ToList();
                dgvProjectPart.DataSource = list;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtContractCode.Text = "";
            txtModel.Text = "";
            txtPartFigureCode.Text = "";
            txtProjectName.Text = "";
            txtSpec.Text = "";
            btnSearch_Click(new object(), new EventArgs());
        }
    }

    public class RootPartModel
    {
        public string ContractCode { get; set; }
        public string ProjectName { get; set; }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string taskText { get; set; }
    }
}
