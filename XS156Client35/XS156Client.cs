using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using XS156Client35.Helper;
using XS156Client35.Models;
using System.Timers;
using Newtonsoft.Json;

namespace XS156Client35
{


    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Xs156Client.Xs156Client")]
    [Guid("EE1E392A-06E1-4EB2-B3AC-C6632A9AD6A3")]
    [ComSourceInterfaces(typeof (IXs156ClientEvents))]
    // [ComSourceInterfacesAttribute("Xs156Client.IXs156ClientEvents")]
    public class Xs156Client : IXs156Client
    {
        [DispId(12)]
        public event MyEventHandlerWithInfo ExceptionEvent;

        public event MyEventHandlerWithInfo TrackingProcessClosedEvent;
        public event MyEventHandlerWithInfo TrackingProcessCreatedEvent;
        public event TrackingDataBagUpdated TrackingDataBagUpdatedEvent;
        public event MyEventHandlerWithInfo NewReferenceTrackingEvent;
        public event TrackingDataBagUpdated TrackingReferenceNewlyLoaded;


        private TrackingData _trackingData;
        private Equipment _thisEquipment;
        private ProductReference _currentReference;
        private EquipmentReferenceProcess _thisEquipmentReferenceProcess;
        private Ticker _scanTicker= new Ticker();
        public TrackingDataBag TrackingDataBag;

        private readonly Xs156Setting _xs156Setting;
        private bool firstLoad;
        private bool _tickerOn;
        private  bool _bufferingMode=false;

        private void Load()
        {
            try
            {
                TrackingDataBag = new TrackingDataBag();
                
                _thisEquipment = Equipment.Load(_xs156Setting.EquipmentIdentity());
                TrackingDataBag.LineGroupName = GetLineGroupByGuid(new Guid(_thisEquipment.EquipmentLineGroup));
                TrackingDataBag.EquipmentIdentity = _thisEquipment.Id;

                _bufferingMode = _xs156Setting.GetBufferingMode();
                if (_bufferingMode)
                {
                    _trackingData = new TrackingData();
                }
                else
                {
                    _trackingData = TrackingData.LoadLastByLineGroup(_thisEquipment.EquipmentLineGroup);
                    _currentReference = _trackingData.ProductReference;
                    _thisEquipmentReferenceProcess =
                        _trackingData.EquipmentReferenceProcesses.First(x => x.Equipment == new Guid(_thisEquipment.Id));
                }
                
              

            }
            catch (Exception ex)
            {
                if (ExceptionEvent != null) ExceptionEvent(ex.Message);
            }
        }

