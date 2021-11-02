using CarShowroom.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarShowroom.Controllers
{
    public class CarsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [NonAction]
        public List<Car> Sort(int Id)
        {
            List<Car> cars;
            switch (Id)
            {
                case (1):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).OrderBy(c => c.Acceleration).ToList();
                    break;
                case (2):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).OrderBy(c => c.Model).ToList();
                    break;
                case (3):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).OrderBy(c => c.Price).ToList();
                    break;
                case (4):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).OrderByDescending(c => c.Price).ToList();
                    break;
                default:
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).ToList();
                    break;
            }
            return cars;
        }

        [NonAction]
        public List<Car> FilterByPrice(int price)
        {
            List<Car> cars;
            switch (price)
            {
                case (1):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Price > 0 && c.Price <= 400000).ToList();
                    break;
                case (2):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Price > 400000 && c.Price <= 550000).ToList();
                    break;
                case (3):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Price > 550000 && c.Price <= 700000).ToList();
                    break;
                case (4):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Price > 700000).ToList();
                    break;
                default:
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).ToList();
                    break;
            }
            return cars;
        }

        [NonAction]
        public List<Car> FilterByColor(Color color)
        {
            List<Car> cars;
            switch (color)
            {
                case (Color.Red):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.Red).ToList();
                    break;
                case (Color.Green):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.Green).ToList();
                    break;
                case (Color.Blue):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.Blue).ToList();
                    break;
                case (Color.Yellow):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.Yellow).ToList();
                    break;
                case (Color.Black):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.Black).ToList();
                    break;
                case (Color.White):
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.color == Color.White).ToList();
                    break;
                default:
                    cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).ToList();
                    break;
            }
            return cars;
        }

        [NonAction]
        public List<Car> FilterByName(string name)
        {
            List<Car> cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Name.Contains(name)).ToList();
            return cars;
        }

        // GET: Cars
        public ActionResult Index()
        {
            ViewBag.Categories = context.Categories.ToList();
            List<Car> cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).ToList();
            return View(cars);
        }

        public ActionResult InCategory(int Id)
        {
            List<Car> cars = context.Cars.Include(c => c.Category).Include(c => c.CarImages).Where(c => c.Category_ID == Id).ToList();
            ViewBag.Categories = context.Categories.ToList();
            return View("Index", cars);
        }

        public ActionResult SortCars(int Id)
        {
            ViewBag.Categories = context.Categories.ToList();


            return View("Index", Sort(Id));
        }

        public ActionResult CarsWithPrice(int Id)
        {
            ViewBag.Categories = context.Categories.ToList();
            return View("Index", FilterByPrice(Id));
        }

        public ActionResult CarsWithColor(Color color)
        {
            ViewBag.Categories = context.Categories.ToList();
            return View("Index", FilterByColor(color));
        }

        public ActionResult CarSearch(string name)
        {
            ViewBag.Categories = context.Categories.ToList();
            return View("Index", FilterByName(name));
        }

        public ActionResult Details(int Id)
        {
            ViewBag.RelatedCars = context.Cars.Where(c => c.Category_ID == Id).ToList();
            ViewBag.Reviews = context.Comments.Where(c => c.Car_ID == Id).ToList();
            Car car = context.Cars.Include(c => c.Category).Include(c => c.CarImages).FirstOrDefault(c => c.Car_ID == Id);
            return View(car);
        }

        [Authorize]
        public ActionResult AddToFav(int Id)
        {
            List<WishList> wishLists = context.WishLists.ToList();
            foreach (var item in wishLists)
            {
                if (item.Car_ID == Id)
                {
                    return RedirectToAction("Index", "Cars");
                }
            }
            Car car = context.Cars.Include(c => c.Category).Include(c => c.CarImages).FirstOrDefault(c => c.Car_ID == Id);
            ApplicationUser user = context.Users.FirstOrDefault(c => c.Email == User.Identity.Name);
            WishList wish = new WishList { Car_ID = car.Car_ID, Customer_ID = user.Id };
            context.WishLists.Add(wish);
            context.SaveChanges();
            return RedirectToAction("Index", "Cars");
        }

        [Authorize]
        public ActionResult ShowFav()
        {
            ApplicationUser user = context.Users.FirstOrDefault(c => c.Email == User.Identity.Name);
            List<WishList> wishLists = context.WishLists.Include(c => c.Car).ThenInclude(c => c.CarImages).Where(c => c.Customer_ID == user.Id).ToList();
            return View(wishLists);
        }

        [Authorize]
        public ActionResult RemoveFav(int Id)
        {
            WishList wishList = context.WishLists.FirstOrDefault(w => w.ID == Id);
            context.WishLists.Remove(wishList);
            context.SaveChanges();
            return RedirectToAction("ShowFav", "Cars");
        }

        [Authorize]
        public ActionResult AddReview(Comment com, int Id)
        {
            ApplicationUser user = context.Users.FirstOrDefault(c => c.Email == User.Identity.Name);
            Car car = context.Cars.Include(c => c.Category).Include(c => c.CarImages).FirstOrDefault(c => c.Car_ID == Id);
            Comment comment = new Comment { Car_ID = car.Car_ID, Customer_ID = user.Id, Text = com.Text, Comment_Date = DateTime.Now };
            context.Comments.Add(comment);
            context.SaveChanges();
            return RedirectToAction("Details", new { Id = Id });
        }
    }
}