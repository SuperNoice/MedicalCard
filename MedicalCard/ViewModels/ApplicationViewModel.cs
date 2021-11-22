using MedicalCard.Animations;
using MedicalCard.Commands;
using MedicalCard.Extensions;
using MedicalCard.Models;
using MedicalCard.Validations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MedicalCard.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private ApplicationContext _db;
        private GridAnimationManager _savingMenuAnimation;
        private GridAnimationManager _cardMenuAnimation;
        private bool _isAdding;
        private bool _isUpdating;

        private ObservableCollection<Card> _cards;
        public ObservableCollection<Card> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropetryChanged(nameof(Cards));
            }
        }

        public ApplicationViewModel()
        {
            CheckDB();
            BackupDB();

            _cardMenuWidth = new GridLengthContainer(0.0, GridUnitType.Star);
            _saveMenuHeight = new GridLengthContainer(0.0, GridUnitType.Pixel);

            _savingMenuAnimation = new GridAnimationManager(this, _saveMenuHeight, nameof(SaveMenuHeight), 0, 60, 100, 100);
            _cardMenuAnimation = new GridAnimationManager(this, _cardMenuWidth, nameof(CardMenuWidth), 0.0, 0.3, 100, 100);

            _isEditing = false;
            _isAdding = false;
            _isUpdating = false;

            _db = new ApplicationContext();
            UpdateCommand.Execute(0);
        }

        private void CheckDB()
        {
            if (File.Exists("./medical.db"))
            {
                return;
            }
            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MedicalCardBackup", "medical.db")))
            {
                File.Copy(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MedicalCardBackup", "medical.db"), "./medical.db", true);
                return;
            }
            MessageBox.Show("Ошибка! Файл medical.db не найден! Программа будет закрыта!", "Ошибка!");
            Environment.Exit(0);
        }

        private void BackupDB()
        {
            if (File.Exists("./lastbackup.txt"))
            {
                string text = File.ReadAllText("./lastbackup.txt");
                DateTime scheduleDate;
                if (DateTime.TryParseExact(text.Trim(), "dd.MM.yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate))
                {
                    if (scheduleDate.Day == DateTime.Now.Day && scheduleDate.Month == DateTime.Now.Month && scheduleDate.Year == DateTime.Now.Year)
                    {
                        return;
                    }
                }
            }
            try
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MedicalCardBackup"));
                File.Copy("./medical.db", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MedicalCardBackup", "medical.db"), true);
                File.WriteAllText("./lastbackup.txt", DateTime.Now.ToString("dd.MM.yyyy"));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Ошибка бекапа базы данных!");
            }
        }

        private Card _selectedCard;
        public Card SelectedCard
        {
            get => _selectedCard;

            set
            {
                _selectedCard = value;
                if (!_isUpdating)
                {
                    Task.Factory.StartNew(() =>
                    {
                        _cardMenuAnimation.Close();
                        OnPropetryChanged(nameof(SelectedCard));
                        _cardMenuAnimation.Open();
                    });
                }
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

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                Task.Factory.StartNew(() =>
                {
                    if (_isEditing == true)
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
            get => !_isEditing;
        }

        private RelayCommand _updCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                return _updCommand ??
                    (_updCommand = new RelayCommand(obj =>
                    {
                        _isUpdating = true;
                        _db.Cards?.Load();
                        Cards = new ObservableCollection<Card>(_db.Cards?.Local.Reverse());
                        OnPropetryChanged(nameof(Cards));
                        _selectedCard = null;
                        Task.Factory.StartNew(() =>
                        {
                            _cardMenuAnimation.Close();
                            _isUpdating = false;
                        });
                        SearchText = "";
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
                        PrintWindow printWindow = new PrintWindow(_selectedCard);
                        printWindow.ShowDialog();
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
                        _isAdding = true;
                        IsEditing = true;
                        SelectedCard = new Card();
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
                        MessageBoxResult dialogResult = MessageBox.Show($"Удалить запись: {SelectedCard?.Fio}\nВы уверены?", "Удалить запись", MessageBoxButton.YesNo);
                        if (_isAdding)
                        {
                            if (dialogResult == MessageBoxResult.Yes)
                            {
                                CancelCommand.Execute(0);
                            }
                        }
                        else
                        {
                            if (dialogResult == MessageBoxResult.Yes)
                            {
                                try
                                {
                                    _db.Cards?.Remove(SelectedCard);
                                    _db.SaveChanges();
                                    _isUpdating = true;
                                    Cards?.Remove(SelectedCard);
                                    _isUpdating = false;
                                    _selectedCard = null;
                                    Task.Factory.StartNew(() =>
                                    {
                                        _savingMenuAnimation.Close();
                                    });
                                    Task.Factory.StartNew(() =>
                                    {
                                        _cardMenuAnimation.Close();
                                        IsEditing = false;
                                    });
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show($"Ошибка добавления в БД. Error:{e.Message}");
                                    return;
                                }
                            }
                        }
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
                        CardValidateStatus[] validateStatuses = CardValidation.Validate(SelectedCard);
                        if (validateStatuses[0] == CardValidateStatus.OK)
                        {
                            try
                            {
                                CardNormalize.Normalize(SelectedCard);

                                if (_isAdding)
                                {
                                    _db.Cards?.Add(SelectedCard);
                                    _db.SaveChanges();
                                    Cards?.Insert(0, SelectedCard);
                                    OnPropetryChanged(nameof(Cards));
                                    _isAdding = false;
                                }
                                else if (_isEditing)
                                {
                                    //_db.Cards.Update(SelectedCard);
                                    _db.Entry(SelectedCard).State = EntityState.Modified;
                                    _db.SaveChanges();
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"Ошибка добавления в БД. Error:{e.Message}");
                                return;
                            }
                        }
                        else
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (CardValidateStatus status in validateStatuses)
                            {
                                stringBuilder.Append($"{status}, ");
                            }
                            string fields = stringBuilder.ToString();
                            fields = fields.Substring(0, fields.Length - 2);
                            MessageBox.Show($"Найдены ошибки в полях: {fields}!");
                            return;
                        }

                        Task.Factory.StartNew(() =>
                        {
                            _savingMenuAnimation.Close();
                            IsEditing = false;
                        });
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
                        if (_isAdding)
                        {
                            _selectedCard = null;
                            _isAdding = false;
                            Task.Factory.StartNew(() =>
                            {
                                _cardMenuAnimation.Close();
                                IsEditing = false;
                            });
                            Task.Factory.StartNew(() =>
                            {
                                _savingMenuAnimation.Close();
                            });
                        }
                        else if (_isEditing)
                        {
                            // Откат изменений
                            foreach (var entry in _db.ChangeTracker.Entries())
                            {
                                switch (entry.State)
                                {
                                    case EntityState.Modified:
                                        entry.State = EntityState.Unchanged;
                                        break;
                                    case EntityState.Deleted:
                                        entry.Reload();
                                        break;
                                    case EntityState.Added:
                                        entry.State = EntityState.Detached;
                                        break;
                                }
                            }
                            SelectedCard?.Notify();
                            Task.Factory.StartNew(() =>
                            {
                                _savingMenuAnimation.Close();
                                IsEditing = false;
                            });
                        }


                    }));
            }
        }

        public void CloseCardMenu()
        {
            Task.Factory.StartNew(() =>
            {
                _cardMenuAnimation.Close();
                _isUpdating = true;
                _selectedCard = null;
                _isUpdating = false;
            });
        }

        private string _searchText = "";
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropetryChanged(nameof(SearchText));
            }
        }

        public void FilterCards()
        {
            if (_searchText.Trim().Length == 0)
            {
                UpdateCommand.Execute(0);
                return;
            }

            IEnumerable<Card> sorted;

            if (CardValidation.CheckDate(_searchText))
            {
                SearchText = CardNormalize.DateNormalize(_searchText);
                sorted = from card in _cards
                         orderby card.BirthDay?.DiceCoefficient(_searchText) descending
                         select card;
            }
            else if (CardValidation.CheckPhone(_searchText))
            {
                SearchText = CardNormalize.PhoneNormalize(_searchText);
                sorted = from card in _cards
                         orderby card.Phone?.DiceCoefficient(_searchText) descending
                         select card;
            }
            else if (CardValidation.CheckPassport(_searchText))
            {
                SearchText = CardNormalize.PassportNormalize(_searchText);
                sorted = from card in _cards
                         orderby card.Passport?.DiceCoefficient(_searchText) descending
                         select card;
            }
            else
            {
                SearchText = CardNormalize.FioNormalize(_searchText);
                sorted = from card in _cards
                         orderby card.Fio?.DiceCoefficient(_searchText) descending
                         select card;
            }

            Cards = new ObservableCollection<Card>(sorted);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
