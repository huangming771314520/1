using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MesClient.Common
{
    public class SimulationUserCommon
    {
        [DllImport("user32.dll")]
        private static extern int mouse_event(MouseEventFlag dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        [Flags]
        private enum MouseEventFlag : uint
        {
            /// <summary>
            /// 移动鼠标
            /// </summary>
            Move = 0x0001,
            /// <summary>
            /// 模拟鼠标左键按下
            /// </summary>
            LeftDown = 0x0002,
            /// <summary>
            /// 模拟鼠标左键抬起
            /// </summary>
            LeftUp = 0x0004,
            /// <summary>
            /// 模拟鼠标右键按下
            /// </summary>
            RightDown = 0x0008,
            /// <summary>
            /// 模拟鼠标右键抬起
            /// </summary>
            RightUp = 0x0010,
            /// <summary>
            /// 模拟鼠标中键按下
            /// </summary>
            MiddleDown = 0x0020,
            /// <summary>
            /// 模拟鼠标中键抬起
            /// </summary>
            MiddleUp = 0x0040,
            /// <summary>
            /// 
            /// </summary>
            XDown = 0x0080,
            /// <summary>
            /// 
            /// </summary>
            XUp = 0x0100,
            /// <summary>
            /// 模拟鼠标滚轮滚动
            /// </summary>
            Wheel = 0x0800,
            /// <summary>
            /// 
            /// </summary>
            VirtualDesk = 0x4000,
            /// <summary>
            /// 标示是否采用绝对坐标
            /// </summary>
            Absolute = 0x8000
        }

        public static void MouseSimulateWheelEvent(int value)
        {
            int pointX = ConfigInfo.WinScreenWidth / 2;
            int pointY = ConfigInfo.WinScreenHeight / 2;
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Move, pointX * 65536 / ConfigInfo.WinScreenWidth, pointY * 65536 / ConfigInfo.WinScreenHeight, 0, 0);
            mouse_event(MouseEventFlag.Absolute | MouseEventFlag.Wheel, pointX * 65536 / ConfigInfo.WinScreenWidth, pointY * 65536 / ConfigInfo.WinScreenHeight, value, 0);
        }

    }
}
