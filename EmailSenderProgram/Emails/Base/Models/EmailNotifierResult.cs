using System.Collections.Generic;

namespace EmailSenderProgram.Emails.Base.Models
{
    public class EmailNotifierResult
    {
        private readonly List<Customer> _successfulRecipients;
        private readonly List<Customer> _failedRecipients;

        public EmailNotifierResult()
        {
            _successfulRecipients = new List<Customer>();
            _failedRecipients = new List<Customer>();
        }

        public void AddSuccess(Customer customer)
        {
            _successfulRecipients.Add(customer);
        }

        public void AddFailure(Customer customer)
        {
            _failedRecipients.Add(customer);
        }

        public bool IsSuccess()
        {
            return _failedRecipients.Count == 0;
        }

        public bool IsAnyFailure()
        {
            return !IsSuccess();
        }

        public List<Customer> GetSuccessfulRecipients()
        {
            return _successfulRecipients;
        }

        public List<Customer> GetFailedRecipients()
        {
            return _failedRecipients;
        }
    }
}
