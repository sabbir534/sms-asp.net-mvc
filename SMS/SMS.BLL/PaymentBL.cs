using System.Collections.Generic;
using SMS.BOL;
using SMS.DAL;

namespace SMS.BLL
{
    public class PaymentBL
    {
        private readonly IPaymentRepository _paymentObj;

        public PaymentBL()
        {
            _paymentObj = new PaymentRepository();
        }
        public IEnumerable<PaymentDetail> GetPaymentDetailsByStudent(int? id)
        {
            return _paymentObj.GetPaymentDetailsByStudent(id);
        }
        public void InsertPaymentByStudent(PaymentDetail paymentDetail)
        {
            _paymentObj.InsertPaymentByStudent(paymentDetail);
        }
    }
}