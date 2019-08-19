using MesWpfClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesWpfClient.Models
{
    public class MaintainPlanModel : ViewModelBase
    {
        private string equipmentName;

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get
            {
                return equipmentName;
            }
            set
            {
                equipmentName = value;
                OnPropertyChanged(nameof(EquipmentName));
            }
        }

        private string maintainInfo;

        /// <summary>
        /// 保养信息
        /// </summary>
        public string MaintainInfo
        {
            get
            {
                return maintainInfo;
            }
            set
            {
                maintainInfo = value;
                OnPropertyChanged(nameof(MaintainInfo));
            }
        }

        private string planedContent;

        /// <summary>
        /// 保养内容
        /// </summary>
        public string PlanedContent
        {
            get
            {
                return planedContent;
            }
            set
            {
                planedContent = value;
                OnPropertyChanged(nameof(PlanedContent));
            }
        }

        private string planDate;

        /// <summary>
        /// 保养时间
        /// </summary>
        public string PlanDate
        {
            get
            {
                return planDate;
            }
            set
            {
                planDate = value;
                OnPropertyChanged(nameof(PlanDate));
            }
        }
    }
}
