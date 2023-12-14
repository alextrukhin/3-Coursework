using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLLTests;
using DAL;

namespace BLL.Tests
{
    [TestClass()]
    public class ProductServiceTests
    {
        ProductService productService = new(new TestEntityContext<Product>());
        Product product = new(0, 1, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            int length = productService.Length();
            ProductCreate productCreate = new();
            // Act
            productService.Insert(productCreate);
            productService.Insert(productCreate);
            // Assert
            Assert.AreEqual(length + 2, productService.Length());
            Assert.AreEqual(productService[length].Id, 1);
            Assert.AreEqual(productService[length + 1].Id, 2);
            // Clear traces
            productService.Delete(length);
            productService.Delete(length);
        }
        [TestMethod()]
        public void FindByCategoryIdTest()
        {
            // Arrange
            Product product = new(0, 1, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product1 = new(1, 2, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            productService.Insert(product);
            productService.Insert(product1);
            productService.Insert(product);
            List<Product> fact = new();
            // Act
            fact = productService.FindByCategoryId(1);
            // Assert
            Assert.AreEqual(2, fact.Count);
            Assert.AreEqual(1, fact[0].Id);
            Assert.AreEqual(3, fact[1].Id);
            // Clear traces
            productService.Delete(0);
            productService.Delete(0);
            productService.Delete(0);
        }
        [TestMethod()]
        public void CategoryRemovedTest()
        {
            // Arrange
            Product product = new(0, 1, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product1 = new(1, 2, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            productService.Insert(product);
            productService.Insert(product1);
            productService.Insert(product);
            // Act
            productService.CategoryRemoved(1);
            // Assert
            Assert.AreEqual(3, productService.Length());
            Assert.AreEqual(null, productService[0].CategoryId);
            Assert.AreEqual(2, productService[1].CategoryId);
            Assert.AreEqual(null, productService[2].CategoryId);
            // Clear traces
            productService.Delete(0);
            productService.Delete(0);
            productService.Delete(0);
        }
        [TestMethod()]
        public void FindByIdTest()
        {
            // Arrange
            Product product = new(1, 1, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product1 = new(2, 2, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            productService.Insert(product);
            productService.Insert(product1);
            Product? fact = null;
            // Act
            fact = productService.FindById(2);
            // Assert
            Assert.AreEqual(2, fact?.CategoryId);
            // Clear traces
            productService.Delete(0);
            productService.Delete(0);
        }
        [TestMethod()]
        public void FindByIdsTest()
        {
            // Arrange
            Product product = new(1, 1, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product1 = new(2, 2, "", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            productService.Insert(product);
            productService.Insert(product1);
            productService.Insert(product);
            List<Product> fact = new();
            // Act
            fact = productService.FindByIds(new List<double>([1,3]).ToArray());
            // Assert
            Assert.AreEqual(2, fact.Count);
            Assert.AreEqual(1, fact[0].CategoryId);
            Assert.AreEqual(1, fact[1].CategoryId);
            // Clear traces
            productService.Delete(0);
            productService.Delete(0);
            productService.Delete(0);
        }
        [TestMethod()]
        public void Search()
        {
            // Arrange
            Product product = new(1, 1, "name", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product1 = new(2, 2, "", "Brand name", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product2 = new(3, 2, "", "", 0, "Description Here yup", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            productService.Insert(product);
            productService.Insert(product1);
            productService.Insert(product2);
            List<Product> fact1 = new();
            List<Product> fact2 = new();
            List<Product> fact3 = new();
            // Act
            fact1 = productService.Search("name");
            fact2 = productService.Search("brand");
            fact3 = productService.Search("here");
            // Assert
            Assert.AreEqual(2, fact1.Count);
            Assert.AreEqual(1, fact2.Count);
            Assert.AreEqual(1, fact3.Count);
            Assert.AreEqual(1, fact1[0].Id);
            Assert.AreEqual(2, fact1[1].Id);
            Assert.AreEqual(2, fact2[0].Id);
            Assert.AreEqual(3, fact3[0].Id);
            // Clear traces
            productService.Delete(0);
            productService.Delete(0);
        }
        [TestMethod()]
        public void SortByNameComparatorTest()
        {
            // Arrange
            Product product1 = new(1, 1, "name1", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product2 = new(1, 1, "name2", "", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            // Act
            int resp1 = ProductService.SortByNameComparator(product1, product2);
            int resp2 = ProductService.SortByNameComparator(product2, product1);
            int resp3 = ProductService.SortByNameComparator(product1, product1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
        [TestMethod()]
        public void SortByBrandComparatorTest()
        {
            // Arrange
            Product product1 = new(1, 1, "", "name1", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product2 = new(1, 1, "", "name2", 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            // Act
            int resp1 = ProductService.SortByBrandComparator(product1, product2);
            int resp2 = ProductService.SortByBrandComparator(product2, product1);
            int resp3 = ProductService.SortByBrandComparator(product1, product1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
        [TestMethod()]
        public void SortByPriceComparatorTest()
        {
            // Arrange
            Product product1 = new(1, 1, "", "", 1, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Product product2 = new(1, 1, "", "", 2, "", 0, 0, 0, 0, 0, 0, 0, 0, 0);
            // Act
            int resp1 = ProductService.SortByPriceComparator(product1, product2);
            int resp2 = ProductService.SortByPriceComparator(product2, product1);
            int resp3 = ProductService.SortByPriceComparator(product1, product1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
    }
}