using test.Data;
using test.Models;
using test.Repository.IRepository;

namespace test.Repository
{
    public class RepositoryOrderHeader:Repository<OrderHeader>,IRepositoryOrderHeader
    {
        private readonly ApplicationDbContext _context;
        public RepositoryOrderHeader(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(OrderHeader entity)
        {
            _context.OrderHeaders.Update(entity);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus)
        {
            var obj = _context.OrderHeaders.Find(id);
            obj.OrderStatus = orderStatus;
            if (paymentStatus != null||paymentStatus!="")
                obj.PaymentStatus = paymentStatus;
        }

        public void UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId)
        {
            var obj = _context.OrderHeaders.FirstOrDefault(o =>o.Id == id);
            obj.SessionId = SessionId;
            obj.PaymentIntentId = PaymentIntentId;
            _context.OrderHeaders.Update(obj);
        }
    }
}
