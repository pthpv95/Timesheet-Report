using System;

namespace TimesheetReport.WebUI.ViewModels.Equipment
{
    public class MyEquipmentViewModel
    {
        public MyEquipmentViewModelItem [] Items { get; set; }
    }

    public class MyEquipmentViewModelItem
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime AssignOn { get; set; }

        public string AssignBy { get; set; }
    }
}