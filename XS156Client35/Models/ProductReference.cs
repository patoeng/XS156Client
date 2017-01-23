using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XS156Client35.Helper;

namespace XS156Client35.Models
{

    [StructLayout(LayoutKind.Auto, CharSet = CharSet.Ansi)] 

    [Guid("9EA3CDC1-2505-4187-A227-4CC1F28046C9")]
    public class ProductReference : IEquatable<ProductReference>
    {
        public  Guid Id { get; set; }
        
        public  String ReferenceName { get; set; }
        
        public  String Descriptions { get; set; }

        public bool Equals(ProductReference other)
        {
            return (Id == other.Id) || (ReferenceName == other.ReferenceName);
        }

        public override string ToString()
        {
            return ReferenceName;
        }

        public static ProductReference GetByName(string reference)
        {
            var setting = new Xs156Setting();
            var baseuri = setting.ServerBaseUri();
            var j= (ProductReference)  Connections<ProductReference>.Get(baseuri + "api/products/getproductbyname/" + reference);
            return j;
        }
    }
}