using System;
using Xunit;
using backend;

namespace backend.Tests
{
    public class OrderValidatorTests
    {
        [Fact]
        public void ValidOrder_ReturnsTrue()
        {
            var order = new Order
            {
                ProductType = "B",
                Quantity = 2,
                HasSignature = true,
                OrderDate = DateTime.Now
            };

            var validator = new OrderValidator();
            Assert.True(validator.IsValid(order));
        }

        [Fact]
        public void InvalidOrder_MissingSignature_ReturnsFalse()
        {
            var order = new Order
            {
                ProductType = "A",
                Quantity = 1,
                HasSignature = false,
                OrderDate = DateTime.Now
            };

            var validator = new OrderValidator();
            Assert.False(validator.IsValid(order));
        }

        [Fact]
        public void InvalidOrder_WrongType_ReturnsFalse()
        {
            var order = new Order
            {
                ProductType = "X",
                Quantity = 1,
                HasSignature = true,
                OrderDate = DateTime.Now
            };

            var validator = new OrderValidator();
            Assert.False(validator.IsValid(order));
        }
    }
}
