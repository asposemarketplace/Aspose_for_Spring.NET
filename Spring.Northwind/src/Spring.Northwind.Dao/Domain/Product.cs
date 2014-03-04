#region License

/*
 * Copyright 2002-2007 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spring.Northwind.Domain
{
    public class Product : IProduct
    {
        int productID;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        string productName;

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        int supplierID;

        public int SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }
        int categoryID;

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        string quantityPerUnit;

        public string QuantityPerUnit
        {
            get { return quantityPerUnit; }
            set { quantityPerUnit = value; }
        }
        decimal unitPrice;

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        short unitsInStock;

        public short UnitsInStock
        {
            get { return unitsInStock; }
            set { unitsInStock = value; }
        }
        short unitsOnOrder;

        public short UnitsOnOrder
        {
            get { return unitsOnOrder; }
            set { unitsOnOrder = value; }
        }
        short reorderLevel;

        public short ReorderLevel
        {
            get { return reorderLevel; }
            set { reorderLevel = value; }
        }
        bool discontinued;

        public bool Discontinued
        {
            get { return discontinued; }
            set { discontinued = value; }
        }

        public Product()
        { 
        }

        public Product(string productName, int supplierID, int categoryID, string quantityPerUnit, decimal unitPrice, short unitsInStock, short unitsOnOrder, short reorderLevel, bool discontinued)
        {
            this.productName = productName;
            this.supplierID = supplierID;
            this.categoryID = categoryID;
            this.quantityPerUnit = quantityPerUnit;
            this.unitPrice = unitPrice;
            this.unitsInStock = unitsInStock;
            this.unitsOnOrder = unitsOnOrder;
            this.reorderLevel = reorderLevel;
            this.discontinued = discontinued;
        }
    }
}
