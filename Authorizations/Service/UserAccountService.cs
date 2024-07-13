using Authorizations.Data;
using Authorizations.Model;
using Microsoft.EntityFrameworkCore;

namespace Authorizations.Service;

public  class UserAccountService
{
    private  UserDbContext _dbContext;
    public UserAccountService()
    {
        _dbContext = new UserDbContext();
    }
    public List<UserAccount> GetUserAccount()
    {
        var acc = _dbContext.UserAccounts.ToList();
        return acc;
    }
    public void Create(string userName,string password)
    {
        var userAccount = new UserAccount()
        {
            UserName = userName,
            Password = password
        };
        _dbContext.UserAccounts.Add(userAccount);
        _dbContext.SaveChanges(); 
    }
}
