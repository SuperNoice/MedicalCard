using MedicalCard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedicalCard.Validations
{
    public static class CardNormalize
    {
        public static void Normalize(Card card)
        {
            if (card == null)
            {
                return;
            }

            card.Fio = FioNormalize(card.Fio);
            card.BirthDay = DateNormalize(card.BirthDay);
            card.Address = card.Address.Trim();
            card.Phone = PhoneNormalize(card.Phone);
            card.Passport = card.Passport.Trim();
            card.DateReg = DateNormalize(card.DateReg);
        }

        private static string FioNormalize(string fio)
        {
            var arr = fio.Trim().Split(" ");
            StringBuilder result = new StringBuilder();
            foreach (var item in arr)
            {
                result.Append(char.ToUpper(item[0]));
                result.Append(item.Substring(1));
                result.Append(" ");
            }

            return result.ToString();
        }

        private static string PhoneNormalize(string phone)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char ch in phone)
            {
                if (Regex.IsMatch(ch.ToString(), @"\d"))
                {
                    stringBuilder.Append(ch);
                }
            }
            string numbers = stringBuilder.ToString();
            if (numbers[0] == '8')
            {
                numbers = "7" + numbers.Substring(1);
            }

            return $"+{numbers[0]}({numbers.Substring(1, 3)}){numbers.Substring(4, 3)}-{numbers.Substring(7, 2)}-{numbers.Substring(9, 2)}";
        }

        private static string DateNormalize(string date)
        {
            DateTime scheduleDate;
            DateTime.TryParseExact(date.Trim(), new string[] { "dd.MM.yyyy", "d.M.yyyy" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate);

            return scheduleDate.ToString("dd.MM.yyyy");
        }
    }
}
