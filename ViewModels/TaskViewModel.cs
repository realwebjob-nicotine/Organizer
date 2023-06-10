﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.ViewModels
{
    public class TaskViewModel : Screen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public Enum State { get; set; }
    }
}
