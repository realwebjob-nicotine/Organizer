using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Enums
{
    /// <summary>
    /// Состояние
    /// </summary>
    public enum State
    {
        /// <summary>
        /// В процессе
        /// </summary>
        [Description("В процессе")]
        InProcess,
        /// <summary>
        /// Выполнена
        /// </summary>
        [Description("Выполнена")]
        Completed
    }
}
