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

        // Internal list of students replacing the repository (fake repository)
        private static IList<Student> students = new List<Student>(); 

        [ClassInitialize]
        public static void classInitializer(TestContext context)
        {
            mock = new Mock<IRepository<Student>>();
            mock.SetupGet(x => x.Count).Returns(() => students.Count);
            mock.Setup(x => x.Add(It.IsAny<Student>())).Callback<Student>((s) => students.Add(s));
            mock.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) => students.FirstOrDefault(x => x.Id == id));
            mock.Setup(x => x.GetAll()).Returns(() => students.ToList());
            mock.Setup(x => x.Remove(It.IsAny<Student>())).Callback<Student>((s) => students.Remove(s));
        }
        /// <summary>
        /// Executed before each test method is executed.
        /// Ensures each test is executed on an empty repository.
        /// </summary>
        [TestInitialize]
        public void testInitializer()
        {
            students.Clear();
        }

        /// <summary>
        /// Test method testing the creation of a StudentManager with an existing repository.
        /// </summary>
        [TestMethod]
        public void Create_StudentManager_Existing_Repository_Test()
        {
            IRepository<Student> repository = mock.Object;

            StudentManager sm = new StudentManager(repository);

            Assert.IsNotNull(sm);
            Assert.AreEqual(0, sm.Count);
        }

        /// <summary>
        /// Test method testing creation of a StudentManager with no repository (null).
        /// Expects ArgumentNullException to be thrown.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_StudentManager_No_Repository_Expect_ArgumentNullException_Test()
        {
            IRepository<Student> repository = null;

            StudentManager sm = new StudentManager(repository);

            Assert.Fail("Created StudentManager with NULL repository");
        }

        /// <summary>
        /// Test method testing adding a new student to the repository.
        /// </summary>
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

        /// <summary>
        /// Test method adding an existing student to the repository.
        /// Expects an ArgumentException to be thrown.
        /// </summary>
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

        /// <summary>
        /// Test method testing the retrieval of all students from the repository.
        /// </summary>
        [TestMethod]
        public void GetAllStudents_Test()
        {
            IRepository<Student> repository = mock.Object; 
            StudentManager sm = new StudentManager(repository);
            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");

            sm.AddStudent(student1);
            sm.AddStudent(student2);

            IList<Student> result = sm.GetAllStudents();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(student1, result[0]);
            Assert.AreEqual(student2, result[1]);
        }

        /// <summary>
        /// Test method testing retrieval of an existing student with a specific Id.
        /// </summary>
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

        /// <summary>
        /// Test method testing the retrieval on a non-existing student.
        /// Expects an ArgumentException to be thrown.
        /// </summary>
        [TestMethod]
        public void GetStudentById_NonExisting_Student_Returns_NULL_Test()
        {
            IRepository<Student> repository = mock.Object;
            StudentManager sm = new StudentManager(repository);
            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");
            sm.AddStudent(student1);

            Student result = sm.GetStudentById(2);

            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// Test method testing removal of an existing student.
        /// </summary>
        [TestMethod]
        public void RemoveStudent_Existing_Student_Test()
        {
            IRepository<Student> repository = mock.Object;
            StudentManager sm = new StudentManager(repository);
            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");
            sm.AddStudent(student2);
            sm.AddStudent(student1);

            sm.RemoveStudent(student2);

            Assert.AreEqual(1, sm.Count);
            Assert.AreEqual(student1, sm.GetAllStudents()[0]);
        }

        /// <summary>
        /// Test method testing removal of a non-existing student.
        /// Expects an ArgumentException to be thrown.
        /// </summary>
        [TestMethod]
        public void RemoveStudent_NonExisting_Student_Expect_ArgumentException_Test()
        {
            IRepository<Student> repository = mock.Object;
            StudentManager sm = new StudentManager(repository);
            Student student1 = new Student(1, "Name", "Email");
            Student student2 = new Student(2, "Name", "Email");
            sm.AddStudent(student1);

            try
            {
                sm.RemoveStudent(student2);
                Assert.Fail("Removed non-existing student");
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(1, sm.Count);
                Assert.AreEqual(student1, sm.GetAllStudents()[0]);
            }
        }
    }
}
