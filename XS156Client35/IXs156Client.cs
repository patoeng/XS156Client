using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using XS156Client35.Models;

namespace XS156Client35
{
    public delegate void MyEventHandler();

    public delegate void MyEventHandlerWithInfo(string info);

    public delegate void TrackingDataBagUpdated(TrackingDataBag data);

    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("0B45B11D-1C00-4F53-8AF6-4EFCB5CFFF65")]
    public interface IXs156Client
    {
        event MyEventHandlerWithInfo ExceptionEvent;
        event MyEventHandlerWithInfo TrackingProcessClosedEvent;
        event MyEventHandlerWithInfo TrackingProcessCreatedEvent;
        event TrackingDataBagUpdated TrackingDataBagUpdatedEvent;
        event MyEventHandlerWithInfo NewReferenceTrackingEvent;
        event TrackingDataBagUpdated TrackingReferenceNewlyLoaded;
        string ConnectionTest();
        void InvokeEvent();
        EquipmentInt ThisEquipment();
        EquipmentReferenceProcess ThisEquipmentReferenceProcess();
        bool StartNewTrackingProcess(string referenceByName, int target, string orderNumber);
        void CloseTrackingProcess();
        void StopUpdater();
        void StartUpdater();
        int UpdateOutputQuantity(int deltaOutputQuantity);
        int UpdateRejectedQuantity(int deltaRejectedQuantity);
        int GetProcessableQuantity();
        string GetProcessIdentity();
        string GetLineGroupByGuid(Guid id);
        string GetLatestByGuid(Guid guid);
        int SetRejectQuantity(int rejectQuantity);
        int SetOutputQuantity(int outputQuantity);
        void Reload();
        void CompleteCurrentEquipmentProcess();
        bool IsCompleted();
        bool IsBufferedMode();
       bool LoadByOrderNumber(string ordernumber);
    }

    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("D63E8FE8-DD9D-4B0B-9514-FE9A05D44614")]
    public interface IXs156ClientEvents
    {
        void ExceptionEvent(string guid);
        void TrackingProcessClosedEvent(string guid);
        void TrackingProcessCreatedEvent(string guid);
        void TrackingDataBagUpdatedEvent(TrackingDataBag trackingDataBag);
        void  NewReferenceTrackingEvent(string guid);
        void TrackingReferenceNewlyLoaded(TrackingDataBag trackingDataBag);
    }
}
