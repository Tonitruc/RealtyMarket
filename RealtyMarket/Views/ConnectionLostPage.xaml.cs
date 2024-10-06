using RealtyMarket.Service;
using RealtyMarket.ViewModels;

namespace RealtyMarket.Views
{
    [QueryProperty(nameof(ReturnPage), "ReturnPage")]
    public partial class ConnectionLostPage : ContentPage
    {
        private readonly ConnectivityService _connectivityService;

        private string _returnPage;
        public string ReturnPage
        {
            get => _returnPage;
            set
            {
                _returnPage = value;
                OnPropertyChanged(); 
                if (BindingContext is ConnectionLostViewModel viewModel)
                {
                    viewModel.ReturnPage = _returnPage; 
                }
            }
        }


        public ConnectionLostPage(ConnectivityService connectivityService)
        {
            InitializeComponent();

            BindingContext = new ConnectionLostViewModel(connectivityService, ReturnPage);

            _connectivityService = connectivityService;
        }
    }
}