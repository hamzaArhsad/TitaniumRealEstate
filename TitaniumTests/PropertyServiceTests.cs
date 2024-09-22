using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Domain;
using Application;
using Infrastructure;

namespace TitaniumTests
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private Mock<InterfaceGeneric<Property>> _mockGenericRepository;
        private Mock<InterfaceProperty> _mockPropertyRepository;
        private PropertyService _propertyService;

        [SetUp]
        public void Setup()
        {
            _mockGenericRepository = new Mock<InterfaceGeneric<Property>>();
            _mockPropertyRepository = new Mock<InterfaceProperty>();
            _propertyService = new PropertyService(_mockGenericRepository.Object, _mockPropertyRepository.Object);
        }

        [Test]
        public void Add_CallsAddOnGenericRepository()
        {
            // Arrange
            var property = new Property { Id = 1, Location = "Location1" };

            // Act
            _propertyService.Add(property);

            // Assert
            _mockGenericRepository.Verify(repo => repo.Add(property), Times.Once);
        }

        [Test]
        public void SearchProperty_CallsSearchPropertyOnPropertyRepository()
        {
            // Arrange
            var properties = new List<Property> { new Property { Id = 1, Location = "Location1" } };
            _mockPropertyRepository.Setup(repo => repo.searchProperty("Location1")).Returns(properties);

            // Act
            var result = _propertyService.searchProperty("Location1");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Location, Is.EqualTo("Location1"));
            _mockPropertyRepository.Verify(repo => repo.searchProperty("Location1"), Times.Once);
        }
    }
}
