namespace TY.Hiring.Fleet.Management.Model.Models.Requests
{
    public class DistributeRequest
    {
        public DistributeRequest()
        {
            Route = new List<RouteItem>();
        }
        public List<RouteItem> Route { get; set; }

        public class RouteItem
        {
            public RouteItem()
            {
                Deliveries = new List<Delivery>();
            }

            public int DeliveryPoint { get; set; }
            public List<Delivery> Deliveries { get; set; }
        }

        public class Delivery
        {
            public string Barcode { get; set; }
        }
    }
}
