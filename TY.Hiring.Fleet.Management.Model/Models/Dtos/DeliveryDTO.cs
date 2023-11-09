namespace TY.Hiring.Fleet.Management.Model.Models.Dtos
{
    public class DeliveryDTO
    {
        public string Barcode { get; set; }
        // delivery (package and sack) state. 1 for created
        public int State { get; set; } = 1; 
        public bool IsUnloaded { get; set; } = true;
    }
}
