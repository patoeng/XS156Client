using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.InteropServices;

namespace XS156Client35
{
    [Guid("59F372C0-DFAB-46FA-972C-E71B1E600CED")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("Xs156Client.Xs156Setting")]
    public  class Xs156Setting: IXs156Setting
    {
        private readonly Configuration _appConfiguration;
        
        public Xs156Setting()
        {
            _appConfiguration = ConfigurationManager.OpenExeConfiguration(WebApiClientLocation());
        }
       
        [DispId(1)]
        public  string WebApiClientLocation()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            return location;
        }

        private  object GetCreateSetting(string parameter, string value)
        {
            
            try
            {
                return _appConfiguration.AppSettings.Settings[parameter].Value;
            }
            catch (Exception)
            {
                _appConfiguration.AppSettings.Settings.Add(new KeyValueConfigurationElement(parameter, value));
                return _appConfiguration.AppSettings.Settings[parameter].Value;
            }
        }
        private  void SetCreateSetting(string parameter, string value)
        {
            try
            {
                _appConfiguration.AppSettings.Settings[parameter].Value = value;
            }
            catch (Exception)
            {
                _appConfiguration.AppSettings.Settings.Add(new KeyValueConfigurationElement(parameter, value));
            }
        }
        private  string GetSetting(string parameter, string defaultValue)
        {
            return GetCreateSetting(parameter, defaultValue).ToString();
        }
        private   void SetSetting(string parameter,string parameterValue)
        {
            SetCreateSetting(parameter, parameterValue);
        }
        [DispId(2)]
        public void Save()
        {
            _appConfiguration.Save();
        }
        [DispId(3)]
        public string ServerBaseUri ()
        {
             return GetSetting("BaseApiUri","http://localhost/service/"); 
        }
        [DispId(5)]
        public string ServerBaseUri(string defaultValue)
        {
            return GetSetting("BaseApiUri", defaultValue);
        }
        [DispId(4)]
        public  string EquipmentIdentity()
        {
           return GetSetting("EquipmentIdentity", Guid.NewGuid().ToString()); 
        }
        [DispId(6)]
        public string EquipmentIdentity(string defaultValue)
        {
            return GetSetting("EquipmentIdentity", new Guid(defaultValue).ToString());
        }

        public int GetTickerInterval(int defaultInterval)
        {
            return Convert.ToInt32(GetSetting("TickerInterval", defaultInterval.ToString()))*1000;
        }
        public int GetTickerInterval()
        {
          return Convert.ToInt32(GetSetting("TickerInterval", "5"))*1000;
        }
        public void SetTickerInterval(int defaultInterval)
        {
            SetSetting("TickerInterval", defaultInterval.ToString());
        }
        public bool GetBufferingMode()
        {
            return Convert.ToBoolean(GetSetting("BufferingMode",false.ToString()));
        }
    }
}
