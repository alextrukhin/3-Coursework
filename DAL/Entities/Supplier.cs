using System.Net;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DAL
{
    public class SupplierCreate
    {
        [XmlElement]
        public string? Name { get; set; }
        [XmlElement]
        public string? LastName { get; set; }
        [XmlElement]
        public double[] ProductIds { get; set; }
        [XmlElement]
        public string? Description { get; set; }
        [XmlElement]
        public string? Email { get; set; }
        [XmlElement]
        public string? Phone { get; set; }
        [XmlElement]
        public string? Address { get; set; }
        public SupplierCreate() { }
        public SupplierCreate(string? name, double[]? productIds, string? lastName, string? description, string? email, string? phone, string? address)
        {
            Name = name;
            ProductIds = productIds ?? [];
            LastName = lastName;
            Description = description;
            Email = email;
            Phone = phone;
            Address = address;
        }
        public override string ToString() => Name;
    }
    [Serializable]
    public class Supplier: SupplierCreate
    {
        [XmlElement]
        public double Id { get; set; }
        public Supplier() { }
        [JsonConstructor]
        public Supplier(double id, string? name, double[]? productIds, string? lastName, string? description, string? email, string? phone, string? address)
            :base(name, productIds, lastName, description, email, phone, address)
        {
            Id = id;
        }
        public Supplier(SupplierCreate supplier, double id)
        : this(id, supplier.Name, supplier.ProductIds, supplier.LastName, supplier.Description, supplier.Email, supplier.Phone, supplier.Address)
        {
        }
    }
}
