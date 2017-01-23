using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XS156Client35.Helper;

namespace XS156Client35.Models
{
    public class TrackingData
    {
        public static TrackingData LoadByLineGroup(string equipmentGroupId)
        {
            var setting = new Xs156Setting();
            var getTrackingData = Connections<TrackingData>.Get(setting.ServerBaseUri()+ "api/tracking/getByLineGroup/" + equipmentGroupId);
            return  (TrackingData)  getTrackingData;
        }
        public static TrackingData LoadLastByLineGroup(string equipmentGroupId)
        {
            var setting = new Xs156Setting();
            var getTrackingData = Connections<TrackingData>.Get(setting.ServerBaseUri() + "api/tracking/getLastByLineGroup/" + equipmentGroupId);
            return (TrackingData)getTrackingData;
        }
        public static TrackingData LoadByProcessId(string processId)
        {
            var setting = new Xs156Setting();
            var getTrackingData = Connections<TrackingData>.Get(setting.ServerBaseUri() + "api/tracking/" + processId);
            return (TrackingData)getTrackingData;
        }

        public static TrackingData CreateTrackingData(ReferenceProcess referenceProcess, Equipment initiatorEquipment)
        {
            var setting = new Xs156Setting();
           // if (initiatorEquipment.Role == (ushort) EquipmentRole.Initiator)
           // {
            var newTrack = (TrackingData)Connections<TrackingData>.Post(setting.ServerBaseUri() + "api/tracking/" + initiatorEquipment.Id,
                    referenceProcess);
            return newTrack;
                //(TrackingData)Connections<TrackingData>.Get(setting.ServerBaseUri() + "api/tracking/"+newTrack);
            // }
            // return null;
        }
        public TrackingData Refresh()
        {
            var setting = new Xs156Setting();
            var getTrackingData = Connections<TrackingData>.Get(setting.ServerBaseUri() + "api/tracking/" + ReferenceProcess.Id);
            return (TrackingData)getTrackingData;
        }

        public TrackingDataBag GetEquipmentStatusBagOfProcess(string processId, Equipment equipment)
        {
            var data = LoadByProcessId(new Guid(processId).ToString());
            var guid = new Guid(processId);
            var currentProcess = data.EquipmentReferenceProcesses;

            int j = 0;
            var i = -1;
            var equipmentReferenceProcesses = currentProcess as EquipmentReferenceProcess[] ?? currentProcess.ToArray();
            if (currentProcess!=null)
            {
                i = 0;
                foreach (var data2 in equipmentReferenceProcesses)
                {

                    if (data2.Equipment.ToString() == equipment.Id)
                    {
                        j = i;
                    }
                    i++;
                }
            }

            if (i > -1)
            {
                var bag = new TrackingDataBag
                {
                    CurrentEquipmentProcessIdentity = equipmentReferenceProcesses[j].Id.ToString(),
                    CurrentReference = data.ProductReference.Id.ToString(),
                    CurrentReferenceName = data.ProductReference.ReferenceName,
                    EquipmentIdentity = equipment.Id,
                    EquipmentLineGroupIdentity = data.ReferenceProcess.LineGroup.ToString(),
                    OutputQuantity = equipmentReferenceProcesses[j].OutputQuantity,
                    PreviousEquipmentIdentity = equipment.PreviousEquipment,
                    ProcessableQuantity = equipmentReferenceProcesses[j].ProcessAbleQuantity,
                    RejectedQuantity = equipmentReferenceProcesses[j].RejectedQuantity,
                    TargetQuantity = equipmentReferenceProcesses[j].TargetQuantity,
                    TrackingIdentity = data.ReferenceProcess.ProcessGuid.ToString(),
                    TrackingStatus = Convert.ToInt16(data.ReferenceProcess.IsClosed),
                    StartDateTime = data.ReferenceProcess.StartDateTime.ToString("s"),
                    EndDateTime = data.ReferenceProcess.EndDateTime.ToString("s"),
                    Completed = equipmentReferenceProcesses[j].Completed,
                    OrderNumber = data.ReferenceProcess.OrderNumber
                };


                return bag;
            }
            return new TrackingDataBag();
        }
        public void Close()
        {
            var setting = new Xs156Setting();
            Connections<TrackingData>.Put(setting.ServerBaseUri() + "api/tracking/close/" + ReferenceProcess.Id);
        }
        #region Private Properties
        
        #endregion
        #region Properties
        public ProductReference ProductReference { get;  set; }
        public IEnumerable<EquipmentReferenceProcess> EquipmentReferenceProcesses { get; set; }
        public ReferenceProcess ReferenceProcess { get; set; }
        #endregion

        public static TrackingData LoadByOrderNumber(string ordernumber)
        {
            try
            {
                var setting = new Xs156Setting();
                var getTrackingData =
                    Connections<TrackingData>.Get(setting.ServerBaseUri() + "api/tracking/GetByOrderNumber/" +
                                                  ordernumber);
                return (TrackingData) getTrackingData;
            }
            catch (Exception exception)
            {
                return new TrackingData();
            }
        }
    }
}