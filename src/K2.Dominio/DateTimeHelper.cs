﻿using System;

namespace K2.Dominio
{
    public static class DateTimeHelper
    {
        public static DateTime ObterHorarioAtualBrasilia() => TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
    }
}
