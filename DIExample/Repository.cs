using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIExample
{
    public class Repository<T> : IRepository<T> where T : AbstractItem
    {
        private IList<T> items = new List<T>();

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public void Add(T e)
        {
            items.Add(e);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T GetById(int id)
        {
            return items.FirstOrDefault(x => x.Id == id);
        }

        public void Remove(T e)
        {
            items.Remove(e);
        }

    }
}
