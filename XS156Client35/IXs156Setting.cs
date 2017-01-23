
using System.Runtime.InteropServices;

namespace XS156Client35
{
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [Guid("3AD30F69-13F1-49BB-8899-8D73C7A9E060")]
    public interface IXs156Setting
    {
        string WebApiClientLocation();
        void Save();
        string ServerBaseUri ();
        string ServerBaseUri(string defaultValue);
        string EquipmentIdentity();
        string EquipmentIdentity(string defaultValue);
        int GetTickerInterval(int defaultInterval);
        int GetTickerInterval();
        void SetTickerInterval(int defaultInterval);
    }
}
