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
                if (students.Contains(s))
                {
                    throw new ArgumentException("Student already exist");
                }
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
            students.Clear();
        }

        [TestMethod]
        public void AddStudent_New_Student_Test()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);
            Student student = new Student(1, "Name", "Email");

            sm.AddStudent(student);

            Assert.AreEqual(1, sm.Count);
            Assert.AreEqual(student, sm.GetStudentById(student.Id));
        }

        [TestMethod]
        public void AddStudent_Existing_Student_Expect_ArgumentException_Test()
        {
            IRepository<Student> repository = mock.Object; //new Repository<Student>();
            StudentManager sm = new StudentManager(repository);
            Student student = new Student(1, "Name", "Email");

            sm.AddStudent(student);

            try
            {
                sm.AddStudent(student); // try to add the same student again
                Assert.Fail("Added existing student to repository");
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(1, sm.Count);
                Assert.AreEqual(student, sm.GetStudentById(student.Id));
            }
            }

        [TestMethod]
        public void GetStudentById_Existing_Student_Test()
        {
            IRepository<Student> repository = mock.Object;
            StudentManager sm = new StudentManager(repository);
            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");
            sm.AddStudent(student2);
            sm.AddStudent(student1);

            Student result = sm.GetStudentById(2);

            Assert.AreEqual(student2, result);
        }
    }
}
