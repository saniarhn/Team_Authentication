using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_MCC59
{
    class UserData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }

        public UserData(string firstName, string lastName, string password, string id)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Id = id;
        }
    }
}
