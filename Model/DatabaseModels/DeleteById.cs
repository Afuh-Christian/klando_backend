using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DatabaseModels
{
    public class DeleteById(string key , string value)
    {
        public string Key { get; set; } = key;
        public string Value { get; set; } = value;

    }
}
