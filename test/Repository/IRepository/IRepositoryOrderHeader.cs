using test.Models;

namespace test.Repository.IRepository
{
    public interface IRepositoryOrderHeader:IRepository<OrderHeader>
    {
        void Update(OrderHeader entity);
        void UpdateStatus(int id, string orderStatus, string paymentStatus);
        void UpdateStripePaymentId(int id, string SessionId, string PaymentIntentId);

    }
}
