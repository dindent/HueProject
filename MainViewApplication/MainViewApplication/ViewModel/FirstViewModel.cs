﻿using Hue.Contracts;
using MainViewApplication.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace MainViewApplication.ViewModel
{
    public class FirstViewModel : PropertyChangedBase
    {
        private ObservableCollection<LightsModel> lights;

        public ObservableCollection<LightsModel> Lights
        {
            get
            {
                return lights;
            }

            set
            {
                lights = value;
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
    }
}
