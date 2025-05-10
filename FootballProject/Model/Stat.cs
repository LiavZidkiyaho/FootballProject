using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballProject.Model
{
    public class Stat : BaseEntity
    {
        private string name;
        private int value;

        public string Name { get => name; set => name = value; }
        public int Value { get => value; set => this.value = value; }
    }
}
