using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Identity.Model
{
    [Table("Identity_Users")]
    public class User : IdentityUser
    {

    }

}