using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Library.Areas.Database.Controllers
{
	[Area("Db")]
	public class HomeController : Controller
	{
		private readonly LibraryContext _db;
		public HomeController(LibraryContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			try
			{
				var books = _db.Books.ToList();
				_db.Books.RemoveRange(books);

				var categories = _db.Categories.ToList();
				_db.Categories.RemoveRange(categories);

				var writers = _db.Writers.ToList();
				_db.Writers.RemoveRange(writers);

                var users = _db.Users.ToList();
                _db.Users.RemoveRange(users);

                var roles = _db.Roles.ToList();
                _db.Roles.RemoveRange(roles);
				if (roles.Count > 0)
				{
					_db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)");
				}
                var cities = _db.Cities.ToList();
                _db.Cities.RemoveRange(cities);

                var countries = _db.Countries.ToList();
                _db.Countries.RemoveRange(countries);
                _db.Writers.Add(new Writer()
				{

					Name = "Stephen",
					Surname = "King",
				});

				_db.Categories.Add(new Category()
				{
					
					Name = "Horror",
					Description = "Horror fiction is a genre of horror literature and horror fantasy literature that aims to give its readers a sense of fear and terror.",

					Books = new List<Book>()
				    {
						new Book()
						{
							
							Name = "It",
							Description = "A killer clown who lives in the sewers terrorizes a small town in Maine in the 1950s. Thirty years later, a group of friends who confronted the clown as children return home to face the evil again, this time as adults. It is about confronting childhood trauma as a grown-up, and the fear the now-adults feel radiates from the page.",
							
						},
						new Book()
						{

							Name = "Pet Sematary",
							Description = "Alongside a busy stretch of Maine highway there is a cemetery for the animals the road has claimed; deeper in the adjacent forest lies a Native American burial ground infused with powerful and terrifying magic. A doctor and his young family move into a house nearby, and a father’s attempt to reconcile with death leads to a series of horrifying events. According to King himself, Pet Sematary is his most disturbing novel."
						},
						new Book()
						{

							Name = "The Shining",
							Description = "Nestled remotely in the Colorado Rockies, the Overlook Hotel is inaccessible from October through April due to impassible roads and extreme weather. Jack Torrence, a recovering alcoholic writer with anger issues, his wife, Wendy, and their son, Danny, a boy with psychic powers, move in to care for the majestic hotel during the off-season. As Jack descends into madness, his wife and son struggle to survive.",
						}
					}
				});
                _db.Countries.Add(new Country()
                {
                    Name = "United States",
                    Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "Los Angeles"
                    },
                    new City()
                    {
                        Name = "New York"
                    }
                }
                });
                _db.Countries.Add(new Country()
                {
                    Name = "Turkey",
                    Cities = new List<City>()
                {
                    new City()
                    {
                        Name = "Ankara"
                    },
                    new City()
                    {
                        Name = "Istanbul"
                    },
                    new City()
                    {
                        Name = "Izmir"
                    }
                }
                });

                _db.SaveChanges();

                _db.Roles.Add(new Role()
                {
                    Name = "Admin",
                    Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "ersen",
                        UserName = "ersen",
                        UserDetail = new UserDetail()
                        {
                            Address = "Cankaya",
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Ankara").Id,
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "Turkey").Id,
                            Email = "ersen@library.com",
                            Sex = Sex.Man
                        }
                    }
                }
                });
                _db.Roles.Add(new Role()
                {
                    Name = "User",
                    Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "leo",
                        UserName = "leo",
                        UserDetail = new UserDetail()
                        {
                            Address = "Hollywood",
                            CityId = _db.Cities.SingleOrDefault(c => c.Name == "Los Angeles").Id,
                            CountryId = _db.Countries.SingleOrDefault(c => c.Name == "United States").Id,
                            Email = "leo@library.com",
                            Sex = Sex.Man
                        }
                    }
                }
                });
                _db.SaveChanges();
				return Content("<label style=\"color:darkgreen;\"><b>Database seed successful.<b></label>", "text/html", Encoding.UTF8);

			}
			catch (Exception exc)
			{
				string message = exc.Message;
				throw exc;
			}


		}
	}
}
