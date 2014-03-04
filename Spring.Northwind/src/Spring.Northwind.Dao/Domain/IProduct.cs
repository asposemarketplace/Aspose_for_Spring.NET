using System.Collections.ObjectModel;

namespace Spring.Northwind.Domain
{
    /// <summary>
    /// Interface for product entity data.
    /// </summary>
    public interface IProduct
    {
        int ProductID { get; set; }
        string ProductName { get; set; }
        int SupplierID { get; set; }
        int CategoryID { get; set; }
        string QuantityPerUnit { get; set; }
        decimal UnitPrice { get; set; }
        short UnitsInStock { get; set; }
        short UnitsOnOrder { get; set; }
        short ReorderLevel { get; set; }
        bool Discontinued { get; set; }
    }
}