using E_ventPlanner.Contexts;
using E_ventPlanner.Models.DTOs;
using E_ventPlanner.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace E_ventPlanner.Tests;

public class LoginServiceTests
{
    //Service fields
    public LoginService _service;
    public PlannerDbContext _inMemoryDb;
    public UserManager<IdentityUser> _userManager;

    [SetUp]
    public void Setup()
    {
        //Setup DB
        var myDatabaseName = "mydatabase_" + DateTime.Now.ToFileTimeUtc();
        var DBoptions = new DbContextOptionsBuilder<PlannerDbContext>()
            .UseInMemoryDatabase(databaseName: myDatabaseName)
            .Options;
        _inMemoryDb = new PlannerDbContext(DBoptions);



        //Setup Mock UserManager
        var userStore = new UserStore<IdentityUser>(_inMemoryDb); // Provide your actual DbContext here
        var UMoptions = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var userValidators = new List<IUserValidator<IdentityUser>> { new UserValidator<IdentityUser>() };
        var passwordValidators = new List<PasswordValidator<IdentityUser>> { new PasswordValidator<IdentityUser>() };
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        var logger = loggerFactory.CreateLogger<UserManager<IdentityUser>>();

        _userManager = new UserManager<IdentityUser>(
            userStore,
            UMoptions.Object,
            passwordHasher,
            userValidators,
            passwordValidators,
            null,
            null,
            null,
            logger
        );

        //Initialize Service to be tested
        _service = new LoginService(_userManager);

        var user = new
        {
            Email = "valid@valid.com",
            Password = "Valid_Passw0rd"
        };

        var identityUser = new IdentityUser
        {
            UserName = user.Email,
            Email = user.Email
        };

        _userManager.CreateAsync(identityUser, user.Password);
    }

    [TearDown]
    public void Cleanup()
    {
        _inMemoryDb.Dispose();
    }

    [Test]
    public async Task LoginUser_WithInvalidEmail_ReturnsFalse()
    {
        //Arrange
        LoginDTO loginData = new LoginDTO
        {
            Email = "invalid",
            Password = "Valid_Passw0rd"
        };

        //Act
        var result = await _service.LoginUser(loginData);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task LoginUser_WithInvalidPwd_ReturnsFalse()
    {
        //Arrange
        LoginDTO loginData = new LoginDTO
        {
            Email = "valid@valid.com",
            Password = "Invalid_Passw0rd"
        };

        //Act
        var result = await _service.LoginUser(loginData);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task LoginUser_WithValidCredentials_ReturnsTrue()
    {
        //Arrange
        LoginDTO loginData = new LoginDTO
        {
            Email = "valid@valid.com",
            Password = "Valid_Passw0rd"
        };

        //Act
        var result = await _service.LoginUser(loginData);

        //Assert
        Assert.That(result, Is.True);
    }
}