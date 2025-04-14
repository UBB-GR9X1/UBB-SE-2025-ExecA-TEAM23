using FluentAssertions;
using Hospital.Managers;
using Hospital.Models;
using Hospital.ViewModels;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PatientDashboardProfilePart.Tests
{
    public class PatientViewModelTests
    {
        private readonly Mock<PatientManagerModel> _managerMock;
        private readonly PatientViewModel _viewModel;

        public PatientViewModelTests()
        {
            _managerMock = new Mock<PatientManagerModel>();
            _viewModel = new PatientViewModel(_managerMock.Object, 1);
        }

        [Fact]
        public async Task UpdateUsername_CallsManagerAndUpdatesState()
        {
            _managerMock.Setup(m => m.UpdateUsername(1, "newUser")).ReturnsAsync(true);

            var result = await _viewModel.UpdateUsername("newUser");

            result.Should().BeTrue();
            _viewModel.Username.Should().Be("newUser");
        }

        [Fact]
        public async Task UpdateWeight_InvalidWeight_ReturnsFalse()
        {
            _managerMock.Setup(m => m.UpdateWeight(1, -1)).ReturnsAsync(false);

            var result = await _viewModel.UpdateWeight(-1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateName_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdateName(1, "John")).ReturnsAsync(true);

            var result = await _viewModel.UpdateName("John");

            result.Should().BeTrue();
            _viewModel.Name.Should().Be("John");
        }

        [Fact]
        public async Task UpdateEmail_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdateEmail(1, "john@mail.com")).ReturnsAsync(true);

            var result = await _viewModel.UpdateEmail("john@mail.com");

            result.Should().BeTrue();
            _viewModel.Email.Should().Be("john@mail.com");
        }

        [Fact]
        public async Task UpdateAddress_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdateAddress(1, "New Street")).ReturnsAsync(true);

            var result = await _viewModel.UpdateAddress("New Street");

            result.Should().BeTrue();
            _viewModel.Address.Should().Be("New Street");
        }

        [Fact]
        public async Task UpdatePassword_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdatePassword(1, "pass123")).ReturnsAsync(true);

            var result = await _viewModel.UpdatePassword("pass123");

            result.Should().BeTrue();
            _viewModel.Password.Should().Be("pass123");
        }

        [Fact]
        public async Task UpdatePhoneNumber_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdatePhoneNumber(1, "0712345678")).ReturnsAsync(true);

            var result = await _viewModel.UpdatePhoneNumber("0712345678");

            result.Should().BeTrue();
            _viewModel.PhoneNumber.Should().Be("0712345678");
        }

        [Fact]
        public async Task UpdateEmergencyContact_UpdatesStateAndCallsManager()
        {
            _managerMock.Setup(m => m.UpdateEmergencyContact(1, "0765432109")).ReturnsAsync(true);

            var result = await _viewModel.UpdateEmergencyContact("0765432109");

            result.Should().BeTrue();
            _viewModel.EmergencyContact.Should().Be("0765432109");
        }

        [Fact]
        public async Task UpdateWeight_ValidWeight_UpdatesState()
        {
            _managerMock.Setup(m => m.UpdateWeight(1, 80)).ReturnsAsync(true);

            var result = await _viewModel.UpdateWeight(80);

            result.Should().BeTrue();
            _viewModel.Weight.Should().Be(80);
        }

        [Fact]
        public async Task UpdateHeight_ValidHeight_UpdatesState()
        {
            _managerMock.Setup(m => m.UpdateHeight(1, 180)).ReturnsAsync(true);

            var result = await _viewModel.UpdateHeight(180);

            result.Should().BeTrue();
            _viewModel.Height.Should().Be(180);
        }

        [Fact]
        public async Task LogUpdate_CallsManager()
        {
            _managerMock.Setup(m => m.LogUpdate(1, ActionType.UPDATE_PROFILE)).ReturnsAsync(true);

            var result = await _viewModel.LogUpdate(1, ActionType.UPDATE_PROFILE);

            result.Should().BeTrue();
        }
    }
}
