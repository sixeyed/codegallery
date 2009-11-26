using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data;
using System.Collections;
using FluentDAL.Sample.Entities;
using FluentDAL.Sample.Maps;
using FluentDAL.Sample.StoredProcedures;

namespace FluentDAL.Sample.Repositories
{
    public class UserRepository 
    {
    public User GetUser(Guid userId)
    {
        //populate basic details:
        User user = Fluently.Load<User>().With<UserMap>()
                            .From<GetUser>(i => i.UserId = userId,
                                           x => x.Execute());
        //add accounts:
        user.Accounts = Fluently.Load<List<Account>>().With<AccountMap>()
                                .From<GetUserAccounts>(i => i.UserId = userId,
                                                       x => x.Execute());

        return user;
    }
    }
}
