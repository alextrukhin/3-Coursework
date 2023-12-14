using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DAL
{
    public class ProductCreate
    {
        [XmlElement]
        public double? CategoryId { get; set; }
        [XmlElement]
        public string? Name { get; set; }
        [XmlElement]
        public string? Brand { get; set; }
        [XmlElement]
        public float? Price { get; set; }
        [XmlElement]
        public string? Description { get; set; }
        [XmlElement]
        public int? InStock { get; set; }
        [XmlElement]
        public float? Width { get; set; }
        [XmlElement]
        public float? Height { get; set; }
        [XmlElement]
        public float? Length { get; set; }
        [XmlElement]
        public float? Weight { get; set; }
        [XmlElement]
        public float? Capacity { get; set; }
        [XmlElement]
        public int? Warranty { get; set; }
        [XmlElement]
        public int? Speed { get; set; }
        [XmlElement]
        public int? Power { get; set; }
        public ProductCreate() { }

        public ProductCreate(double? categoryId, string? name, string? brand, float? price, string? description, int? inStock, float? width, float? height, float? length, float? weight, float? capacity, int? warranty, int? speed, int? power)
        {
            CategoryId = categoryId;
            Name = name;
            Brand = brand;
            Price = price;
            Description = description;
            InStock = inStock;
            Width = width;
            Height = height;
            Length = length;
            Weight = weight;
            Capacity = capacity;
            Warranty = warranty;
            Speed = speed;
            Power = power;
        }

        public override string ToString() => Name;
    }
    [Serializable]
    public class Product : ProductCreate
    {
        [XmlElement]
        public double? Id { get; set; }
        public Product() { }
        [JsonConstructor]
        public Product(double? id, double? categoryId, string? name, string? brand, float? price, string? description, int? inStock, float? width, float? height, float? length, float? weight, float? capacity, int? warranty, int? speed, int? power)
            : base(categoryId, name, brand, price, description, inStock, width, height, length, weight, capacity, warranty, speed, power) { Id = id; }
        public Product(ProductCreate product, double id)
        : this(id, product.CategoryId, product.Name, product.Brand, product.Price, product.Description, product.InStock, product.Width, product.Height, product.Length, product.Weight, product.Capacity, product.Warranty, product.Speed, product.Power)
        {
        }
        public override string ToString() => Name;
    }
}
