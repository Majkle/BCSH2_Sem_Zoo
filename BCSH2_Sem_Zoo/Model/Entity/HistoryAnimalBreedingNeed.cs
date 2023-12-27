using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("History of Animals and Breeding Needs")]
    public class HistoryAnimalBreedingNeed : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Animal")]
        public Animal Animal { get; set; }

        [DisplayName("Breeding Need")]
        public BreedingNeed BreedingNeed { get; set; }

        [DisplayName("Date of Administration")]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime Served { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is HistoryAnimalBreedingNeed other) return Id.CompareTo(other.Id);

            return 1;
        }

        public override string ToString()
        {
            return $"[{Id}] {Animal} - {BreedingNeed}";
        }

        [DataGridHiddenColumn]
        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Animal):
                        if (Animal == null) return "Animal is required.";
                        break;
                    case nameof(BreedingNeed):
                        if (BreedingNeed == null) return "Breeding Need is required.";
                        break;
                }

                return "";
            }
        }
    }
}
