using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("Breeding Need Type")]
    public class BreedingNeedType : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {Name}";
        }

        public int CompareTo(object? obj)
        {
            if (obj is BreedingNeedType other) return Id.CompareTo(other.Id);

            return 1;
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
