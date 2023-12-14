using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using BLLTests;

namespace BLL.Tests
{
    [TestClass()]
    public class CategoryServiceTests
    {
        CategoryService categoryService = new(new TestEntityContext<Category>());
        Category category = new(0, "category", ["Warranty"]);
        [TestMethod()]
        public void InsertTest()
        {
            // Arrange
            int length = categoryService.Length();
            CategoryCreate categoryCreate = new CategoryCreate();
            // Act
            categoryService.Insert(categoryCreate);
            categoryService.Insert(categoryCreate);
            // Assert
            Assert.AreEqual(length + 2, categoryService.Length());
            Assert.AreEqual(categoryService[length].Id, 1);
            Assert.AreEqual(categoryService[length+1].Id, 2);
            // Clear traces
            categoryService.Delete(length);
            categoryService.Delete(length);
        }
        [TestMethod()]
        public void FindByIdTest()
        {
            // Arrange
            int length = categoryService.Length();
            Category? inserted = null;
            // Act
            inserted = categoryService.Insert(category);
            // Assert
            Assert.AreEqual(inserted, categoryService.FindById(inserted.Id));
            // Clear traces
            categoryService.Delete(length);
        }
        [TestMethod()]
        public void GetFieldsTest()
        {
            // Arrange
            List<Tuple<string, string>> expected = new([new Tuple<string,string>("Warranty", "Int32")]);
            List<Tuple<string, string>> fact = new();
            // Act
            fact = CategoryService.GetFields(category);
            // Assert
            Assert.AreEqual(expected.Count, fact.Count);
            Assert.AreEqual(expected[0].Item1, fact[0].Item1);
            Assert.AreEqual(expected[0].Item2, fact[0].Item2);
        }
        [TestMethod()]
        public void DeleteByIdTest()
        {
            // Arrange
            int length = categoryService.Length();
            // Act
            categoryService.Insert(category);
            categoryService.DeleteById(1);
            // Assert
            Assert.AreEqual(length, categoryService.Length());
        }
        [TestMethod()]
        public void SortByNameComparatorTest()
        {
            // Arrange
            Category category1 = new(2, "category1", ["Warranty"]);
            Category category2 = new(1, "category2", ["Warranty"]);
            // Act
            int resp1 = CategoryService.SortByNameComparator(category1, category2);
            int resp2 = CategoryService.SortByNameComparator(category2, category1);
            int resp3 = CategoryService.SortByNameComparator(category1, category1);
            // Assert
            Assert.AreEqual(-1, resp1);
            Assert.AreEqual(1, resp2);
            Assert.AreEqual(0, resp3);
        }
    }
}