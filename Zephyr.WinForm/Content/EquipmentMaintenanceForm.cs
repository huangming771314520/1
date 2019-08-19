using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zephyr.WinForm.Content
{
    public partial class EquipmentMaintenanceForm : Form
    {

        private Models.EquipmentMaintenancePlanModel.DataModel.EMaintenancePlanModel EquipmentMaintenancePlan { get; set; }

        public EquipmentMaintenanceForm()
        {
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }

        public EquipmentMaintenanceForm(Models.EquipmentMaintenancePlanModel.DataModel.EMaintenancePlanModel _equipmentMaintenancePlan)
        {
            EquipmentMaintenancePlan = _equipmentMaintenancePlan;
            InitializeComponent();

            Icon = Properties.Resources.favicon_32;
        }

        private void EquipmentMaintenanceForm_Load(object sender, EventArgs e)
        {
            lblEquipmentName.Text = EquipmentMaintenancePlan.EquipmentName;
            lblMaintenancePlanCode.Text = EquipmentMaintenancePlan.MaintenancePlanCode;
            lblMaintenanceName.Text = string.Format(@"设备正在 {0} 中...", EquipmentMaintenancePlan.MaintenanceState);
            lblPlanedStartTime.Text = EquipmentMaintenancePlan.PlanedStartTime.ToString();
            lblPlanedFinishTime.Text = EquipmentMaintenancePlan.PlanedFinishTime.ToString();





        }
    }
}
