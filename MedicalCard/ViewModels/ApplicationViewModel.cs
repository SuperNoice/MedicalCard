using System;
using System.Windows;
using System.Windows.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Controls;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using MedicalCard.Models;
using MedicalCard.Animations;
using System.Windows.Threading;
using System.Threading;

namespace MedicalCard.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ApplicationContext db;
        private GridAnimation _savingCardMenuAnimation;
        private GridAnimation _cardMenuAnimation;
        private Card _selectedCard;

        public ObservableCollection<Card> Cards { get; set; }

        public Card SelectedCard
        {
            get => _selectedCard;

            set
            {
                _selectedCard = value;
                Task.Factory.StartNew(() =>
                {
                    _cardMenuAnimation.Animation(ref _menuWidth, nameof(MenuWidth), 0.0, 100, 120);
                    OnPropetryChanged(nameof(SelectedCard));
                    _cardMenuAnimation.Animation(ref _menuWidth, nameof(MenuWidth), 0.3, 100, 120);
                });

            }
        }

        private GridLength _menuWidth;
        public GridLength MenuWidth
        {
            get { return _menuWidth; }
            set
            {
                _menuWidth = value;
                OnPropetryChanged(nameof(MenuWidth));
            }
        }

        private GridLength _saveMenuWidth;
        public GridLength SaveMenuWidth
        {
            get { return _saveMenuWidth; }
            set
            {
                _saveMenuWidth = value;
                OnPropetryChanged(nameof(SaveMenuWidth));
            }
        }

        public ApplicationViewModel()
        {
            _menuWidth = new GridLength(0.0, GridUnitType.Star);
            _savingCardMenuAnimation = new GridAnimation(this);
            _cardMenuAnimation = new GridAnimation(this);
            db = new ApplicationContext();

            db.Cards?.Load();
            //Cards = db.Cards?.Local.ToObservableCollection() ?? new ObservableCollection<Card>();
            Cards = new ObservableCollection<Card>();

            Cards.Add(new Card()
            {
                Fio = "Лозовик Леонид Евгеньевич",
                Phone = "+7(999)632-68-75",
                BirthDay = "14.02.2001",
                Passport = "1720 814484 Зарегестрирован в Мвд россии по республике адыгея"
            });
            Cards.Add(new Card()
            {
                Fio = "YaЛозовик Леонид Евгеньевич",
                Phone = "+7(999)632-68-75",
                BirthDay = "14.02.2001",
                Passport = "1720 814484 Зарегестрирован в Мвд россии по республике адыгея"
            });

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
