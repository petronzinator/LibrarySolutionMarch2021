using System;
using Xunit;

namespace LibraryApiIntegrationTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Given - Arrange
            int a = 10, b = 20, answer;
            // When - Act
            answer = a + b;
            // Then - Assert
            Assert.Equal(30, answer);
        }

        [Theory]
        [InlineData(10, 20, 30)]
        [InlineData(5, 5, 10)]
        [InlineData(20, 5, 25)]
        public void CanAdd(int a, int b, int expected)
        {
            var answer = a + b;
            Assert.Equal(expected, answer);
        }
    }
}