        public void Reload()
        {
            Load();
            firstLoad = false;
        }
        private void TickerElapased(object sender, ElapsedEventArgs e)
        {
            _scanTicker.Enabled = false;
            try
            {
                switch (_bufferingMode)
                {
                    case false: 
                        if (TrackingDataBagUpdatedEvent != null)
                                {
                                    var data =
                                        _trackingData.GetEquipmentStatusBagOfProcess(
                                            _trackingData.ReferenceProcess.ProcessGuid.ToString(),
                                            _thisEquipment);
                                    data.LineGroupName = GetLineGroupByGuid(new Guid(data.EquipmentLineGroupIdentity));
                                    TrackingDataBag = data;
                                    TrackingDataBagUpdatedEvent(TrackingDataBag);
                                }
                                if (TrackingReferenceNewlyLoaded != null &&
                                    (_trackingData.ReferenceProcess.LineGroup != Guid.Empty || firstLoad == true))
                                {
                                    var data2 = new Guid(GetLatestByGuid(_trackingData.ReferenceProcess.LineGroup));
                                    if ((data2 != Guid.Empty) &&
                                        ((data2 != _trackingData.ReferenceProcess.ProcessGuid) || firstLoad))
                                    {
                                        var data =
                                           _trackingData.GetEquipmentStatusBagOfProcess(
                                              data2.ToString(),
                                               _thisEquipment);
                                        data.LineGroupName = GetLineGroupByGuid(new Guid(data.EquipmentLineGroupIdentity));
                                        TrackingDataBag = data;
                                        TrackingReferenceNewlyLoaded(TrackingDataBag);
                                    }
                                }
                                firstLoad = false;
                        break;
                    case true:
                        firstLoad = false;
                        if (_trackingData.ReferenceProcess != null)
                        {
                            if (TrackingDataBagUpdatedEvent != null)
                            {
                                var data =
                                    _trackingData.GetEquipmentStatusBagOfProcess(
                                        _trackingData.ReferenceProcess.ProcessGuid.ToString(),
                                        _thisEquipment);
                                data.LineGroupName = GetLineGroupByGuid(new Guid(data.EquipmentLineGroupIdentity));
                                TrackingDataBag = data;
                                TrackingDataBagUpdatedEvent(TrackingDataBag);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionEvent != null) ExceptionEvent("Tracking : "+ex.StackTrace+" "+ ex.Message);
            }
            _scanTicker.Enabled = _tickerOn;
        }

        public Xs156Client()
        {
            _xs156Setting = new Xs156Setting();
            Load();
            firstLoad = true;
        }
        public Xs156Client(bool bufferingMode)
        {
            _xs156Setting = new Xs156Setting();
            _bufferingMode = bufferingMode;
            Load();
            firstLoad = true;
        }
        [DispId(10)]
        public string ConnectionTest()
        {
            return( (string) Connections<string>.Get(_xs156Setting.ServerBaseUri() + "/api/tracking/GetTestHello"))+" "+ _thisEquipment.Id;
        }


        public void InvokeEvent()
        {

            if (TrackingProcessClosedEvent != null) TrackingProcessClosedEvent("Invoked Manually");
        }

        public EquipmentInt ThisEquipment()
        {
            return
                (EquipmentInt)
                    Connections<EquipmentInt>.Get(_xs156Setting.ServerBaseUri() + Equipment.GetPartialUri +
                                                  _thisEquipment.Id);
        }

        public EquipmentReferenceProcess ThisEquipmentReferenceProcess()
        {
            return _thisEquipmentReferenceProcess;
        }

        public bool StartNewTrackingProcess(string referenceByName, int target, string orderNumber)
        {
            try
            {
                var d = new ReferenceProcess()
                {
                    TargetQuantity = target,
                    ProductReference = ProductReference.GetByName(referenceByName).Id,
                    StartDateTime = DateTime.Now,
                    OrderNumber = orderNumber
                };
                _trackingData = TrackingData.CreateTrackingData(d, _thisEquipment);
                Load();
                return true;
            }
            catch (Exception ex)
            {
                if (ExceptionEvent != null) ExceptionEvent("Create Tracking : " + ex.Message);
                return false;
            }
        }

        public void CloseTrackingProcess()
        {
            _trackingData.Close();
        }

        public void StartUpdater()
        {
            _scanTicker = new Ticker();
            _scanTicker.Elapsed += TickerElapased;
            _scanTicker.Enabled = true;
            _tickerOn = true;
        }

        public void StopUpdater()
        {
            _scanTicker.Enabled = false;
            _tickerOn = false;
        }

        public int UpdateOutputQuantity(int deltaOutputQuantity)
        {
            _thisEquipmentReferenceProcess.IncreaseOutput(deltaOutputQuantity);
            _thisEquipmentReferenceProcess.Update();
            return _thisEquipmentReferenceProcess.OutputQuantity;
        }
        public int SetOutputQuantity(int outputQuantity)
        {
            _thisEquipmentReferenceProcess.OutputQuantity = outputQuantity;
            _thisEquipmentReferenceProcess.Update();
            return _thisEquipmentReferenceProcess.OutputQuantity;
        }
        public int SetRejectQuantity(int rejectQuantity)
        {
            _thisEquipmentReferenceProcess.RejectedQuantity = rejectQuantity;
            _thisEquipmentReferenceProcess.Update();
            return _thisEquipmentReferenceProcess.RejectedQuantity;
        }
        public int UpdateRejectedQuantity(int deltaRejectedQuantity)
        {
            _thisEquipmentReferenceProcess.IncreaseOutput(deltaRejectedQuantity);
            _thisEquipmentReferenceProcess.Update();
            return _thisEquipmentReferenceProcess.RejectedQuantity;
        }

        public int GetProcessableQuantity()
        {
            return _thisEquipmentReferenceProcess.ProcessAbleQuantity;
        }

        public string GetProcessIdentity()
        {
            return _trackingData.ReferenceProcess!=null? _trackingData.ReferenceProcess.ProcessGuid.ToString():Guid.NewGuid().ToString();

        }
        public string GetCurrentOrderNumber()
        {
            return _trackingData.ReferenceProcess != null ? _trackingData.ReferenceProcess.OrderNumber : string.Empty;

        }
        public string GetLineGroupByGuid(Guid id)
        {
            try
           {
               var j = (EquipmentLineGroup)Connections<EquipmentLineGroup>.Get(_xs156Setting.ServerBaseUri() + "api/EquipmentGroup/" + id);
                return j.LineGroup;
           }
           catch (Exception)
           {
                return "";
           }

        }

        public string GetLatestByGuid(Guid guid)
        {
           try
           {
               var j = (string)Connections<string>.Get(_xs156Setting.ServerBaseUri() + "api/tracking/GetLastByLineGroupS/" + guid);
               return j;
           }
           catch (Exception)
           {
                return "";
           } 
        }

        public void CompleteCurrentEquipmentProcess()
        {
            _thisEquipmentReferenceProcess.Complete();
        }

        public bool IsCompleted()
        {
            return _thisEquipmentReferenceProcess.Completed;
        }

        public bool IsBufferedMode()
        {
            return _bufferingMode;
        }

        public bool LoadByOrderNumber(string ordernumber)
        {
            try
            {
                _scanTicker.Enabled = false;
               var trackingData = TrackingData.LoadByOrderNumber(ordernumber);
                if (trackingData.ReferenceProcess != null)
                {
                    _currentReference = trackingData.ProductReference;
                    _thisEquipmentReferenceProcess =
                        trackingData.EquipmentReferenceProcesses.First(x => x.Equipment == new Guid(_thisEquipment.Id));
                    var data = trackingData.GetEquipmentStatusBagOfProcess(
                        trackingData.ReferenceProcess.ProcessGuid.ToString(),
                        _thisEquipment);
                    data.LineGroupName = GetLineGroupByGuid(new Guid(data.EquipmentLineGroupIdentity));
                    TrackingDataBag = data;
                    if (TrackingReferenceNewlyLoaded != null)
                        TrackingReferenceNewlyLoaded(TrackingDataBag);
                    _trackingData = trackingData;
                    return true;
                }
                else
                {
                    if (ExceptionEvent != null) ExceptionEvent("Load By OrderNumber: Not Found!");
                    
                }
            }
            catch (Exception exception)
            {
                if (ExceptionEvent != null) ExceptionEvent("Load By OrderNumber : "+exception.Message);
                
            }
            _scanTicker.Enabled = _tickerOn;
            return false;
        }
    }
}
