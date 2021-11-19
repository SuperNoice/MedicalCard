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
            card.Passport = PassportNormalize(card.Passport);
            card.DateReg = DateNormalize(card.DateReg);
        }

        public static string FioNormalize(string fio)
        {
            var arr = fio.Trim().Split(" ");
            StringBuilder result = new StringBuilder();
            foreach (var item in arr)
            {
                result.Append(char.ToUpper(item[0]));
                result.Append(item.Substring(1));
                result.Append(" ");
            }

            return result.ToString().Trim();
        }

        public static string PhoneNormalize(string phone)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char ch in phone)
            {
                if (Regex.IsMatch(ch.ToString(), @"\d"))
                {
                    stringBuilder.Append(ch);
                }
            }
            string numbers = stringBuilder.ToString().Trim();
            if (numbers[0] == '8')
            {
                numbers = "7" + numbers.Substring(1);
            }

            return $"+{numbers[0]}({numbers.Substring(1, 3)}){numbers.Substring(4, 3)}-{numbers.Substring(7, 2)}-{numbers.Substring(9, 2)}";
        }

        public static string DateNormalize(string date)
        {
            DateTime scheduleDate;
            DateTime.TryParseExact(date.Trim(), new string[] { "dd.MM.yyyy", "d.M.yyyy" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate);

            return scheduleDate.ToString("dd.MM.yyyy");
        }

        public static string PassportNormalize(string passport)
        {
            StringBuilder nomStringBuilder = new StringBuilder();
            string secondPart = "";
            int count = 0;

            for (int index = 0; index < passport.Length; ++index)
            {
                if (Char.IsDigit(passport[index]))
                {
                    if (count == 4)
                    {
                        nomStringBuilder.Append(" ");
                    }
                    ++count;
                    nomStringBuilder.Append(passport[index]);
                }
                if (Char.IsLetter(passport[index]))
                {
                    secondPart = passport.Substring(index);
                    break;
                }
            }

            if (secondPart.Length == 0)
            {
                return nomStringBuilder.ToString();
            }

            StringBuilder secondPartStringBuilder = new StringBuilder();
            foreach (string item in secondPart.Split(" "))
            {
                secondPartStringBuilder.Append(item);
                secondPartStringBuilder.Append(" ");
            }

            nomStringBuilder.Append(" ");
            nomStringBuilder.Append(secondPartStringBuilder.ToString().Trim());

            return nomStringBuilder.ToString().Trim();
        }
    }
}