using System;
using System.Runtime.InteropServices;
using XS156Client35.Helper;

namespace XS156Client35.Models
{
    [Guid("21672510-2B24-4F2E-8226-2FDC70D40722")]
    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Ansi)]
    public class EquipmentReferenceProcess
    {

        private int _processableQty;
        private int _qtyToProcess;

        public Guid Id { get;  set; }
        public Guid Equipment { get; set; }

        public Guid ReferenceProcess { get; set; }

        public int ProcessAbleQuantity
        {
            get
            {
                _processableQty=  UpdateProcessAbleQuantity();
                return _processableQty;
            }
            protected set { _processableQty = value; }
        }

        public int OutputQuantity { get;  set; }

        public int RejectedQuantity { get; set; }
        public int TargetQuantity { get;  set; }
        public bool Completed { get; set; }

        public int QuantityLeftToProcess
        {
            get
            {
                return _qtyToProcess;
            }
        }

        public DateTime LastUpdated { get; protected set; }

        public int IncreaseOutput(int value)
        {
            OutputQuantity += value;
            _qtyToProcess = _processableQty - OutputQuantity;
            return OutputQuantity;
        }
        public int DecreaseOutput(int value)
        {
            OutputQuantity -= value;
            _qtyToProcess = _processableQty - OutputQuantity;
            return OutputQuantity;
        }
        public int IncreaseRejected(int value)
        {
            RejectedQuantity += value;
            return RejectedQuantity;
        }
        public int UpdateProcessAbleQuantity()
        {
            var setting = new Xs156Setting();
            var j = (EquipmentReferenceProcess)Connections<EquipmentReferenceProcess>.Get(setting.ServerBaseUri() + "api/EquipmentReferenceProcess/GetPrevById/" + Id);
            return j.OutputQuantity;
        }

        public int SetTarget(int value)
        {
            _qtyToProcess = _qtyToProcess + (value - TargetQuantity);
            TargetQuantity = value;
            return TargetQuantity;
        }

        public void Refresh()
        {
            var setting = new Xs156Setting();
            var j = (EquipmentReferenceProcess)Connections<EquipmentReferenceProcess>.Get(setting.ServerBaseUri() + "api/EquipmentReferenceProcess/" + Id);
        }

        public void Update()
        {
            var setting = new Xs156Setting();
            var j = (EquipmentReferenceProcess)Connections<EquipmentReferenceProcess>.Put( setting.ServerBaseUri() + "api/EquipmentReferenceProcess/"+Id , this);
        }

        public void Complete()
        {
            Completed = true;
            var setting = new Xs156Setting();
            var j = (EquipmentReferenceProcess)Connections<EquipmentReferenceProcess>.Put(setting.ServerBaseUri() + "api/EquipmentReferenceProcess/" + Id, this);
        }

        public bool IsCompleted()
        {
            var setting = new Xs156Setting();
            var j = (EquipmentReferenceProcess)Connections<EquipmentReferenceProcess>.Get(setting.ServerBaseUri() + "api/EquipmentReferenceProcess/" + Id);
            if (j != null)
            {
                return j.Completed;
            }
            return false;
        }
    }
}