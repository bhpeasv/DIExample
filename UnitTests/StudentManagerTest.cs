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
            // redirecting the used repository to the internal students list of the test class (a fake repository)
            mock = new Mock<IRepository<Student>>();
            mock.SetupGet(x => x.Count).Returns(() => students.Count);
            mock.Setup(x => x.Add(It.IsAny<Student>())).Callback<Student>((s) => students.Add(s));
            mock.Setup(x => x.GetById(It.IsAny<int>())).Returns((int id) => students.FirstOrDefault(x => x.Id == id));
            mock.Setup(x => x.GetAll()).Returns(() => students.ToList());
            mock.Setup(x => x.Remove(It.IsAny<Student>())).Callback<Student>((s) => students.Remove(s));
        }

        /// <summary>
        /// Test initialize method is executed before execution of EACH test
        /// </summary>
        [TestInitialize]
        public void testInitializer()
        {
            students.Clear();
        }

        /// <summary>
        /// Test method for adding an exisiting repository object to the StudentManager at creation.
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
        /// Test method for adding a NULL reference as repository to the StudentManager at creation.
        /// Should throw an ArgumentNullException.
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
        /// Test method for adding a new student to the repository.
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
        /// Test method for trying to add a student with an ID matching a student already contained in the repository.
        /// Should throw an ArgumentException.
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
        /// Test method for retrieving an existing student by Id.
        /// The found student object is returned.
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
        /// Test method for retrieving a non-existing student.
        /// Should return null.
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
        /// Test method for removing an existing student.
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
        /// Test method for trying to remove a non-existing student.
        /// Should throw an ArgumentException.
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
