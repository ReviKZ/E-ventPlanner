using E_ventPlanner.Contexts;
using E_ventPlanner.Enums;
using E_ventPlanner.Models;
using E_ventPlanner.Models.DTOs;
using E_ventPlanner.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace E_ventPlanner.Tests
{
    public class RegisterServiceTests
    {
        //Service fields
        public RegisterService _service;
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
            _service = new RegisterService(_userManager, _inMemoryDb);
        }

        [TearDown]
        public void Cleanup()
        {
            _inMemoryDb.Dispose();
        }

        [Test]
        public async Task RegisterUser_WithInvalidEmail_ReturnsFalse()
        {
            //Arrange
            RegisterDTO fakeUser = new RegisterDTO
            {
                Email = "invalid",
                Password = "Valid_Passw0rd",
                FirstName = "valid",
                LastName = "valid",
                Gender = 0
            };
            //Act
            var result = await _service.RegisterUser(fakeUser);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RegisterUser_WithInvalidPassword_ReturnsFalse()
        {
            //Arrange
            RegisterDTO fakeUser = new RegisterDTO
            {
                Email = "valid@domain.com",
                Password = "inv",
                FirstName = "valid",
                LastName = "valid",
                Gender = 0
            };
            //Act
            var result = await _service.RegisterUser(fakeUser);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task RegisterUser_WithValidCredentials_ReturnsTrue()
        {
            //Arrange
            RegisterDTO fakeUser = new RegisterDTO
            {
                Email = "valid@domain.com",
                Password = "Valid_Passw0rd",
                FirstName = "Valid",
                LastName = "Valid",
                Gender = 0
            };
            //Act
            var result = await _service.RegisterUser(fakeUser);

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RegisterUser_WithValidCredentials_DataInDBMatches()
        {
            //Arrange
            RegisterDTO fakeUser = new RegisterDTO
            {
                Email = "valid@domain.com",
                Password = "Valid_Passw0rd",
                FirstName = "Valid",
                LastName = "Valid",
                Gender = 0
            };

            string expectedEmail = fakeUser.Email;
            UserData expectedData = new UserData
            {
                FirstName = fakeUser.FirstName,
                LastName = fakeUser.LastName,
                Gender = fakeUser.Gender,
                Role = Role.User
            };

            //Act
            await _service.RegisterUser(fakeUser);
            string resultEmail = _inMemoryDb.Users.First(u => u.Email == fakeUser.Email).Email;
            string resultId = _inMemoryDb.Users.First(u => u.Email == fakeUser.Email).Id;
            UserData resultUser = _inMemoryDb.UserData.First(u => u.FirstName == fakeUser.FirstName);

            //Assert
            Assert.That(resultEmail, Is.EqualTo(expectedEmail));
            Assert.That(resultUser.FirstName, Is.EqualTo(expectedData.FirstName));
            Assert.That(resultUser.LastName, Is.EqualTo(expectedData.LastName));
            Assert.That(resultUser.Gender, Is.EqualTo(expectedData.Gender));
            Assert.That(resultUser.Role, Is.EqualTo(expectedData.Role));
            Assert.That(resultId, Is.EqualTo(resultUser.UserId));
        }
    }
}