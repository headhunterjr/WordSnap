// <copyright file="UserServiceTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WordSnapWPFAppTests
{
    using WordSnapWPFApp.BLL.Services;

    /// <summary>
    /// UserService tests.
    /// </summary>
    public class UserServiceTests
    {
        /// <summary>
        /// Instance returns the same instance.
        /// </summary>
        [Fact]
        public void Instance_AlwaysReturnsTheSameInstance()
        {
            // Act
            var firstInstance = UserService.Instance;
            var secondInstance = UserService.Instance;

            // Assert
            Assert.Same(firstInstance, secondInstance);
        }

        /// <summary>
        /// IsUserLoggedIn returns false if the user is not logged in.
        /// </summary>
        [Fact]
        public void IsUserLoggedIn_ReturnsFalse_WhenNoUserIsLoggedIn()
        {
            // Arrange
            var service = UserService.Instance;
            service.Logout();

            // Act
            var result = service.IsUserLoggedIn;

            // Assert
            Assert.False(result);
        }
    }
}
