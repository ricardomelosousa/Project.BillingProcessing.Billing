namespace Project.BillingProcessing.Customer.Domain.Exceptions
{
    public class BillingException : Exception
    {
        public BillingException(string message) : base(message) { }   
        
    }
}
