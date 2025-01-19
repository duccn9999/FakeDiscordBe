using BusinessLogics.RepositoriesImpl;
using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
    }
}
