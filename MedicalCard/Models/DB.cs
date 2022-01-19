using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Collections.ObjectModel;
using System.Data;

namespace MedicalCard.Models
{
    public class DB
    {
        private string _dbPath = "./medical.db";
        private SQLiteConnection _connection;
        private SQLiteCommand _command;
        private static DB _instance;

        private DB()
        {
            _connection = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            _command = _connection.CreateCommand();
        }

        public static DB GetInstance()
        {
            return _instance ?? (_instance = new DB());
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public ObservableCollection<Card> GetAll()
        {
            ObservableCollection<Card> cards = new ObservableCollection<Card>();
            var table = new DataTable();

            _connection.Open();

            _command.CommandText = "SELECT * FROM Cards ORDER BY Id DESC";
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter("SELECT * FROM Cards ORDER BY Id DESC", _connection);
            dataAdapter.Fill(table);
            dataAdapter.Dispose();

            _connection.Close();

            foreach (DataRow row in table.Rows)
            {
                var card = new Card(Convert.ToInt32(row[0]), Convert.ToString(row[1]), (Sex)Enum.ToObject(typeof(Sex), row[2]), Convert.ToString(row[3]), Convert.ToString(row[4]), Convert.ToString(row[5]), (CountryType)Enum.ToObject(typeof(CountryType), row[6]), Convert.ToString(row[7]), Convert.ToString(row[8]));
                cards.Add(card);
            }

            return cards;
        }

        public void Add(Card card)
        {
            _connection.Open();

            _command.CommandText = $"INSERT INTO Cards VALUES (NULL, '{card.Fio}', {card.Sex}, '{card.BirthDay}', '{card.Address}', '{card.Phone}', {card.CountryType}, '{card.Passport}', '{card.DateReg}')";
            var result = _command.ExecuteNonQuery();

            Log.Write($"Добавлено записей: {result} (fio: {card.Fio})");

            _connection.Close();
        }

        public void Del(Card card)
        {
            _connection.Open();

            _command.CommandText = $"DELETE FROM Cards WHERE Id = {card.Id}";
            var result = _command.ExecuteNonQuery();

            Log.Write($"Удалено записей: {result} (fio: {card.Fio})");

            _connection.Close();
        }

        public void Upd(Card card)
        {
            _connection.Open();

            _command.CommandText = $"UPDATE Cards SET Fio = '{card.Fio}', Sex = {card.Sex}, Birthday = '{card.BirthDay}', Address = '{card.Address}', Phone = '{card.Phone}', CountryType = {card.CountryType}, Passport = '{card.Passport}', DateReg = '{card.DateReg}' WHERE Id = {card.Id}";
            var result = _command.ExecuteNonQuery();

            Log.Write($"Обновлено записей: {result} (fio: {card.Fio})");

            _connection.Close();
        }
    }
}
