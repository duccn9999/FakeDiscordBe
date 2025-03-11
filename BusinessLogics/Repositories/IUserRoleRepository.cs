﻿using DataAccesses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogics.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UserRole>
    {
        public List<UserRole> GetUserRolesByUserId(int userId);
    }
}
