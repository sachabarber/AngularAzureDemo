using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularAzureDemo.Models
{
    public class Users : List<User>
    {
        public Users()
        {
            this.Add(new User{Id=1, Name="Sacha Barber"});
            this.Add(new User{Id=2, Name="Adam Gril"});
            this.Add(new User{Id=3, Name="James Franklin"});
            this.Add(new User{Id=4, Name="Vicky Merry" });
            this.Add(new User{Id=5, Name="Cena Rego"});
        }
    }
}