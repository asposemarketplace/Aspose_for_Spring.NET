using System.Collections.ObjectModel;

namespace Spring.Northwind.Domain
{
    /// <summary>
    /// Interface for product entity data.
    /// </summary>
    public interface ICategory
    {
        int CategoryID { get; set; }
        string CategoryName { get; set; }
        string Description { get; set; }
        string Picture { get; set; }
    }
}