using MesClient.API;
using MesClient.Common;
using MesClient.ViewModels;
using MesClient.Views;
using System;
using System.IO.Ports;
using System.Linq;
using System.Windows;

namespace MesClient.AppStart
{
    public class SerialPortScanCode
    {
        private static SerialPort serialPort;
        private string readStr = string.Empty;

        public SerialPortScanCode()
        {

        }

        public void Start()
        {
            try
            {
                string[] sAllPort = SerialPort.GetPortNames();
                if (sAllPort.Contains(ConfigInfo.SerialPortName))
                {
                    if (serialPort != null)
                    {
                        serialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_DataReceived);
                        if (serialPort.IsOpen)
                        {
                            serialPort.Close();
                        }
                        serialPort.Dispose();
                        serialPort = null;
                    }

                    serialPort = new SerialPort()
                    {
                        PortName = ConfigInfo.SerialPortName,
                        BaudRate = 9600,
                        ReceivedBytesThreshold = 1
                    };
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                    serialPort.Open();
                    readStr = "";
                }
                else
                {
                    MessageBox.Show($"串口【{ConfigInfo.SerialPortName}】不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch
            {
                MessageBox.Show("请检查扫描枪设置！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int n = serialPort.BytesToRead;
                byte[] buf = new byte[n];
                serialPort.Read(buf, 0, n);
                for (int i = 0; i < n; i++)
                {
                    readStr += ConvertCommon.ToStringByASCII(buf[i]).ToUpper();
                }

                string[] LampMacDatab = readStr.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (LampMacDatab.Length >= 3)
                    {
                        for (int i = 1; i < LampMacDatab.Length - 2; i++)
                        {
                            DealWithBarCode(LampMacDatab[i]);
                        }

                        if (readStr.Substring(readStr.Length - 2).Equals("\r\n"))
                        {
                            DealWithBarCode(LampMacDatab[LampMacDatab.Length - 1]);
                        }
                        else
                        {
                            readStr = LampMacDatab[LampMacDatab.Length - 1];
                        }
                    }
                    else
                    {
                        if (LampMacDatab.Length.Equals(0))
                        {
                            //直接返回
                        }
                        else if (LampMacDatab.Length.Equals(1))
                        {
                            if (readStr.Length < 2)
                            {

                            }
                            else
                            {
                                if (readStr.Substring(readStr.Length - 2).Equals("\r\n"))
                                {
                                    DealWithBarCode(LampMacDatab[0]);
                                    readStr = "";
                                }
                                else
                                {

                                }
                            }
                        }
                        else if (LampMacDatab.Length.Equals(2))
                        {
                            DealWithBarCode(LampMacDatab[0]);
                            if (readStr.Substring(readStr.Length - 2).Equals("\r\n"))
                            {
                                DealWithBarCode(LampMacDatab[1]);
                            }
                            else
                            {
                                readStr = LampMacDatab[1];
                            }
                        }
                        else
                        {
                            //这是什么情况
                        }
                    }
                }));

            }
            catch
            {

            }
        }

        private void DealWithBarCode(string barCode)
        {
            switch (WindowHelper.MainViewModel.CurrentPage.GetType().Name)
            {
                //登录页面扫码
                case nameof(PageLogin):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (((WindowHelper.MainViewModel.CurrentPage as PageLogin).DataContext) as LoginViewModel).Login(barCode);
                    }));
                    break;
                //计划执行人员记录页面扫码
                case nameof(PageOperatorRecord):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (((WindowHelper.MainViewModel.CurrentPage as PageOperatorRecord).DataContext) as OperatorRecordViewModel).BarCodeScan(barCode);
                    }));
                    break;
                case nameof(PagePlanExecute):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (WindowHelper.IsOpenMaterialScanWindow)
                        {
                            (WindowHelper.MaterialScanWindow.DataContext as MaterialScanViewModel).BarCodeScan(barCode);
                        }
                        else
                        {
                            if (WindowHelper.WorkShopType.Equals(EnumManager.WorkShopTypeEnum.BlankingShop))
                            {
                                return;
                            }
                            else
                            {
                                ((WindowHelper.MainViewModel.CurrentPage as PagePlanExecute).DataContext as PlanExecuteViewModel).QrCodeScan(barCode);
                            }
                        }
                    }));
                    break;
                default:

                    break;
            }

        }


    }
}
