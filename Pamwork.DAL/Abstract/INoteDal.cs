using Pamwork.Core.DataAccess;
using Pamwork.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.DAL.Abstract
{
    public interface INoteDal:IEntityRepository<Note>
    {
    }
}
