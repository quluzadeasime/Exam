using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstracts
{
    public interface IChefService
    {
        void Add(Chef chef);
        void Delete(int id);
        void Update(int id,Chef chef);
        Chef Get(Func<Chef, bool> func = null);
        List<Chef> GetAll(Func<Chef, bool> func = null);

    }
}
