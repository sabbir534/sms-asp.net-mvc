using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SMS.BOL;

namespace SMS.DAL
{
    public class PaymentRepository:IPaymentRepository
    {
        private readonly SMSDBEntities _smsdb;

        public PaymentRepository()
        {
            _smsdb = new SMSDBEntities();
        }
        public IEnumerable<PaymentDetail> GetPaymentDetailsByStudent(int? id)
        {
            return _smsdb.PaymentDetails.Include(g => g.Grade).Where(p => p.StudentId == id);
        }

        public void InsertPaymentByStudent(PaymentDetail paymentDetail)
        {
            paymentDetail.PaymentDate = DateTime.Now;
            _smsdb.PaymentDetails.Add(paymentDetail);
            Save();
        }

        public void Save()
        {
            _smsdb.SaveChanges();
        }
    }
}