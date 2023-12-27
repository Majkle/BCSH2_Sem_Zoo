using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("Show")]
    public class Show : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Animal")]
        public Animal Animal { get; set; }

        [DisplayName("Caretaker")]
        public Caretaker Caretaker { get; set; }

        [DisplayName("Event Time")]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy HH:mm")]
        public DateTime Date { get; set; }

        public int CompareTo(object? obj)
        {
            if (obj is Show other) return Id.CompareTo(other.Id);

            return 1;
        }

        public override string ToString()
        {
            return $"[{Id}] {Animal} - {Caretaker}";
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
                    case nameof(Caretaker):
                        if (Caretaker == null) return "Caretakaer is required.";
                        break;
                }

                return "";
            }
        }
    }
}
