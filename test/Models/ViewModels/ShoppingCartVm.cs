namespace test.Models.ViewModels
{
    public class ShoppingCartVm
    {
        public IEnumerable<ShoppingCart> cartList { get; set; }

        public OrderHeader orderHeader { get; set; }
    }
}
