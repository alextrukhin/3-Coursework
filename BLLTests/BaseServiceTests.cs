using BLL;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLLTests
{
    public class TestProvider<T> : IProvider<T> where T : class
    {
        public List<T> data = new List<T>();
        public TestProvider(string fileName)
        {
        }
        public List<T> Load()
        {
            return data;
        }
        public void Save(List<T> listToSave)
        {
            data = listToSave;
        }
    }
    public class TestEntityContext<T> : EntityContext<T> where T : class, new()
    {
        public TestEntityContext()
        {
            DBType = "test";
            Provider = new TestProvider<T>(DBFile);
        }
        public new void SetProvider(string dbType, string dbName)
        {
            DBType = dbType;
            DBName = dbName;
        }
    }
    [TestClass()]
    public class BaseServiceTests
    {
        BaseService<Supplier> baseService = new(new TestEntityContext<Supplier>());
        Supplier supplier = new(0, "", [], "", "", "", "", "");
        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(supplier);
            // Assert
            Assert.AreEqual(length + 1, baseService.Length());
            Assert.AreEqual(baseService[length], supplier);
            // Clear traces
            baseService.Delete(length);
        }
        [TestMethod()]
        public void UpdateTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(new Supplier());
            baseService.Update(supplier, length);
            // Assert
            Assert.AreEqual(length + 1, baseService.Length());
            Assert.AreEqual(baseService[length], supplier);
            // Clear traces
            baseService.Delete(length);
        }
        [TestMethod()]
        public void DeleteTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(supplier);
            baseService.Delete(length);
            // Assert
            Assert.AreEqual(length, baseService.Length());
        }
        [TestMethod()]
        public void UpdateByIdTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(new Supplier(0, "", [], "", "", "", "", ""));
            baseService.UpdateById(supplier, (supplier) => supplier.Id == 0);
            // Assert
            Assert.AreEqual(length + 1, baseService.Length());
            Assert.AreEqual(baseService[length], supplier);
            // Clear traces
            baseService.Delete(length);
        }
        [TestMethod()]
        public void GetByIdTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(supplier);
            // Assert
            Assert.AreEqual(length + 1, baseService.Length());
            Assert.AreEqual(baseService.GetById((s) => s.Id == supplier.Id), supplier);
            // Clear traces
            baseService.Delete(length);
        }
        [TestMethod()]
        public void DeleteByIdTest()
        {
            // Arrange
            int length = baseService.Length();
            // Act
            baseService.Insert(supplier);
            baseService.DeleteById((supplier) => supplier.Id == 0);
            // Assert
            Assert.AreEqual(length, baseService.Length());
        }
        [TestMethod()]
        public void SortTest()
        {
            // Arrange
            List<Supplier> list = new();
            Supplier supplier1 = new Supplier(1, "Abc", [], "", "", "", "", "");
            Supplier supplier2 = new Supplier(2, "Cde", [], "", "", "", "", "");
            Supplier supplier3 = new Supplier(3, "Bcd", [], "", "", "", "", "");
            // Act
            baseService.Insert(supplier1);
            baseService.Insert(supplier2);
            baseService.Insert(supplier3);
            list = baseService.Sort(SupplierService.SortByNameComparator);
            // Assert
            Assert.AreEqual(list[0], supplier1);
            Assert.AreEqual(list[1], supplier3);
            Assert.AreEqual(list[2], supplier2);
            // Clear traces
            baseService.Delete(0);
            baseService.Delete(0);
            baseService.Delete(0);
        }
        [TestMethod()]
        public void SearchTest()
        {
            // Arrange
            List<Tuple<int, Supplier>> list = new();
            Supplier supplier1 = new Supplier(1, "Abc", [], "", "", "", "", "");
            Supplier supplier2 = new Supplier(2, "Cde", [], "", "", "", "", "");
            Supplier supplier3 = new Supplier(3, "Bcd", [], "", "", "", "", "");
            // Act
            baseService.Insert(supplier1);
            baseService.Insert(supplier2);
            baseService.Insert(supplier3);
            list = baseService.Search((sup) => sup.Id > 1);
            // Assert
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0].Item2, supplier2);
            Assert.AreEqual(list[1].Item2, supplier3);
            // Clear traces
            baseService.Delete(0);
            baseService.Delete(0);
            baseService.Delete(0);
        }
        [TestMethod()]
        public void DisposeTest()
        {
            try {  baseService.Dispose(); } catch { Assert.Fail(); }
        }
        [TestMethod()]
        public void DataTest()
        {
            // Arrange
            List<Supplier> list = new();
            Supplier supplier1 = new Supplier(1, "Abc", [], "", "", "", "", "");
            Supplier supplier2 = new Supplier(2, "Cde", [], "", "", "", "", "");
            // Act
            baseService.Insert(supplier1);
            baseService.Insert(supplier2);
            list = baseService.Data;
            // Assert
            Assert.AreEqual(list.Count, 2);
            Assert.AreEqual(list[0], supplier1);
            Assert.AreEqual(list[1], supplier2);
            // Clear traces
            baseService.Delete(0);
            baseService.Delete(0);
        }
        [TestMethod()]
        public void LengthTest()
        {
            // Arrange
            // Act
            baseService.Insert(supplier);
            // Assert
            Assert.AreEqual(baseService.Length(), 1);
            // Clear traces
            baseService.Delete(0);
        }
    }
}