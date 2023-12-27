using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCSH2_Sem_Zoo.Model.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    class DataGridHiddenColumnAttribute : ValidationAttribute
    {
    }
}
