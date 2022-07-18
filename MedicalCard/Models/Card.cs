using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MedicalCard.Models
{
    public enum Sex
    {
        Жен, Муж
    }

    public enum CountryType
    {
        Гор, Сел
    }
    public class Card : INotifyPropertyChanged
    {
        private int _id;
        private string _fio = "";
        private Sex _sex = Models.Sex.Жен;
        private string _birthDay = "";
        private string _address = "";
        private string _phone = "";
        private CountryType _countryType = Models.CountryType.Гор;
        private string _passport = "";
        private string _dateReg = DateTime.Now.ToString("dd.MM.yyyy");

        public Card() { }

        public Card(int id, string fio, Sex sex, string birthDay, string address, string phone, CountryType countryType, string passport, string dateReg)
        {
            _id = id;
            _fio = fio;
            _sex = sex;
            _birthDay = birthDay;
            _address = address;
            _phone = phone;
            _countryType = countryType;
            _passport = passport;
            _dateReg = dateReg;
        }

        public Card Clone()
        {
            return new Card(Id, Fio, _sex, BirthDay, Address, Phone, _countryType, Passport, DateReg);
        }

        public int Id
        {
            get => _id;

            set
            {
                _id = value;
                OnPropetryChanged(nameof(Id));
            }
        }

        public string Fio
        {
            get => _fio;

            set
            {
                _fio = value;
                OnPropetryChanged(nameof(Fio));
            }
        }

        public int Sex
        {
            get => (int)_sex;

            set
            {
                _sex = (Sex)value;
                OnPropetryChanged(nameof(Sex));
            }
        }

        public string BirthDay
        {
            get => _birthDay;

            set
            {
                _birthDay = value;
                OnPropetryChanged(nameof(BirthDay));
            }
        }

        public string Address
        {
            get => _address;

            set
            {
                _address = value;
                OnPropetryChanged(nameof(Address));
            }
        }

        public string Phone
        {
            get => _phone;

            set
            {
                _phone = value;
                OnPropetryChanged(nameof(Phone));
            }
        }

        public int CountryType
        {
            get => (int)_countryType;

            set
            {
                _countryType = (CountryType)value;
                OnPropetryChanged(nameof(CountryType));
            }
        }

        public string Passport
        {
            get => _passport;

            set
            {
                _passport = value;
                OnPropetryChanged(nameof(Passport));
            }
        }

        public string DateReg
        {
            get => _dateReg;

            set
            {
                _dateReg = value;
                OnPropetryChanged(nameof(DateReg));
            }
        }

        public void Notify()
        {
            OnPropetryChanged(nameof(DateReg));
            OnPropetryChanged(nameof(Passport));
            OnPropetryChanged(nameof(CountryType));
            OnPropetryChanged(nameof(Phone));
            OnPropetryChanged(nameof(Address));
            OnPropetryChanged(nameof(BirthDay));
            OnPropetryChanged(nameof(Sex));
            OnPropetryChanged(nameof(Fio));
            OnPropetryChanged(nameof(Id));
        }

        public string GetShortFIO()
        {
            StringBuilder res = new StringBuilder();
            string[] splitedFio = Fio.Split(' ');
            res.Append($"{splitedFio[0]} ");
            res.Append($"{splitedFio[1][0]}. ");
            if (splitedFio.Length == 3)
            {
                res.Append($"{splitedFio[2][0]}.");
            }

            return res.ToString().Trim();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropetryChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
