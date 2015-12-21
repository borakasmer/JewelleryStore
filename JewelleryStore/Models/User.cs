using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JewelleryStore.Models
{
    public enum LogType
    {
        SignIn = 1,
        LogIn = 2,
        WaitForConfirm=3,
        LogOut=4,
        ProductDelete=5,
        ProductInsert = 6,
        ProductUpdate = 7,
        Admin = 8,
        Log=9,
        Home=10
    }
    public enum LoginState
    {
        NoRecord=1,
        Recorded=2,
        WaitForApprove=3,
        WaitForApproveWrongPassword = 4,
        WrongPassword = 5
    }
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [NotMapped]
        public bool IsRemmber { get; set; }
    }    
}