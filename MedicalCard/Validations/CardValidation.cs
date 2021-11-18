﻿using MedicalCard.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MedicalCard.Validations
{
    public enum CardValidateStatus
    {
        OK, ФИО, Пол, Дата_Рождения, Место_Регистрации, Телефон, Паспортные_Данные, Дата_Регистрации
    }

    public static class CardValidation
    {
        public static CardValidateStatus[] Validate(Card card)
        {
            List<CardValidateStatus> result = new List<CardValidateStatus>();

            if (!CheckFio(card.Fio.Trim()))
            {
                result.Add(CardValidateStatus.ФИО);
            }
            if (!CheckDate(card.DateReg.Trim()))
            {
                result.Add(CardValidateStatus.Дата_Регистрации);
            }
            if (!CheckDate(card.BirthDay.Trim()))
            {
                result.Add(CardValidateStatus.Дата_Рождения);
            }
            if (card.Address.Trim().Length == 0)
            {
                result.Add(CardValidateStatus.Место_Регистрации);
            }
            if (!CheckPhone(card.Phone.Trim()))
            {
                result.Add(CardValidateStatus.Телефон);
            }
            if (card.Passport.Trim().Length == 0)
            {
                result.Add(CardValidateStatus.Паспортные_Данные);
            }

            if (result.Count == 0)
            {
                result.Add(CardValidateStatus.OK);
            }

            return result.ToArray();
        }
        private static bool CheckFio(string value)
        {
            var s = value.Trim().Split(" ");
            return (s.Length == 2) || (s.Length == 3);
        }

        private static bool CheckDate(string value)
        {
            DateTime scheduleDate;
            return DateTime.TryParseExact(value.Trim(), new string[] { "dd.MM.yyyy", "d.M.yyyy" }, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate);
        }

        private static bool CheckPhone(string value)
        {
            return Regex.IsMatch(value, @"^\+?\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d\D*\d");
        }
    }
}