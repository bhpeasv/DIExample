using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIExample
{
    public interface IRepository<T> where T : AbstractItem
    {
        int Count { get; }
        void Add(T e);
        void Remove(T e);
        List<T> GetAll();
        T GetById(int id);
    }
}
