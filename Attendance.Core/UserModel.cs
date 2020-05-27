using Attendance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Core
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
       

        public UserModel()
        {

        }

        public UserModel(User user)
        {
            if (user == null) return;
            UserId = user.UserId;
            UserName = user.UserName;
            PassWord = user.Password;
        }

        public User Create(UserModel model)
        {
            return new User
            {
                UserName = model.UserName,
                Password = model.PassWord,
            };
        }

        public User Edit(User entity, UserModel model)
        {
            entity.UserId = model.UserId;
            entity.UserName = model.UserName;
            entity.Password = model.PassWord;
            return entity;
        }
    }
}
