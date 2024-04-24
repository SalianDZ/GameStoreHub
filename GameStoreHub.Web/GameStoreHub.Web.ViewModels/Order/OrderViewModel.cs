namespace GameStoreHub.Web.ViewModels.Order
{
    public class OrderViewModel
    {
        public string OrderId { get; set; } = null!;

        public string UserFullName { get; set; } = null!;

        public string TotalPrice { get; set; } = null!;

        public bool IsOrderCompleted { get; set; }
    }
}
