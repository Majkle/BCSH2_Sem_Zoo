using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{
    [Description("Spieces")]
    public class Spieces : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is Spieces other) return Id.CompareTo(other.Id);

            return 1;
        }

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }

        [DataGridHiddenColumn]
        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name)) return "Name is required.";
                        break;
                }

                return "";
            }
        }
    }
}
