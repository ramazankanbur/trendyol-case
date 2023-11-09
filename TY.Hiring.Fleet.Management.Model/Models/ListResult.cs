namespace TY.Hiring.Fleet.Management.Model.Models
{
    public class ListResult<T>
    {
        public List<T> List { get; set; }
        public int Count { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
