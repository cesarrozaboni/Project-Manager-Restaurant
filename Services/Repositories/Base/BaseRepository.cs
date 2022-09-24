using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Repositories.Base
{
    public class BaseRepository<T> where T : class, new()
    {
        private readonly T objDal;

        public BaseRepository()
        {
            objDal = new T();
        }

        public T ObjDal { get => objDal; }
    }

   
}
