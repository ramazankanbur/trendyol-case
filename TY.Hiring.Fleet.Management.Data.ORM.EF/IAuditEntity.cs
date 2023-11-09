namespace TY.Hiring.Fleet.Management.Data.ORM.EF
{
    internal interface IAuditEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
