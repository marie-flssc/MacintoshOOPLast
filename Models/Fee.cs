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
        public string Name { get; set; }
       
    }
}
