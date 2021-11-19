using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MedicalCard.Models;
using MedicalCard.ViewModels;

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
            _printViewModel = new PrintViewModel();
            
            DataContext = _printViewModel;
        }
    }
}
