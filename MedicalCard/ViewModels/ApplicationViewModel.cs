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
using MedicalCard.Commands;

namespace MedicalCard.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ApplicationContext db;
        private GridAnimationManager _savingMenuAnimation;
        private GridAnimationManager _cardMenuAnimation;
        private bool _isCreatingCard;

        public ObservableCollection<Card> Cards { get; set; }

        public ApplicationViewModel()
        {
            _cardMenuWidth = new GridLengthContainer(0.0, GridUnitType.Star);
            _saveMenuHeight = new GridLengthContainer(0.0, GridUnitType.Pixel);

            _savingMenuAnimation = new GridAnimationManager(this, _saveMenuHeight, nameof(SaveMenuHeight), 0, 60, 100, 120);
            _cardMenuAnimation = new GridAnimationManager(this, _cardMenuWidth, nameof(CardMenuWidth), 0.0, 0.3, 100, 120);

            _editing = false;
            _isCreatingCard = false;

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

        private Card _selectedCard;
        public Card SelectedCard
        {
            get => _selectedCard;

            set
            {
                _selectedCard = value;
                Task.Factory.StartNew(() =>
                {
                    _cardMenuAnimation.Close();
                    OnPropetryChanged(nameof(SelectedCard));
                    _cardMenuAnimation.Open();
                });

            }
        }

        private GridLengthContainer _cardMenuWidth;
        public GridLength CardMenuWidth
        {
            get { return _cardMenuWidth.Length; }
            set
            {
                _cardMenuWidth.Length = value;
                OnPropetryChanged(nameof(CardMenuWidth));
            }
        }

        private GridLengthContainer _saveMenuHeight;
        public GridLength SaveMenuHeight
        {
            get { return _saveMenuHeight.Length; }
            set
            {
                _saveMenuHeight.Length = value;
                OnPropetryChanged(nameof(SaveMenuHeight));
            }
        }

        private bool _editing;
        public bool IsEditing
        {
            get => _editing;
            set
            {
                _editing = value;
                Task.Factory.StartNew(() =>
                {
                    if (_editing == true)
                    {
                        _savingMenuAnimation.Open();
                    }
                    else
                    {
                        _savingMenuAnimation.Close();
                    }

                    OnPropetryChanged(nameof(IsEditing));
                    OnPropetryChanged(nameof(NotIsEditing));
                });
            }
        }

        public bool NotIsEditing
        {
            get => !_editing;
        }

        private RelayCommand _updCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updCommand ??
                    (_updCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand _printCommand;
        public RelayCommand PrintCommand
        {
            get
            {
                return _printCommand ??
                    (_printCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _addCommand ??
                    (_addCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand _editCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _editCommand ??
                    (_editCommand = new RelayCommand(obj =>
                    {
                        IsEditing = true;
                    }));
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
                    (_deleteCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand _saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _saveCommand ??
                    (_saveCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand ??
                    (_cancelCommand = new RelayCommand(obj =>
                    {

                    }));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
