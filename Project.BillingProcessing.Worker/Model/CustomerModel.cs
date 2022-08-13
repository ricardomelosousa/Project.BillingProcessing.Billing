using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BillingProcessing.Worker.Model
{
    public class CustomerModel
    {
        public string Name { get; set; }
        public string State { get; set; }
        public long Identification { get; set; }
    }
}
