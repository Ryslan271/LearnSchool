using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSchool
{
    static class DBConnect
    {
        static public ServiceEntities db { get; set; }

        static DBConnect()
        {
            db = new ServiceEntities();

            db.Client.Load();
            db.ClientService.Load();
            db.Service.Load();
            db.ServicePhoto.Load();
            db.Role.Load();
            db.User.Load();
        }
    }
}
