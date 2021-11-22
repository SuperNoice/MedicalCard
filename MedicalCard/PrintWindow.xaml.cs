using MedicalCard.Models;
using MedicalCard.ViewModels;
using System.Windows;

namespace MedicalCard
{
    /// <summary>
    /// Логика взаимодействия для PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow : Window
    {
        Card _selectedCard;
        PrintViewModel _printViewModel;

        public PrintWindow(Card card)
        {
            InitializeComponent();

            _selectedCard = card;
            _printViewModel = new PrintViewModel(_selectedCard);

            DataContext = _printViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_printViewModel.IsPrinting)
            {
                e.Cancel = true;
            }
        }
    }
}
