using Microsoft.AspNetCore.Identity;

namespace ReactChess.Models
{
    public class User : IdentityUser
    {
        public int Games { get; set; } 
        public int Wins { get; set; } 
        public int Draws { get; set; }
        public int Losses { get; set; }
                
    }
}
