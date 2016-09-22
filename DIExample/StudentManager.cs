﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIExample
{
    public class StudentManager
    {
        private IRepository<Student> sr;

        public int Count
        {
            get
            {
                return sr.Count;
            }
        }

        public StudentManager(IRepository<Student> repo)
        {
            if (repo == null)
            {
                throw new ArgumentNullException("Repository is missing");
            }
            sr = repo;
        }

        public void AddStudent(Student aStudent)
        {            
            sr.Add(aStudent);
        }

        public Student GetStudentById(int id)
        {
            return sr.GetById(id);
        }

        public IList<Student> GetAllStudents()
        {
            return sr.GetAll();
        }

        public void RemoveStudent(Student student)
        {
            sr.Remove(student);
        }
    }
}
