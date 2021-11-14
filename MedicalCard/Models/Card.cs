using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MedicalCard.Models
{
    public class Card : INotifyPropertyChanged
    {
        private int? _id;
        private string? _fio;
        private string? _sex;
        private string? _birthDay;
        private string? _address;
        private string? _phone;
        private string? _countryType;
        private string? _passport;

        public int? Id
        {
            get => _id;

            set
            {
                _id = value;
                OnPropetryChanged(nameof(Id));
            }
        }

        public string? Fio
        {
            get => _fio;

            set
            {
                _fio = value;
                OnPropetryChanged(nameof(Fio));
            }
        }

        public string? Sex
        {
            get => _sex;

            set
            {
                _sex = value;
                OnPropetryChanged(nameof(Sex));
            }
        }

        public string? BirthDay
        {
            get => _birthDay;

            set
            {
                _birthDay = value;
                OnPropetryChanged(nameof(BirthDay));
            }
        }

        public string? Address
        {
            get => _address;

            set
            {
                _address = value;
                OnPropetryChanged(nameof(Address));
            }
        }

        public string? Phone
        {
            get => _phone;

            set
            {
                _phone = value;
                OnPropetryChanged(nameof(Phone));
            }
        }

        public string? CountryType
        {
            get => _countryType;

            set
            {
                _countryType = value;
                OnPropetryChanged(nameof(CountryType));
            }
        }

        public string? Passport
        {
            get => _passport;

            set
            {
                _passport = value;
                OnPropetryChanged(nameof(Passport));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
