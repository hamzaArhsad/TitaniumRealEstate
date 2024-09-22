using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Contact
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        public string Email { get; set; }
      
        public String Message { get; set; } 
    }
}
