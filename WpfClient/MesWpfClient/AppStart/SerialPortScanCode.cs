using MesWpfClient.API;
using MesWpfClient.Common;
using MesWpfClient.ViewModels;
using MesWpfClient.Views;
using Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MesWpfClient.AppStart
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
                if (sAllPort.Contains(ConfigInfoModel.SerialPortName))
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
                        PortName = ConfigInfoModel.SerialPortName,
                        BaudRate = 9600,
                        ReceivedBytesThreshold = 1
                    };
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

                    serialPort.Open();
                    readStr = "";
                }
                else
                {
                    MessageBox.Show($"串口【{ConfigInfoModel.SerialPortName}】不存在！", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {

            }
        }

        private void DealWithBarCode(string barCode)
        {
            switch (WindowHelper.CurrentWindow.CurrentPage.GetType().Name)
            {
                //登录页面扫码
                case nameof(PageLogin):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (((WindowHelper.CurrentWindow.CurrentPage as PageLogin).DataContext) as LoginViewModel).Login(barCode);
                    }));
                    break;
                //计划执行人员记录页面扫码
                case nameof(PageOperatorStatistical):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        (((WindowHelper.CurrentWindow.CurrentPage as PageOperatorStatistical).DataContext) as OperatorStatisticalViewModel).BarCodeScan(barCode);
                    }));
                    break;
                case nameof(PageProduceProce):
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (WindowHelper.IsOpenMaterialScanWindow)
                        {

                            (WindowHelper.MaterialScanWindow.DataContext as MaterialScanViewModel).BarCodeScan(barCode);
                        }
                        else
                        {


                            (((WindowHelper.CurrentWindow.CurrentPage as PageProduceProce).DataContext) as ProduceProceViewModel).QrCodeScan(barCode);
                        }
                    }));
                    break;
                default:

                    break;
            }

        }


    }
}
