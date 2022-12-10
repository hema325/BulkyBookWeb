namespace test.Models.ViewModels
{
    public class OrderVm
    {
        public OrderHeader orderHeader { get; set; }
        public IEnumerable<OrderDetail> orderDetails { get; set; }

    }
}
