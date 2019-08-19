using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace MesWpfClient.Common
{
    public class CsBarCodePrint
    {
        //1打开打印机的连接(需要连接的打印机名称)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_OpenPrinter")]
        public static extern int PRN_OpenPrinter(string printername);

        //2追加需要打印的文本（需要打印的字符串，打印起点x，打印起点y）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_AddTextToLabel")]
        public static extern int PRN_AddTextToLabel(string textToPrint, int posX, int posY);

        //3追加需打印的文本(可设定字体与旋转)(字符串/字体名/字体大小/x/y/文本方向/对准基点)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_AddTextToLabelEx")]
        public static extern int PRN_AddTextToLabelEx(string textToPrint, string fontName, int fontSize, int posX, int posY, int dir, int align);

        //4设定条码解释文本的字体
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_IndBarcodeFont")]
        public static extern int PRN_IndBarcodeFont(string fontName, int size, int slant, int offset);

        //5追加需打印的条码（条码字符串/条码类型/条码高度/x/y/文本方向/对准基点/放大倍数）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_AddBarcodeToLabel")]
        public static extern int PRN_AddBarcodeToLabel(string codeToPrint, string codeType, int height, int posX, int posY, int dir, int align, int enlargeWidth);

        //6追加需打印的图片(图片路径)
        [DllImport("HoneywellPrint.dll", EntryPoint = " PRN_AddImageToLabel")]
        public static extern int PRN_AddImageToLabel(string imgFilePath, int posX, int posY);

        //7追加需打印的图片(可设定旋转)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_AddImageToLabelEx")]
        public static extern int PRN_AddImageToLabelEx(string imgFilePath, int posX, int posY, int dir, int align);

        //8清除已追加的需打印内容
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_ClearLabelBuffer")]
        public static extern int PRN_ClearLabelBuffer();

        //9打印已追加的需打印内容（打印份数）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_PrintLabel")]
        public static extern int PRN_PrintLabel(int numOfCopies);

        //10查询打印机状态
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_GetStatus")]
        public static extern int PRN_GetStatus(int piPrinterStatus);
        //10查询打印机状态
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_GetStatus")]
        public static extern int PRN_GetStatus();

        //11关闭打印机连接
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_ClosePrinter")]
        public static extern int PRN_ClosePrinter();

        //12设定打印机使用的纸种类(0.黑标纸/1.间隙纸/2.连续固定长纸/3.连续可变长纸)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetMediaType")]
        public static extern int PRN_SetMediaType(int mtype);

        //13设定打印机打印模式（0.热敏/1.热转印）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetPrintMethod")]
        public static extern int PRN_SetPrintMethod(int method);

        //14设定打印机用纸边距（边距值）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetMediaMarginX")]
        public static extern int PRN_SetMediaMarginX(int margin);

        //15设定打印机用纸的纸宽（宽度）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetMediaWidth")]
        public static extern int PRN_SetMediaWidth(int width);

        //16设定打印机用纸的纸长（长度）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetMediaLength")]
        public static extern int PRN_SetMediaLength(int length);

        //17设定打印机打印起始位置调整值(起始位置调整值)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetStartAdjust")]
        public static extern int PRN_SetStartAdjust(int startAdjust);

        //18设定打印机打印停止位置调整值(停止位置调整值)
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetStopAdjust")]
        public static extern int PRN_SetStopAdjust(int stopAdjust);

        //19设定打印机纸张调整模式（0.慢速/1.快速）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetMediaCalibrateMode")]
        public static extern int PRN_SetMediaCalibrateMode(int mode);

        //20设定打印机开机时的动作（0.无/1.进纸/2.进纸测试）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetPowerUpAction")]
        public static extern int PRN_SetPowerUpAction(int actionType);

        //21设定打印头合上时的动作（0.无/1.进纸/2.进纸测试）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetHeadDownAction")]
        public static extern int PRN_SetHeadDownAction(int actionType);

        //22设定打印机打印速度（0.50/1.70/2.100）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetPrintSpeed")]
        public static extern int PRN_SetPrintSpeed(int speedMode);

        //23设定打印机打印浓度（1-100）
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SetDarkness")]
        public static extern int PRN_SetDarkness(int darkness);

        //24执行需更改(已调用 PRN_Set*)的所有打印设定
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_WriteConfig")]
        public static extern int PRN_WriteConfig();

        //25发送数据到打印机，用于未封装成 API 的打印语言指令等
        [DllImport("HoneywellPrint.dll", EntryPoint = "PRN_SendBuffer")]
        public static extern int PRN_SendBuffer(string data);

        /// <summary>
        /// 产品信息打印模板
        /// </summary>
        /// <param name="PrintName">打印机名称</param>
        /// <param name="PointX">起始点X坐标</param>
        /// <param name="PointY">起始点Y坐标</param>
        /// <param name="ContractName">合同</param>
        /// <param name="ProductName">产品</param>
        /// <param name="PartCode">零件图号</param>
        /// <param name="PartName">零件名称</param>
        /// <param name="MaterialName">材质</param>
        /// <param name="BarCode">条码</param>
        /// <returns></returns>
        public static bool BarCodePrint(string PrintName, int PointX, int PointY, string ContractName, string ProductName, string PartCode, string PartName, string MaterialName, string BarCode)
        {
            int rtn = 0;
            rtn = PRN_OpenPrinter(PrintName); //打开指定名称的打印机链接
            rtn = PRN_SendBuffer("NASC 936 \r\n");        //选择中文字符集
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (420 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (420 + PointY), 1, 1);

            //第一行内容
            rtn = PRN_AddTextToLabelEx("合同", "MHeiGB18030C-Medium", 10, (170 + PointX), (390 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(ContractName, "MHeiGB18030C-Medium", 10, (300 + PointX), (390 + PointY), 1, 1);

            //表格横线一
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (370 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (370 + PointY), 1, 1);

            //第二行内容
            rtn = PRN_AddTextToLabelEx("产品", "MHeiGB18030C-Medium", 10, (170 + PointX), (340 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(ProductName, "MHeiGB18030C-Medium", 10, (300 + PointX), (340 + PointY), 1, 1);

            //表格横线二
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (320 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (320 + PointY), 1, 1);

            //第三行内容
            rtn = PRN_AddTextToLabelEx("零件图号", "MHeiGB18030C-Medium", 10, (170 + PointX), (290 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(PartCode, "MHeiGB18030C-Medium", 10, (300 + PointX), (290 + PointY), 1, 1);

            //表格横线三
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (270 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (270 + PointY), 1, 1);

            //第四行内容
            rtn = PRN_AddTextToLabelEx("零件名称", "MHeiGB18030C-Medium", 10, (170 + PointX), (240 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(PartName, "MHeiGB18030C-Medium", 10, (300 + PointX), (240 + PointY), 1, 1);

            //表格横线四
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (220 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (220 + PointY), 1, 1);

            //第五行内容
            rtn = PRN_AddTextToLabelEx("材质", "MHeiGB18030C-Medium", 10, (170 + PointX), (190 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(MaterialName, "MHeiGB18030C-Medium", 10, (300 + PointX), (190 + PointY), 1, 1);

            //表格横线五 
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (170 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (170 + PointY), 1, 1);

            //下边框线
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (0 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (0 + PointY), 1, 1);

            //左边框线
            rtn = PRN_AddTextToLabelEx("-----------------------------------------------", "MHeiGB18030C-Medium", 10, (150 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("----------------------------------------------", "MHeiGB18030C-Medium", 10, (150 + PointX), (423 + PointY), 2, 1);

            //表格竖线一
            rtn = PRN_AddTextToLabelEx("----------------------------", "MHeiGB18030C-Medium", 10, (280 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("---------------------------", "MHeiGB18030C-Medium", 10, (280 + PointX), (423 + PointY), 2, 1);

            //右边框线
            rtn = PRN_AddTextToLabelEx("-----------------------------------------------", "MHeiGB18030C-Medium", 10, (715 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("----------------------------------------------", "MHeiGB18030C-Medium", 10, (715 + PointX), (423 + PointY), 2, 1);

            //条码
            rtn = PRN_AddBarcodeToLabel(BarCode, "CODE39", 90, (220 + PointX), (70 + PointY), 1, 1, 2);
            rtn = PRN_AddTextToLabelEx(BarCode, "MHeiGB18030C-Medium", 10, (350 + PointX), (30 + PointY), 1, 1);
            rtn = PRN_PrintLabel(1);
            rtn = PRN_ClearLabelBuffer();
            rtn = PRN_ClosePrinter(); //关闭打印机链接
            return true;
        }

        /// <summary>
        /// 人员信息打印模板
        /// </summary>
        /// <param name="PrintName">打印机名称</param>
        /// <param name="PointX">起始点X坐标</param>
        /// <param name="PointY">起始点Y坐标</param>
        /// <param name="PersonName">姓名</param>
        /// <param name="PersonCode">员工编号</param>
        /// <param name="DepartmentName">部门</param>
        /// <param name="BarCode">条码</param>
        /// <returns></returns>
        public static bool PersonBarCodePrint(string PrintName, int PointX, int PointY, string PersonName, string PersonCode, string DepartmentName, string BarCode)
        {
            int rtn = 0;
            rtn = PRN_OpenPrinter(PrintName); //打开指定名称的打印机链接                      
            rtn = PRN_SendBuffer("NASC 936 \r\n");        //选择中文字符集
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (420 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (420 + PointY), 1, 1);

            //第一行内容
            rtn = PRN_AddTextToLabelEx("姓名", "MHeiGB18030C-Medium", 10, (170 + PointX), (390 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(PersonName, "MHeiGB18030C-Medium", 10, (300 + PointX), (390 + PointY), 1, 1);

            //表格横线一
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (370 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (370 + PointY), 1, 1);

            //第二行内容
            rtn = PRN_AddTextToLabelEx("员工编号", "MHeiGB18030C-Medium", 10, (170 + PointX), (340 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(PersonCode, "MHeiGB18030C-Medium", 10, (300 + PointX), (340 + PointY), 1, 1);

            //表格横线二
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (320 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (320 + PointY), 1, 1);

            //第三行内容
            rtn = PRN_AddTextToLabelEx("部门", "MHeiGB18030C-Medium", 10, (170 + PointX), (290 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx(DepartmentName, "MHeiGB18030C-Medium", 10, (300 + PointX), (290 + PointY), 1, 1);

            //表格横线三
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (270 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (270 + PointY), 1, 1);

            //下边框线
            rtn = PRN_AddTextToLabelEx("---------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (160 + PointX), (0 + PointY), 1, 1);
            rtn = PRN_AddTextToLabelEx("--------------------------------------------------------------", "MHeiGB18030C-Medium", 10, (167 + PointX), (0 + PointY), 1, 1);

            //左边框线
            rtn = PRN_AddTextToLabelEx("-----------------------------------------------", "MHeiGB18030C-Medium", 10, (150 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("----------------------------------------------", "MHeiGB18030C-Medium", 10, (150 + PointX), (423 + PointY), 2, 1);

            //表格竖线一
            rtn = PRN_AddTextToLabelEx("-----------------", "MHeiGB18030C-Medium", 10, (280 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("----------------", "MHeiGB18030C-Medium", 10, (280 + PointX), (423 + PointY), 2, 1);

            //右边框线
            rtn = PRN_AddTextToLabelEx("-----------------------------------------------", "MHeiGB18030C-Medium", 10, (715 + PointX), (430 + PointY), 2, 1);
            rtn = PRN_AddTextToLabelEx("----------------------------------------------", "MHeiGB18030C-Medium", 10, (715 + PointX), (423 + PointY), 2, 1);

            //条码
            rtn = PRN_AddBarcodeToLabel(BarCode, "CODE39", 90, (220 + PointX), (120 + PointY), 1, 1, 2);
            rtn = PRN_AddTextToLabelEx(BarCode, "MHeiGB18030C-Medium", 10, (350 + PointX), (80 + PointY), 1, 1);
            rtn = PRN_PrintLabel(1);
            rtn = PRN_ClearLabelBuffer();
            rtn = PRN_ClosePrinter(); //关闭打印机链接
            return true;
        }
    }
}
