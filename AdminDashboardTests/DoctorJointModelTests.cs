using System;
using Hospital.Models;
using NUnit.Framework;

namespace AdminDashboardTests
{
    [TestFixture]
    public class DoctorJointModelTests
    {
        private DoctorJointModel CreateDoctorJointModel(int departmentId = 1, double rating = 4.5)
        {
            return new DoctorJointModel(
                doctorId: 101,
                userId: 202,
                doctorName: "Dr. Alice Smith",
                departmentId: departmentId,
                rating: rating,
                licenseNumber: "MED123456",
                username: "dralice",
                password: "secure123",
                mail: "alice@example.com",
                birthDate: new DateOnly(1980, 5, 20),
                cnp: "1234567890123",
                address: "123 Health St.",
                phoneNumber: "123-456-7890",
                registrationDate: new DateTime(2020, 1, 15)
            );
        }

        [Test]
        public void GetDoctorName_ReturnsCorrectName()
        {
            var model = CreateDoctorJointModel();
            var result = model.GetDoctorName();
            Assert.AreEqual("Dr. Alice Smith", result);
        }

        [Test]
        public void SetDoctorName_ChangesNameCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.DoctorName = "Dr. Bob Jones";
            Assert.AreEqual("Dr. Bob Jones", model.DoctorName);
        }

        [Test]
        public void GetDoctorRating_ReturnsCorrectRating()
        {
            var model = CreateDoctorJointModel(rating: 3.8);
            var result = model.GetDoctorRating();
            Assert.AreEqual(3.8, result);
        }

        [Test]
        public void SetRating_ChangesRatingCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Rating = 2.9;
            Assert.AreEqual(2.9, model.Rating);
        }

        [Test]
        public void GetBirthDate_ReturnsCorrectDate()
        {
            var model = CreateDoctorJointModel();
            Assert.AreEqual(new DateOnly(1980, 5, 20), model.GetBirthDate());
        }

        [Test]
        public void SetBirthDate_ChangesDateCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.BirthDate = new DateOnly(1990, 12, 1);
            Assert.AreEqual(new DateOnly(1990, 12, 1), model.BirthDate);
        }

        [Test]
        public void GetRegistrationDate_ReturnsCorrectDate()
        {
            var model = CreateDoctorJointModel();
            Assert.AreEqual(new DateTime(2020, 1, 15), model.GetRegistrationDate());
        }

        [Test]
        public void SetRegistrationDate_ChangesDateCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.RegistrationDate = new DateTime(2021, 3, 10);
            Assert.AreEqual(new DateTime(2021, 3, 10), model.RegistrationDate);
        }

        [TestCase(1, "Cardiology")]
        [TestCase(2, "Neurology")]
        [TestCase(3, "Pediatrics")]
        [TestCase(4, "Ophthalmology")]
        [TestCase(5, "Gastroenterology")]
        [TestCase(6, "Orthopedics")]
        [TestCase(7, "Dermatology")]
        [TestCase(999, "Unknown")]
        public void GetDoctorDepartment_ReturnsCorrectDepartment(int deptId, string expected)
        {
            var model = CreateDoctorJointModel(departmentId: deptId);
            var result = model.GetDoctorDepartment();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void SetDepartmentId_UpdatesDepartmentCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.DepartmentId = 5;
            Assert.AreEqual("Gastroenterology", model.GetDoctorDepartment());
        }

        [Test]
        public void ToString_ReturnsExpectedFormat()
        {
            var model = CreateDoctorJointModel(departmentId: 3, rating: 4.6);
            var result = model.ToString();
            Assert.AreEqual("Dr. Alice Smith (Department ID: 3, Rating: 4.6)", result);
        }

        [Test]
        public void SetDoctorId_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.DoctorId = 999;
            Assert.AreEqual(999, model.DoctorId);
        }

        [Test]
        public void SetUserId_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.UserId = 888;
            Assert.AreEqual(888, model.UserId);
        }

        [Test]
        public void SetLicenseNumber_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.LicenseNumber = "NEW456789";
            Assert.AreEqual("NEW456789", model.LicenseNumber);
        }

        [Test]
        public void SetUsername_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Username = "drbob";
            Assert.AreEqual("drbob", model.Username);
        }

        [Test]
        public void SetPassword_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Password = "newpass";
            Assert.AreEqual("newpass", model.Password);
        }

        [Test]
        public void SetMail_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Mail = "newemail@example.com";
            Assert.AreEqual("newemail@example.com", model.Mail);
        }

        [Test]
        public void SetCnp_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Cnp = "9876543210987";
            Assert.AreEqual("9876543210987", model.Cnp);
        }

        [Test]
        public void SetAddress_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.Address = "456 New St.";
            Assert.AreEqual("456 New St.", model.Address);
        }

        [Test]
        public void SetPhoneNumber_UpdatesValueCorrectly()
        {
            var model = CreateDoctorJointModel();
            model.PhoneNumber = "987-654-3210";
            Assert.AreEqual("987-654-3210", model.PhoneNumber);
        }

    }
}
