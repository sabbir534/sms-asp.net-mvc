using System.Collections.Generic;
using SMS.BOL;

namespace SMS.DAL
{
    public interface IPaymentRepository
    {
        IEnumerable<PaymentDetail> GetPaymentDetailsByStudent(int? id);
        void InsertPaymentByStudent(PaymentDetail paymentDetail);
        void Save();
    }
}