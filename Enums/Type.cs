using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Enums
{
    /// <summary>
    /// Тип документа
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// Документ
        /// </summary>
        [Description("Документ")]
        Document,
        /// <summary>
        /// Задача
        /// </summary>
        [Description("Задача")]
        Task
    }
}
