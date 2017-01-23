using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using XS156Client35.Helper;


namespace XS156Client35.Models
{
    [ComVisible(false)]
    public class Equipment : IEquatable<Equipment>
    {
        public const string GetPartialUri = "api/equipment/";
        #region Property
        public string Id { get; set; }
        public string EquipmentName { get; set; }

        public string Description { get; set; }

        public ushort Role { get; set; }

        public string PreviousEquipment { get; set; }

        public string EquipmentLineGroup { get; set; }
        public ushort Status { get; set; }
        #endregion
        public static Equipment Load(string guid)
        {
            var j = new Xs156Setting();
            return (Equipment) Connections<Equipment>.Get(j.ServerBaseUri()+GetPartialUri+guid);
        }
        public static Equipment Save(string guid)
        {
            return (Equipment)Connections<Equipment>.Post(guid);
        }
       
        public void RegisterToLineGroup(EquipmentLineGroup equipmentLineGroup)
        {
            EquipmentLineGroup = equipmentLineGroup.Id.ToString();
        }

        public void ChangeEquipmentStatus(EquipmentStatus equipmentStatus)
        {
            Status = (ushort) equipmentStatus;
        }

        public void SetDescriptions(string descriptions)
        {
            Description = descriptions;
        }

        public bool Equals(Equipment other)
        {
            return Id == other.Id;
        }

        public override string ToString()
        {
            return Id;
        }

       
    }
}