using BeanSceneWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BeanSceneWebApp.Services
{
    public class PersonService
    {




        private readonly ApplicationDbContext _context;
        public PersonService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Person> FindOrCreateAsync(string email, string firstName, string lastName,string phone)
        {
            email = email.Trim().ToUpper();
            var result = await _context.People.FirstOrDefaultAsync(p => p.Email.Trim().ToUpper() == email);
            if (result != null)
            {
                return result;

            }
            var person = new Person { Email = email, FirstName = firstName.Trim(), LastName = lastName.Trim(), Phone= phone.Trim() };
            _context.People.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }






        public async Task<Person> SetUserIdAsync(int personId,string userId)
        {
          
            var person = await _context.People.FirstOrDefaultAsync(p => p.Id==personId);
            person.UserId = userId;
            

            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> GetPersonByUserName(string username)
        {

            var person = await _context.People.Include(p => p.User).FirstOrDefaultAsync(p => p.User.UserName == username);
  

            return person;
        }








    }
}
