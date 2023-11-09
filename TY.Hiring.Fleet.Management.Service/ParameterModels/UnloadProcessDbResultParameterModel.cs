using TY.Hiring.Fleet.Management.Data.ORM.EF.Models;

namespace TY.Hiring.Fleet.Management.Service.ParameterModels
{
    internal class UnloadProcessDbResultParameterModel
    {
        public List<Package> Packages { get; set; }
        public List<Sack> Sacks { get; set; }
        public List<SackPackage> SackPackages { get; set; }
    }
}
