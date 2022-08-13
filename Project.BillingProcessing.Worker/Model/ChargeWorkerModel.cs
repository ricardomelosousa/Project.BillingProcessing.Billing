using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BillingProcessing.Worker.Model
{
    public class ChargeWorkerModel
    {
        public Int32 Identification { get; set; }
        public DateTime dueDate { get; set; }  
       
        public decimal ChargeValue = 4;
    }
}
