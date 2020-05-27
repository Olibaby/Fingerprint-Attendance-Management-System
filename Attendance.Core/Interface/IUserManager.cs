using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core.Interface
{
    public interface IUserManager
    {
        bool ValidateUser(LoginModel model);
        void SignUp(UserModel model);
    }
}
