    namespace ProductApi.Models
    {
        public class Product
        {
            public int ProductID { get; set; } // Keep this for database mapping but it won't be required in the POST body
            public string ProductName { get; set; } = string.Empty; // Required in POST body
            public int CategoryID { get; set; } // Required in POST body
            public int SupplierID { get; set; } // Required in POST body
            public decimal UnitPrice { get; set; } // Required in POST body
            public int UnitsInStock { get; set; } // Required in POST body
            public bool Discontinued { get; set; } // Required in POST body
        }
    }
