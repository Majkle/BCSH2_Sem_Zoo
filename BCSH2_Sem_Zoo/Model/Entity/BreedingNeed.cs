using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("Breeding Need")]
    public class BreedingNeed : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Manufacturer")]
        public string Manufacturer { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Type")]
        public BreedingNeedType BreedingNeedType { get; set; }

        [DisplayName("Available")]
        public bool IsAvailable { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is BreedingNeed other) return Id.CompareTo(other.Id);

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
                    case nameof(Manufacturer):
                        if (string.IsNullOrWhiteSpace(Manufacturer)) return "Manufacturer is required.";
                        break;
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(Name)) return "Name is required.";
                        break;
                    case nameof(BreedingNeedType):
                        if (BreedingNeedType == null) return "Type is required.";
                        break;
                }

                return "";
            }
        }
    }
}
