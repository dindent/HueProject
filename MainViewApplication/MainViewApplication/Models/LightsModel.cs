using Hue.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace MainViewApplication.Models
{
    public class LightsModel : PropertyChangedBase
    {

        public LightsModel(HueLight light, string id)
        {
            Light = light;
            IdHueLight = id;
        }

        private string idHueLight;
        public string IdHueLight
        {
            get
            {
                return idHueLight;
            }

            set
            {
                idHueLight = value;
                OnPropertyChanged();
            }
        }


        private RelayCommand switchHueLight;
        public RelayCommand SwitchHueLight
        {
            get
            {
                return switchHueLight;
            }

            set
            {
                switchHueLight = value;
                OnPropertyChanged();
            }
        }

        private HueLight light;
        public HueLight Light
        {
            get
            {
                return light;
            }

            set
            {
                light = value;
                OnPropertyChanged();
            }
        }


    }
}
