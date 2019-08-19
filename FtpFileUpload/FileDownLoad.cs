using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FtpFileUpload
{
    public partial class FileDownLoad : Form
    {
        public string FileAddress { get; set; }
        public string FileName { get; set; }
        public string DownType { get; set; }
        public FileDownLoad(string file_address, string file_name,string down_type)
        {
            this.FileAddress = file_address;
            this.FileName = file_name;
            this.DownType = down_type;
            InitializeComponent();
        }

        private void FileDownLoad_Load(object sender, EventArgs e)
        {
            int Index = 0;
            var FileNameList = FileName.Split('|');
            var FileAddressList = FileAddress.Split('|');
            for (int i = 0; i < FileAddressList.Length; i++)
            {
                string FileName = FileNameList[i];
                string FileAddress = FileAddressList[i];
                DataGridViewRow dgr = new DataGridViewRow();

                DataGridViewTextBoxCell C1 = new DataGridViewTextBoxCell();
                C1.Value = Index + 1;

                DataGridViewTextBoxCell C2 = new DataGridViewTextBoxCell();
                C2.Value = FileName;

                DataGridViewLinkCell C3 = new DataGridViewLinkCell();
                C3.Value = "下载";

                DataGridViewLinkCell C4 = new DataGridViewLinkCell();
                C4.Value = FileAddress;

                Index++;

                dgr.Cells.Add(C1);
                dgr.Cells.Add(C2);
                dgr.Cells.Add(C3);
                dgr.Cells.Add(C4);

                dgv_file_list.Rows.Add(dgr);
            }
        }

        private void dgv_file_list_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 0 && e.RowIndex >= 0)
            {
                string header_text = dgv_file_list.Columns[e.ColumnIndex].HeaderText;
                if (header_text.Equals("下载"))
                {
                    var FileAddress_ftp = dgv_file_list.Rows[e.RowIndex].Cells["FileAddress_ftp"].Value.ToString();
                    var FileNames = dgv_file_list.Rows[e.RowIndex].Cells["FileNames"].Value.ToString();

                    DownLoadForm down = new DownLoadForm(FileAddress_ftp, FileNames, DownType);
                    down.StartPosition = FormStartPosition.CenterScreen;
                    down.ShowDialog();

                }
            }
        }
    }
}
