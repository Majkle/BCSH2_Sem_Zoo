using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("Animal")]
    public class Animal : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Nickname")]
        public string Nickname { get; set; }

        [DisplayName("Spieces")]
        public Spieces Spieces { get; set; }

        [DisplayName("Continent")]
        public string Origin { get; set; }

        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime Birthday { get; set; }

        [DisplayName("Date of Arrival")]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime Acquired { get; set; }

        [DisplayName("Caretaker")]
        public Caretaker? Caretaker { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is Animal other) return Id.CompareTo(other.Id);

            return 1;
        }

        public override string ToString()
        {
            return $"[{Id}] {Nickname}";
        }

        [DataGridHiddenColumn]
        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Nickname):
                        if (string.IsNullOrWhiteSpace(Nickname)) return "Name is required.";
                        break;
                    case nameof(Spieces):
                        if (Spieces == null) return "Spieces is required.";
                        break;
                    case nameof(Origin):
                        if (string.IsNullOrWhiteSpace(Origin)) return "Continent is required.";
                        break;
                }

                return "";
            }
        }
    }
}
