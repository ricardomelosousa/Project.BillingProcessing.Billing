using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BillingProcessing.Billing.Domain.BillingEntity
{
    public class Billing
    {
        public string Id { get; set; }
        public DateTime DueDate { get; set; }
        public string Month { get; set; }
        public decimal ChargeValue { get; set; }
        public long Identification { get; set; }
    }
}
