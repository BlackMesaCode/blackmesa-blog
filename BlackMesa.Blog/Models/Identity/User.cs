using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BlackMesa.Blog.Models.Identity
{
    [Table("Identity_Users")]
    public class User : IdentityUser
    {

    }

}