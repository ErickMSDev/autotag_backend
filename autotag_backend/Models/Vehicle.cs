using System;
using System.Collections.Generic;

namespace AutoTagBackEnd.Models
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public int PatentGroupId { get; set; }
        public string Patent { get; set; } = null!;
        public string? DownloadedPatent { get; set; }
        public string? OwnerRun { get; set; }
        public string? OwnerName { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public string? Color { get; set; }
        public string? NMotor { get; set; }
        public string? NChasis { get; set; }
        public string? Fines { get; set; }

        public virtual PatentGroup PatentGroup { get; set; } = null!;
    }
}
