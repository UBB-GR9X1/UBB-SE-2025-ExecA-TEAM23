using FluentAssertions;
using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PatientDashboardProfilePart.Tests
{
    public class PatientViewModelTests
    {
        private readonly Mock<PatientService> _mockPatientService;
        private readonly PatientViewModel _patientViewModel;

        public PatientViewModelTests()
        {
            _mockPatientService = new Mock<PatientService>();
            _patientViewModel = new PatientViewModel(_mockPatientService.Object, 1);
        }

        [Fact]
        public async Task UpdateUsername_CallsManagerAndUpdatesState()
        {
            _mockPatientService.Setup(m => m.UpdateUsername(1, "newUser")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateUsername("newUser");

            result.Should().BeTrue();
            _patientViewModel.Username.Should().Be("newUser");
        }

        [Fact]
        public async Task UpdateWeight_InvalidWeight_ReturnsFalse()
        {
            _mockPatientService.Setup(m => m.UpdateWeight(1, -1)).ReturnsAsync(false);

            var result = await _patientViewModel.UpdateWeight(-1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateName_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdateName(1, "John")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateName("John");

            result.Should().BeTrue();
            _patientViewModel.Name.Should().Be("John");
        }

        [Fact]
        public async Task UpdateEmail_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdateEmail(1, "john@mail.com")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateEmail("john@mail.com");

            result.Should().BeTrue();
            _patientViewModel.Email.Should().Be("john@mail.com");
        }

        [Fact]
        public async Task UpdateAddress_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdateAddress(1, "New Street")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateAddress("New Street");

            result.Should().BeTrue();
            _patientViewModel.Address.Should().Be("New Street");
        }

        [Fact]
        public async Task UpdatePassword_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdatePassword(1, "pass123")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdatePassword("pass123");

            result.Should().BeTrue();
            _patientViewModel.Password.Should().Be("pass123");
        }

        [Fact]
        public async Task UpdatePhoneNumber_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdatePhoneNumber(1, "0712345678")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdatePhoneNumber("0712345678");

            result.Should().BeTrue();
            _patientViewModel.PhoneNumber.Should().Be("0712345678");
        }

        [Fact]
        public async Task UpdateEmergencyContact_UpdatesStateAndCallsManager()
        {
            _mockPatientService.Setup(m => m.UpdateEmergencyContact(1, "0765432109")).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateEmergencyContact("0765432109");

            result.Should().BeTrue();
            _patientViewModel.EmergencyContact.Should().Be("0765432109");
        }

        [Fact]
        public async Task UpdateWeight_ValidWeight_UpdatesState()
        {
            _mockPatientService.Setup(m => m.UpdateWeight(1, 80)).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateWeight(80);

            result.Should().BeTrue();
            _patientViewModel.Weight.Should().Be(80);
        }

        [Fact]
        public async Task UpdateHeight_ValidHeight_UpdatesState()
        {
            _mockPatientService.Setup(m => m.UpdateHeight(1, 180)).ReturnsAsync(true);

            var result = await _patientViewModel.UpdateHeight(180);

            result.Should().BeTrue();
            _patientViewModel.Height.Should().Be(180);
        }

        [Fact]
        public async Task LogUpdate_CallsManager()
        {
            _mockPatientService.Setup(m => m.LogUpdate(1, ActionType.UPDATE_PROFILE)).ReturnsAsync(true);

            var result = await _patientViewModel.LogUpdate(1, ActionType.UPDATE_PROFILE);

            result.Should().BeTrue();
        }
    }
}
