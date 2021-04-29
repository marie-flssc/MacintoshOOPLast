using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OOP_CA_Macintosh.Helpers;
using OOP_CA_Macintosh.Models;
using OOP_CA_Macintosh.Data;
using OOP_CA_Macintosh.DTO;


namespace OOP_CA_Macintosh.Services
{
    public interface IUserServices
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Delete(int id);
        void Update(User user, string currentPassword, string newPassword, string confirmNewPassword);
    }

    public class UserServices : IUserServices
    {
        private Context _context;

        public UserServices(Context context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = _context.User.FirstOrDefault(x => x.Username == username) ?? null;

            // check if username exists
            if (user == null)
            {
                return null;
            }

            // Granting access if the hashed password in the database matches with the password(hashed in computeHash method) entered by user.
            if (computeHash(password) != user.password)
            {
                return null;
            }
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User;
        }

        public User GetById(int id)
        {
            return _context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new AppException("Password is required");
            }

            if (_context.User.Any(x => x.Username == user.Username))
            {
                throw new AppException("Username \"" + user.Username + "\" is already taken");
            }

            //Saving hashed password into Database table
            user.password = computeHash(password);
            user.AccessLevel = null;
            user.Created = DateTime.UtcNow;
            user.LastModified = DateTime.UtcNow;

            _context.User.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string currentPassword = null, string password = null, string confirmPassword = null)
        {
            //Find the user by Id
            var user = _context.User.Find(userParam.Id);

            if (user == null)
            {
                throw new AppException("User not found");
            }
            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.User.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException("Username " + userParam.Username + " is already taken");
                }
                else
                {
                    user.Username = userParam.Username;
                    user.LastModified = DateTime.UtcNow;
                }
            }
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
            {
                user.FirstName = userParam.FirstName;
                user.LastModified = DateTime.UtcNow;
            }
            if (!string.IsNullOrWhiteSpace(userParam.LastName))
            {
                user.LastName = userParam.LastName;
                user.LastModified = DateTime.UtcNow;
            }
            if (!string.IsNullOrWhiteSpace(currentPassword))
            {
                if (computeHash(currentPassword) != user.password)
                {
                    throw new AppException("Invalid Current password!");
                }

                if (currentPassword == password)
                {
                    throw new AppException("Please choose another password!");
                }

                //Updating hashed password into Database table
                user.password = computeHash(password);
                user.LastModified = DateTime.UtcNow;
            }

            _context.User.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.User.Find(id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        private static string computeHash(string Password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var input = md5.ComputeHash(Encoding.UTF8.GetBytes(Password));
            var hashstring = "";
            foreach (var hashbyte in input)
            {
                hashstring += hashbyte.ToString("x2");
            }
            return hashstring;
        }

        IEnumerable<User> IUserServices.GetAll()
        {
            throw new NotImplementedException();
        }

        User IUserServices.GetById(int id)
        {
            throw new NotImplementedException();
        }

    }
}