using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace XS156Client35.Models
{
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Ansi)]
    [Guid("300F38FA-C1BD-457F-8513-463E491981C4")]
    public class TrackingDataBag
    {
        public string TrackingIdentity;
        public int TrackingStatus;
        public string CurrentEquipmentProcessIdentity;
        public string CurrentReference;
        public string EquipmentIdentity;
        public string EquipmentLineGroupIdentity;
        public string PreviousEquipmentIdentity;
        public int TargetQuantity;
        public int OutputQuantity;
        public int RejectedQuantity;
        public int ProcessableQuantity;
        public string CurrentReferenceName;
        public string LineGroupName;
        public string StartDateTime;
        public string EndDateTime;
        public bool Completed;
        public string OrderNumber;
    }
}
