using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class DateDimension
    {
        public DateTime? TheDate { get; set; }
        public int? TheDay { get; set; }
        public string? TheDayName { get; set; }
        public int? TheDayOfWeek { get; set; }
        public byte? TheDayOfWeekInMonth { get; set; }
        public int? TheDayOfYear { get; set; }
        public int IsWeekend { get; set; }
        public int? TheWeek { get; set; }
        public int? TheIsoweek { get; set; }
        public DateTime? TheFirstOfWeek { get; set; }
        public DateTime? TheLastOfWeek { get; set; }
        public byte? TheWeekOfMonth { get; set; }
        public int? TheMonth { get; set; }
        public string? TheMonthName { get; set; }
        public DateTime? TheFirstOfMonth { get; set; }
        public DateTime? TheLastOfMonth { get; set; }
        public DateTime? TheFirstOfNextMonth { get; set; }
        public DateTime? TheLastOfNextMonth { get; set; }
        public int? TheQuarter { get; set; }
        public DateTime? TheFirstOfQuarter { get; set; }
        public DateTime? TheLastOfQuarter { get; set; }
        public int? TheYear { get; set; }
        public int? TheIsoyear { get; set; }
        public DateTime? TheFirstOfYear { get; set; }
        public DateTime? TheLastOfYear { get; set; }
        public bool? IsLeapYear { get; set; }
        public int Has53Weeks { get; set; }
        public int Has53Isoweeks { get; set; }
        public string? Mmyyyy { get; set; }
        public string? Style101 { get; set; }
        public string? Style103 { get; set; }
        public string? Style112 { get; set; }
        public string? Style120 { get; set; }
    }
}
