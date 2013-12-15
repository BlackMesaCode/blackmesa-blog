using System.Collections.Generic;
using System.Web.Http;

namespace BlackMesa.Website.Main.Areas.Learning.Api.Controllers
{
    public class FoldersController : ApiController
    {
        // GET api/folder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/folder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/folder
        public void Post([FromBody]string value)
        {
        }

        // PUT api/folder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/folder/5
        public void Delete(int id)
        {
        }
    }
}
