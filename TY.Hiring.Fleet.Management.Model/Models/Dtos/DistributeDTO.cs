namespace TY.Hiring.Fleet.Management.Model.Models.Dtos
{
    public class DistributeDTO
    {
        public DistributeDTO()
        {
            Route = new List<RouteItem>();
        }
        
        public string Vehicle { get; set; }
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
            public int State { get; set; }
        }

    }
}
