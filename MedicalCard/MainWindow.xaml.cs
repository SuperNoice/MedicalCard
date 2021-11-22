using MedicalCard.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace MedicalCard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ApplicationViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            _viewModel = new ApplicationViewModel();
            DataContext = _viewModel;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.CloseCardMenu();
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _viewModel.FilterCards();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.BackupDB();
        }
    }
}
