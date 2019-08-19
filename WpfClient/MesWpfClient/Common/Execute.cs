using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MesWpfClient.Common
{
    public static class Execute
    {
        private static Action<Action> executor = action => action();
        /// <summary>
        /// 初始化UI调度器
        /// </summary>
        public static void InitializeWithDispatcher()
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            executor = action =>
            {
                if (dispatcher.CheckAccess())
                    action();
                else dispatcher.BeginInvoke(action);
            };
        }
        /// <summary>
        /// UI线程中执行方法
        /// </summary>
        public static void OnUIThread(this Action action)
        {
            executor(action);
        }
    }
}
