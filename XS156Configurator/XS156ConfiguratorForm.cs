using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using XS156Client35;
using XS156Client35.Models;

namespace XS156Configurator
{
    public partial class Xs156ConfiguratorForm : Form
    {
        public delegate void TextDelegate(string data);

        public TextDelegate TxtBox3;
        public void TxtBox3M(string data)
        {
           
        }
        public Xs156ConfiguratorForm()
        {
            InitializeComponent();
        }

        #region Public CLass Properties

        public IXs156Setting Xs156Settings;
        public IXs156Client Xs156Client; 

        #endregion

        private void XS156ConfiguratorForm_Load(object sender, EventArgs e)
        {
            Xs156Settings = new Xs156Setting();
           
        }

        private void ProcessClosed(string data)
        {
            MessageBox.Show(data);
        }

        private void UpdateTrackingBag(TrackingDataBag data)
        {
            this.Invoke(TxtBox3,new object[]{JsonConvert.SerializeObject(data)});
           // MessageBox.Show(JsonConvert.SerializeObject(data));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Xs156Settings.ServerBaseUri("http://localhost/service/"));
            MessageBox.Show(Xs156Settings.EquipmentIdentity());
            Xs156Settings.SetTickerInterval(4);
            Xs156Settings.Save();
            Xs156Client = new Xs156Client();

            Xs156Client.TrackingDataBagUpdatedEvent += UpdateTrackingBag;
            Xs156Client.TrackingProcessClosedEvent += ProcessClosed;
            TxtBox3 = new TextDelegate(TxtBox3M);

            Xs156Client.StartUpdater();
        }
    }
}
