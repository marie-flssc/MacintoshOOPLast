using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OOP_CA_Macintosh.Models
{
    public class Fee
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int AmountToPay { get; set; }
        public int PayedAmount { get; set; }

        //Maybe add a reference to the student id here or in the controller
        public int Pay(int amount)
        {
            if (AmountToPay < PayedAmount + amount)
            {
                //TODO: Change this line so that it's visible on the site
                Console.WriteLine("The amount you payed exceeds the money needed, here is your change :");
                return PayedAmount + amount - AmountToPay;
            }
            PayedAmount += amount;
            return 0;
        }
    }
}
