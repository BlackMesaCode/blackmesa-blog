using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Website.Main.Models.Identity
{
    [Table("Identity_Users")]
    public class User : IdentityUser
    {

    }

}