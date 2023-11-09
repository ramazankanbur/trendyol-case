namespace TY.Hiring.Fleet.Management.Model.Models.Dtos
{
    public class LogDTO
    {
        public string Message { get; set; }
        // Load, Unload ..
        public string LogName { get; set; }
        public int DeliveryPointId { get; set; }
        public string Barcode { get; set; }
    }
}
