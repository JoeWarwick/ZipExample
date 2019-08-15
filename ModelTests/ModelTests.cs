using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZipExample.DBModel;

namespace ModelTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void EmailRequired()
        {
            var m = new User
            {
                UserId = Guid.NewGuid(),
                Name = "Test 1",
                EmailAddress = null,
                MonthlyExpenses = 3000,
                MonthlySalary = 4000
            };

            Assert.IsTrue(ValidateModel(m).Count > 0);
        }

        [TestMethod]
        public void ExpensesGreaterThanZero()
        {
            var m = new User
            {
                UserId = Guid.NewGuid(),
                Name = "Test 2",
                EmailAddress = "test@test.com",
                MonthlyExpenses = -3000,
                MonthlySalary = 4000
            };

            Assert.IsTrue(ValidateModel(m).Count > 0);
        }

        [TestMethod]
        public void SalaryGreaterThanZero()
        {
            var m = new User
            {
                UserId = Guid.NewGuid(),
                Name = "Test 2",
                EmailAddress = "test@test.com",
                MonthlyExpenses = 3000,
                MonthlySalary = -2000
            };

            Assert.IsTrue(ValidateModel(m).Count > 0);
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
