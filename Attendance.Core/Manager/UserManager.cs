using Attendance.Core.Interface;
using Attendance.Data;
using Attendance.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Manager
{
    public class UserManager : IUserManager
    {
        private IGenericRepository _repo;

        public UserManager(IGenericRepository repo)
        {
            _repo = repo;
        }

        public bool ValidateUser(LoginModel model)
        {
            var user = _repo.Get<User>().Where(u => u.UserName.ToLower() == model.UserName.ToLower() && u.Password.ToLower() == model.Password.ToLower()).FirstOrDefault();
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public void SignUp(UserModel model)
        {
            var createuser = _repo.Get<User>().Where(c => c.UserName == model.UserName).FirstOrDefault();
            if (createuser != null) throw new Exception("User already exist");
            var entity = model.Create(model);
            _repo.Add<User>(entity);
        }
    }
}
