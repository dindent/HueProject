using MainViewApplication.View;
using MainViewApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MainViewApplication.Presenter
{
    public class FirstViewPresenter
    {
        private readonly FirstView view;
        private readonly FirstViewModel viewModel;

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
            var bridge = Hue.HueBridgeLocator.Locate();
        }

        public UserControl GetView()
        {
            return view;
        }
    }
}
