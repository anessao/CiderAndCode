using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CiderAndCode.Web.DataModels;
using CiderAndCode.Web.ViewModels;

namespace CiderAndCode.Web.Controllers
{
    [RoutePrefix("api/Bushels")]
    public class BushelController : ApiController
    {
        [HttpGet, Route("")]
        public HttpResponseMessage GetAllBushels()
        {
            var db = new AppDbContext();

            var bushels = db.Bushels.Select(bushel => new AppleResult
            {
                ContributingUser = bushel.User.Name,
                NumberOfBushels = bushel.Quantity,
                TypeOfApple = bushel.Type.ToString(),
                Id = bushel.Id,
                Pressed = bushel.Pressed
            });

            return Request.CreateResponse(HttpStatusCode.OK, bushels);
        }

        [HttpDelete, Route("{id}")]
        public HttpResponseMessage RemoveApples(int id)
        {
            var db = new AppDbContext();

            db.Bushels.Remove(db.Bushels.Find(id));
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPut, Route("updatebushel")]
        public HttpResponseMessage StealAmountOfBushels(Bushel newBushel)
        {
            var db = new AppDbContext();
            //firstordefault then make changes
            var updatedBushel = db.Bushels.FirstOrDefault<Bushel>(bushel => bushel.Id == newBushel.Id);

            updatedBushel.Quantity = newBushel.Quantity;
            updatedBushel.Pressed = newBushel.Pressed;

            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);

        }


        [HttpGet, Route("user/{userId}")]
        public HttpResponseMessage GetBushelsByUser(int userId)
        {
            var db = new AppDbContext();

            var bushels = db.Bushels.Where(bushel => bushel.User.Id == userId)
                .Select(bushel => new AppleResult
                {
                    ContributingUser = bushel.User.Name,
                    NumberOfBushels = bushel.Quantity,
                    TypeOfApple = bushel.Type.ToString(),
                    Id = bushel.Id
                });

            //var bushelsContributed = db.Bushels.GroupBy(bushel => bushel.User.Id)
            //    .Select(group => new
            //    {
            //        UserId = group.Key,
            //        NumberOfBushels = group.Sum(bushel => bushel.Quantity)
            //    });

            return Request.CreateResponse(HttpStatusCode.OK, bushels);
        }
    }
}