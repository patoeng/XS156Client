using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace XS156Client35.Models
{
    [Guid("C3A6E913-CFDD-4F18-AD07-8BC34D50809E")]
    [StructLayout(LayoutKind.Auto ,CharSet = CharSet.Ansi)]
    public  class EquipmentInt
    {
        
        public string Id;
       
        public string EquipmentName;
        
        public string Description;
       
        public ushort Role;
     
        public string PreviousEquipment;
       
        public string EquipmentLineGroup;
       
        public ushort Status;
    }
}
