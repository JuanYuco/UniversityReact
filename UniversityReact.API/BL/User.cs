using System;
using System.Threading.Tasks;
using UniversityReact.API.Data;
using System.Linq;
using System.Data.Entity;

namespace UniversityReact.API.BL
{
    public class User
    {
        private UniversityReactEntities db = new UniversityReactEntities();

        public async Task<Models.User> CreateUser ( Models.User user )
        {
            try
            {
                db.Users.Add(new Data.Users
                {
                    id = user.id,
                    name = user.name,
                    lastName = user.lastName,
                    email = user.email,
                    password = user.password
                });

                await db.SaveChangesAsync();
                return user;
            } catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public bool FindByEmail ( string email )
        {
            try
            {
                var user = db.Users.FirstOrDefault(x => x.email == email);
                return user == null;
            } catch ( Exception e )
            {
                Console.WriteLine(e);
                return true;
            }
        }

        public async Task<Models.User> getUserByEmailAndPassword ( string email, string password )
        {
            try
            {
                Models.User userResult = null;
                var user = await db.Users.FirstOrDefaultAsync( x => x.email == email && x.password == password );
                if ( user != null )
                {
                    userResult = new Models.User
                    {
                        id = user.id,
                        name = user.name,
                        lastName = user.lastName,
                        email = user.email
                    };
                }

                return userResult;

            } catch( Exception e )
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}