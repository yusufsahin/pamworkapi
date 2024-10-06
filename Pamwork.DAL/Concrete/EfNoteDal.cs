using Pamwork.Core.DataAccess.EntityFramework;
using Pamwork.DAL.Abstract;
using Pamwork.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamwork.DAL.Concrete
{
   
        public class EfNoteDal : EfEntityRepositoryBase<Note>, INoteDal
        {
            private PamworkDbContext _pamworkDbContext;
            public EfNoteDal(PamworkDbContext pamworkDbContext) : base(pamworkDbContext)
            {
                _pamworkDbContext = pamworkDbContext;
            }
        }

    }

