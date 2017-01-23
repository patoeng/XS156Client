using System;

namespace XS156Client35.Models
{
 
    public class ReferenceProcess : IEquatable<ReferenceProcess>
    {
     
        public virtual int Id { get; set; }
      
        public virtual Guid ProcessGuid { get; set; }
       
        public virtual Guid ProductReference { get; set; }
        
       
        public virtual Guid LineGroup { get; set; }
        
        public virtual int TargetQuantity { get; set; }
       
        public virtual DateTime StartDateTime { get; set; }
       
        public virtual DateTime EndDateTime { get; set; }
       
        public virtual bool IsClosed { get; set; }
        public virtual string OrderNumber { get; set; }
        public bool Equals(ReferenceProcess other)
        {
            return ProcessGuid == other.ProcessGuid;
        }
    }
}