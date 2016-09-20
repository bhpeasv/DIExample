﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DIExample;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{

    [TestClass]
    public class StudentManagerTest
    {
        private static Mock<IRepository<Student>> mock;
        private static IList<Student> students = new List<Student>();

        [ClassInitialize]
        public static void classInitializer(TestContext context)
        {
            mock = new Mock<IRepository<Student>>();
            mock.SetupGet(x => x.Count).Returns(() => students.Count);
            mock.Setup(x => x.Add(It.IsAny<Student>())).Callback<Student>((s) =>
            {
                students.Add(s);
            });
            mock.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) => students.FirstOrDefault(x => x.Id == id));
        }
        [TestInitialize]
        public void testInitializer()
        {
            students = new List<Student>();
        }

        [TestMethod]
        public void AddStudentTest()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);
            Student student = new Student(1, "Name", "Email");

            sm.AddStudent(student);

            Assert.AreEqual(1, sm.Count);
            Assert.AreSame(student, sm.GetById(student.Id));
        }
    }
}
