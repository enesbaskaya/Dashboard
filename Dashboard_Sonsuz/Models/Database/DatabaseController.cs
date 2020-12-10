using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Models.Database
{
    interface DatabaseController
    {
        Task<int> Update();
        Task<int> Insert();
    }
}
