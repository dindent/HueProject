using Hue;
using Hue.Contracts;
using MainViewApplication.Models;
using MainViewApplication.View;
using MainViewApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Utils;

namespace MainViewApplication.Presenter
{
    public class FirstViewPresenter
    {
        private readonly FirstView view;
        private readonly FirstViewModel viewModel;
        private bool IsLightsOn = true;

        public FirstViewPresenter()
        {
            this.view = new FirstView();
            this.viewModel = new FirstViewModel();
            view.DataContext = viewModel;
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            InitializeBridgeHue();
        }

        private void InitializeBridgeHue()
        {
            //Récuperation de toutes les ampoules
            var bridge = HueBridgeLocator.Locate();
            viewModel.Lights = new ObservableCollection<LightsModel>();
            foreach (var light in bridge.Lights)
            {
                LightsModel l = new LightsModel(light.Value, light.Key);
                l.SwitchHueLight = new RelayCommand(_ => SwitchHueLight(light.Key));
                viewModel.Lights.Add(l);
            }

        }

        private void SwitchHueLight(string key)
        {
            if (IsLightsOn)
            {
                HueBridgeLocator.Locate().TurnOffLights(key);
            }
            else
            {
                HueBridgeLocator.Locate().TurnOnLights(key);
            }
            IsLightsOn = !IsLightsOn;
        }

        public UserControl GetView()
        {
            return view;
        }
    }
}
