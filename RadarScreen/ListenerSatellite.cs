using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarScreen
{
    public class ListenerSatellite : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Satellite _satellite = new Satellite();

        public Satellite satellite
        {
            get => _satellite;
            set
            {
                _satellite = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Satellite"));
            }
        }
    }
}
