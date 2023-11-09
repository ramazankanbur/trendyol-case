namespace TY.Hiring.Fleet.Management.Data.ORM.EF
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
