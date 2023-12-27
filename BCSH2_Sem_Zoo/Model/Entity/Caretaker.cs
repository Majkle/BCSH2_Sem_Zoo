using BCSH2_Sem_Zoo.Model.Attributes;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BCSH2_Sem_Zoo.Model.Entity
{

    [Description("Caretaker")]
    public class Caretaker : IComparable, IDataErrorInfo
    {
        [Key]
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string Firstname { get; set; }

        [DisplayName("Last Name")]
        public string Lastname { get; set; }

        [DisplayName("Sallary")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        public int Sallary { get; set; }

        public int CompareTo(Caretaker? other)
        {
            if (other == null) return 1;

            return Id.CompareTo(other.Id);
        }

        public int CompareTo(object? obj)
        {
            if (obj is Caretaker other) return Id.CompareTo(other.Id); 

            return 1;
        }

        public override string ToString()
        {
            return $"[{Id}] {Firstname} {Lastname}";
        }

        [DataGridHiddenColumn]
        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Firstname):
                        if (string.IsNullOrWhiteSpace(Firstname)) return "First Name is required.";
                        break;
                    case nameof(Lastname):
                        if (string.IsNullOrWhiteSpace(Lastname)) return "Last Name is required.";
                        break;
                }

                return "";
            }
        }
    }
}
