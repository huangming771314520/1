using Fleck;
using Newtonsoft.Json;
using PrintBarCodeService.Common;
using PrintBarCodeService.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace PrintBarCodeService.WinService
{
    public class PrintService
    {
        public void Start()
        {
            int offsetX = 0, offsetY = 0;
            string PrintMachineName = ConfigurationManager.AppSettings["PrintMachineName"].ToString();
            int.TryParse(ConfigurationManager.AppSettings["Offset_X"].ToString(), out offsetX);
            int.TryParse(ConfigurationManager.AppSettings["Offset_Y"].ToString(), out offsetY);

            //默认打码机名称
            var defaultPrintName = string.Empty;

            if (string.IsNullOrEmpty(PrintMachineName))
            {
                foreach (string item in PrinterSettings.InstalledPrinters)
                {
                    if (item.ToLower().Contains("honeywell"))
                    {
                        defaultPrintName = item;
                        break;
                    }
                }
            }
            else
            {
                defaultPrintName = PrintMachineName;
            }

            try
            {
                var server = new WebSocketServer("ws://0.0.0.0:45000");
                server.Start(socket =>
                {
                    socket.OnOpen = () =>
                    {
                        socket.Send(JsonConvert.SerializeObject(new
                        {
                            code = "info",
                            msg = "socket通讯已连接！"
                        }));
                    };
                    socket.OnClose = () =>
                    {
                        socket.Send(JsonConvert.SerializeObject(new
                        {
                            code = "info",
                            msg = "socket通讯已关闭！"
                        }));
                        socket.Close();
                    };
                    socket.OnMessage = message =>
                    {
                        if (string.IsNullOrEmpty(defaultPrintName))
                        {
                            socket.Send(JsonConvert.SerializeObject(new
                            {
                                code = "error",
                                msg = "未检测到打印机驱动！"
                            }));
                            return;
                        }

                        if (message.Contains("print_"))
                        {
                            string json = message.Replace("print_", "");

                            PrintInfoEntity printInfo = JsonConvert.DeserializeObject<PrintInfoEntity>(json);

                            if (printInfo.PrintType.Equals(PrintTypeEnum.PersonPrint))
                            {
                                List<PersonPrintInfoEntity> items = JsonConvert.DeserializeObject<List<PersonPrintInfoEntity>>(printInfo.Data.ToString());
                                items.ForEach(i =>
                                {
                                    CsBarCodePrint.PersonBarCodePrint(defaultPrintName, offsetX, offsetY, i.PersonName ?? "", i.PersonCode ?? "", i.DeptName ?? "", i.BarCode ?? "");
                                });
                            }
                            if (printInfo.PrintType.Equals(PrintTypeEnum.PartPrint))
                            {
                                List<PartPrintInfoEntity> items = JsonConvert.DeserializeObject<List<PartPrintInfoEntity>>(printInfo.Data.ToString());
                                items.ForEach(i =>
                                {
                                    CsBarCodePrint.PartBarCodePrint(defaultPrintName, offsetX, offsetY, i.ContractName ?? "", i.ProductName ?? "", i.PartFigureNo ?? "", i.PartName ?? "", i.MaterialName ?? "", i.BarCode ?? "");
                                });
                            }

                            socket.Send(JsonConvert.SerializeObject(new
                            {
                                code = "prompt",
                                msg = "打印信息已推送！"
                            }));
                        }

                    };
                });
            }
            catch
            {

            }

        }

        public void Stop()
        {

        }
    }
}
