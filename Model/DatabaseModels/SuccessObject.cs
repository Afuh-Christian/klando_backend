using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DatabaseModels
{
    public class SuccessObject
    {

        public SuccessObject() {
            this.Value = true;
            this.Error = null;
        }

        public SuccessObject(bool value , string error) { 
        
            this.Value = value;
            this.Error = error;
        }


        public bool Value { get; set; }

        public string Error { get; set; }

       // public IEnumerable<T> List { get; set; }
    }
}
