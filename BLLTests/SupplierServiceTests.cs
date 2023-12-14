using BLLTests;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLL.Tests
{
    [TestClass()]
    public class SupplierServiceTests
    {
        SupplierService supplierService = new(new TestEntityContext<Supplier>());
        Supplier supplier = new();
        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            int length = supplierService.Length();
            SupplierCreate supplierCreate = new();
            // Act
            supplierService.Insert(supplierCreate);
            supplierService.Insert(supplierCreate);
            // Assert
            Assert.AreEqual(length + 2, supplierService.Length());
            Assert.AreEqual(supplierService[length].Id, 1);
            Assert.AreEqual(supplierService[length + 1].Id, 2);
            // Clear traces
            supplierService.Delete(length);
            supplierService.Delete(length);
        }
        [TestMethod()]
        public void UpdateByIdTest()
        {
            // Arrange
            int length = supplierService.Length();
            // Act
            supplierService.Insert(new Supplier(1, "", [], "", "", "", "", ""));
            supplierService.UpdateById(supplier, (supplier) => supplier.Id == 1);
            // Assert
            Assert.AreEqual(length + 1, supplierService.Length());
            Assert.AreEqual(supplierService[length], supplier);
            // Clear traces
            supplierService.Delete(length);
        }
        [TestMethod()]
        public void FindByIdTest()
        {
            // Arrange
            int length = supplierService.Length();
            Supplier? inserted = null;
            // Act
            inserted = supplierService.Insert(supplier);
            // Assert
            Assert.AreEqual(inserted, supplierService.FindById(inserted.Id));
            // Clear traces
            supplierService.Delete(length);
        }
        [TestMethod()]
        public void ProductRemovedTest()
        {
            // Arrange
            int length = supplierService.Length();
            Supplier supplier1 = new(1, "", new List<double>([1,2,3]).ToArray(), "", "","","","");
            // Act
            supplierService.Insert(supplier1);
            supplierService.ProductRemoved(2);
            // Assert
            Assert.AreEqual(supplierService[0].Id, 1);
            Assert.AreEqual(supplierService[0].ProductIds[0], 1);
            Assert.AreEqual(supplierService[0].ProductIds[1], 3);
            // Clear traces
            supplierService.Delete(0);
        }
        [TestMethod()]
        public void HaveProductTest()
        {
            // Arrange
            int length = supplierService.Length();
            Supplier supplier1 = new(1, "", new List<double>([1, 2, 3]).ToArray(), "", "", "", "", "");
            List<Supplier> fact = new();
            // Act
            supplierService.Insert(supplier1);
            supplierService.Insert(supplier1);
            fact = supplierService.HaveProduct(2);
            // Assert
            Assert.AreEqual(fact.Count, 2);
            Assert.AreEqual(fact[0].Id, 1);
            Assert.AreEqual(fact[1].Id, 2);
            // Clear traces
            supplierService.Delete(0);
        }
        [TestMethod()]
        public void Search()
        {
            // Arrange
            Supplier supplier1 = new(1, "Name", [], "", "", "", "", "");
            Supplier supplier2 = new(2, "Name", [], "Last name", "", "", "", "");
            Supplier supplier3 = new(3, "", [], "", "description", "", "", "");
            Supplier supplier4 = new(4, "", [], "", "", "123@gmail.com", "", "");
            Supplier supplier5 = new(5, "", [], "", "", "", "+38093", "");
            Supplier supplier6 = new(6, "", [], "", "", "", "", "123 street, Ukraine");
            supplierService.Insert(supplier1);
            supplierService.Insert(supplier2);
            supplierService.Insert(supplier3);
            supplierService.Insert(supplier4);
            supplierService.Insert(supplier5);
            supplierService.Insert(supplier6);
            List<Supplier> fact1 = new();
            List<Supplier> fact2 = new();
            List<Supplier> fact3 = new();
            List<Supplier> fact4 = new();
            List<Supplier> fact5 = new();
            List<Supplier> fact6 = new();
            // Act
            fact1 = supplierService.Search("name");
            fact2 = supplierService.Search("last name");
            fact3 = supplierService.Search("descr");
            fact4 = supplierService.Search("@gmail.com");
            fact5 = supplierService.Search("+38");
            fact6 = supplierService.Search("ukraine");
            // Assert
            Assert.AreEqual(2, fact1.Count);
            Assert.AreEqual(1, fact2.Count);
            Assert.AreEqual(1, fact3.Count);
            Assert.AreEqual(1, fact4.Count);
            Assert.AreEqual(1, fact5.Count);
            Assert.AreEqual(1, fact6.Count);

            Assert.AreEqual(1, fact1[0].Id);
            Assert.AreEqual(2, fact1[1].Id);
            Assert.AreEqual(2, fact2[0].Id);
            Assert.AreEqual(3, fact3[0].Id);
            Assert.AreEqual(4, fact4[0].Id);
            Assert.AreEqual(5, fact5[0].Id);
            Assert.AreEqual(6, fact6[0].Id);
            // Clear 
            supplierService.Delete(0);
            supplierService.Delete(0);
            supplierService.Delete(0);
            supplierService.Delete(0);
            supplierService.Delete(0);
            supplierService.Delete(0);
        }
        [TestMethod()]
        public void SortByNameComparatorTest()
        {
            // Arrange
            Supplier supplier1 = new(1, "Name1", [], "", "", "", "", "");
            Supplier supplier2 = new(1, "Name2", [], "", "", "", "", "");
            // Act
            int resp1 = SupplierService.SortByNameComparator(supplier1, supplier2);
            int resp2 = SupplierService.SortByNameComparator(supplier2, supplier1);
            int resp3 = SupplierService.SortByNameComparator(supplier1, supplier1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
        [TestMethod()]
        public void SortByLastNameComparatorTest()
        {
            // Arrange
            Supplier supplier1 = new(1, "", [], "last1", "", "", "", "");
            Supplier supplier2 = new(1, "", [], "last2", "", "", "", "");
            // Act
            int resp1 = SupplierService.SortByLastNameComparator(supplier1, supplier2);
            int resp2 = SupplierService.SortByLastNameComparator(supplier2, supplier1);
            int resp3 = SupplierService.SortByLastNameComparator(supplier1, supplier1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
    }
}