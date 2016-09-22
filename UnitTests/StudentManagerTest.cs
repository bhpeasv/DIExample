using System;
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
            mock.Setup(x => x.GetAll()).Returns(() => students.ToList());
            mock.Setup(x => x.Remove(It.IsAny<Student>())).Callback<Student>((s) =>
            {
                students.Remove(s);
            });
        }
        [TestInitialize]
        public void testInitializer()
        {
            students = new List<Student>();
        }

        [TestMethod]
        public void Add_New_Student_Test()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);
            Student student = new Student(1, "Name", "Email");

            sm.AddStudent(student);

            Assert.AreEqual(1, sm.Count);
            Assert.AreSame(student, sm.GetById(student.Id));
        }

        [TestMethod]
        public void GetAll_Test()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);

            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");

            sm.AddStudent(student1);
            sm.AddStudent(student2);

            IList<Student> result = sm.GetAll();

            Assert.AreEqual(2, result.Count);
            Assert.AreSame(student1, result[0]);
            Assert.AreSame(student2, result[1]);
        }

        [TestMethod]
        public void Remove_Existing_Student_Test()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);

            Student student = new Student(1, "Name", "Email");
            sm.Remove(student);

            Assert.AreEqual(0, sm.Count);
        }
    }
}
