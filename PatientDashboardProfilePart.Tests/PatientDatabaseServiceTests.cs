﻿using FluentAssertions;
using Hospital.Models;
using System;
using System.Threading.Tasks;
using Hospital.Repositories;
using Xunit;

namespace PatientDashboardProfilePart.Tests
{
    public class PatientRepositoryTests
    {
        private readonly PatientRepository _patientRepository;

        public PatientRepositoryTests()
        {
            _patientRepository = new PatientRepository();
        }

        [Fact]
        public async Task GetPatientByUserId_WithInvalidId_ReturnsDefaultPatient()
        {
            var result = await _patientRepository.GetPatientByUserId(-999);

            result.Should().NotBeNull();
            result.UserId.Should().Be(0);
        }

        [Fact]
        public async Task UpdateEmail_WithNonexistentUserId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateEmail(-999, "test@example.com");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task LogUpdate_WithInvalidType_Throws()
        {
            Func<Task> act = async () => await _patientRepository.LogUpdate(-1, (ActionType)999);

            await act.Should().ThrowAsync<Exception>().WithMessage("*Invalid type for Authentication Log*");
        }

        [Fact]
        public async Task UpdatePassword_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdatePassword(-1, "newpass123");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateUsername_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateUsername(-1, "username");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateName_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateName(-1, "Test Name");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateBirthDate_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateBirthDate(-1, DateOnly.FromDateTime(DateTime.Today));

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAddress_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateAddress(-1, "Test Address");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdatePhoneNumber_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdatePhoneNumber(-1, "0712345678");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateEmergencyContact_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateEmergencyContact(-1, "0712345678");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateWeight_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateWeight(-1, 75.5);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateHeight_WithInvalidId_ReturnsFalse()
        {
            var result = await _patientRepository.UpdateHeight(-1, 180);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task LogUpdate_WithValidUpdateProfile_ReturnsTrue()
        {
            var result = await _patientRepository.LogUpdate(1, ActionType.UPDATE_PROFILE);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task LogUpdate_WithValidChangePassword_ReturnsTrue()
        {
            var result = await _patientRepository.LogUpdate(1, ActionType.CHANGE_PASSWORD);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllPatients_DoesNotThrow()
        {
            var result = await _patientRepository.GetAllPatients();

            result.Should().NotBeNull();
        }
    }
}
