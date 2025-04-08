using UserApi.Models;
using UserApi.Validators;

namespace UserApi.Tests
{
    public class UserValidatorTests
    {
        // Arrange
        // Act
        // Assert

        //[Fact]
        //public void UserValidatorTests_ValidNewUser()
        //{
        //    // Arrange
        //    User user = new User() 
        //    { 
        //        Id = 1, 
        //        CreatedDate = DateTime.Now, 
        //        FirstName = "First", 
        //        LastName = "Last", 
        //        Address1 = "123 Fake St.", 
        //        Address2 = "Apt #2", 
        //        City = "Fakeapolis", 
        //        State = "AL", 
        //        ZipCode = "12345" 
        //    };

        //    // Act
        //    bool valid = UserValidator.ValidateNewUser(user);

        //    // Assert
        //    Assert.True(valid);
        //}

        //[Fact]
        //public void UserValidatorTests_ValidExistingUser()
        //{
        //    // Arrange
        //    int id = 1;

        //    // Act
        //    bool valid = UserValidator.ValidateExistingUser(id);

        //    // Assert
        //    Assert.True(valid);
        //}

        //[Fact]
        //public void UserValidatorTests_InvalidExistingUser_IdLessThan0()
        //{
        //    // Arrange
        //    int id = -1;

        //    // Act
        //    Action testCode = () => { UserValidator.ValidateExistingUser(id); };
        //    var ex = Record.Exception(testCode);

        //    // Assert
        //    Assert.NotNull(ex);
        //    Assert.IsType<InvalidDataException>(ex);
        //}

        //[Fact]
        //public void UserValidatorTests_InvalidExistingUser_IdIs0()
        //{
        //    // Arrange
        //    int id = 0;

        //    // Act
        //    Action testCode = () => { UserValidator.ValidateExistingUser(id); };
        //    var ex = Record.Exception(testCode);

        //    // Assert
        //    Assert.NotNull(ex);
        //    Assert.IsType<InvalidDataException>(ex);
        //}

        //[Fact]
        //public void UserValidatorTests_InvalidExistingUser_IdGreaterThan99999()
        //{
        //    // Arrange
        //    int id = 999999;

        //    // Act
        //    Action testCode = () => { UserValidator.ValidateExistingUser(id); };
        //    var ex = Record.Exception(testCode);

        //    // Assert
        //    Assert.NotNull(ex);
        //    Assert.IsType<InvalidDataException>(ex);
        //}

        //[Fact]
        //public void UserValidatorTests_InvalidExistingUser_IdNotNumeric()
        //{
        //    // Arrange
        //    var id = 'A';

        //    // Act
        //    Action testCode = () => { UserValidator.ValidateExistingUser(id); };
        //    var ex = Record.Exception(testCode);

        //    // Assert
        //    Assert.NotNull(ex);
        //    Assert.IsType<InvalidOperationException>(ex);
        //}

        //[Fact]
        //public void UserValidatorTests_InvalidExistingUser_IdDoesNotExist()
        //{
        //    // Arrange
        //    int id = 6;

        //    // Act
        //    Action testCode = () => { UserValidator.ValidateExistingUser(id); };
        //    var ex = Record.Exception(testCode);

        //    // Assert
        //    Assert.NotNull(ex);
        //    Assert.IsType<InvalidOperationException>(ex);
        //}

    }
}