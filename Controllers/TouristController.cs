using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TouristPlacesProject.DAL;
using TouristPlacesProject.Models;

namespace TouristPlacesProject.Controllers
{
    public class TouristController : Controller
    {
        private readonly TouristContext _context;
        public TouristController()
        {
            _context = new TouristContext();
        }
        public ActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Users model)
        {
            if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.UserPassword))
            {
                ViewBag.Message = "Username and Password cannot be empty.";

            }
            Users u = _context.users.ToList().Find(t => t.UserName == model.UserName || t.UserPassword == model.UserPassword);
            if (u == null)
            {
                Users newUser = new Users
                {
                    UserName = model.UserName,
                    UserPassword = model.UserPassword
                }; 
                _context.users.Add(newUser);
                _context.SaveChanges();
            }
            else
            {
                ViewBag.Message = "user already exists with this data try to register with new one";
            }
            return View();
        }


        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(Users model)
        {

            Users u = _context.users.ToList().Find(t => t.UserName == model.UserName && t.UserPassword == model.UserPassword);
            if (u != null)
            {
                return RedirectToAction("return_user");
            }
            else
            {
                ViewBag.Message = "invalid credentials please register ";
            }
            return View();
        }
        public ActionResult return_user(string Name)
        {
            List<TouristPlace> tplist;
            if (!string.IsNullOrEmpty(Name))
            {
                tplist = _context.touristPlaces
                                 .Include("touristPlaceType")
                                 .Where(tp => tp.TouristPlaceName.Contains(Name))
                                 .ToList();
                if (tplist == null)
                {
                    ViewBag.Message = "there is no matching Place";
                }
            }
            else
            {
                tplist = _context.touristPlaces.Include("touristPlaceType").ToList();
            }

            return View(tplist);
        }
        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }
        
        public ActionResult Admin(string AdminName, string AdminPassword)
        {
            string adminname = "karunakar";
            string adminpassword = "kanna@1116";
            if (AdminName == adminname && AdminPassword == adminpassword)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Enter valid credentials";
            }

            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }

        public ActionResult Index(string searchString)
        {
           
            var places = _context.touristPlaces.Include("touristPlaceType").AsQueryable();

           
            if (!string.IsNullOrEmpty(searchString))
            {
                places = places.Where(tp => tp.TouristPlaceName.Contains(searchString));
            }
            List<TouristPlace> tplist = places.ToList();
            return View(tplist);
        }


        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> typelist = new List<SelectListItem>();
            foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
            {
                typelist.Add(
                new SelectListItem()
                {
                    Text = ttype.TouristPlaceTypeName,
                    Value = ttype.TouristPlaceTypeId.ToString()
                });
            

            }
            ViewBag.TouristPlaceTypeId = typelist;
            return View();
        }
        [HttpPost]
        public ActionResult Create(TouristPlace tp)
        {
            tp.Files = Request.Files[0];
            tp.ImagePath = "/Images/" + tp.Files.FileName;
            if (!tp.Files.ContentType.Equals("image/jpeg"))
            {
                ModelState.AddModelError("TpImageFile", "Invalid Image Type");
                List<SelectListItem>typelist=new List<SelectListItem>();
                foreach(TouristPlaceType ttype in _context.touristPlaceTypes)
                {
                    typelist.Add(
                        new SelectListItem()
                        {
                            Text = ttype.TouristPlaceTypeName,
                            Value = ttype.TouristPlaceTypeId.ToString()
                        });

                }
                ViewBag.TouristPlaceTypeId=typelist;
                return View(tp);

            }
            if (tp.Files.ContentLength <= 0 || tp.Files.ContentLength > 1000000)
            {
                ModelState.AddModelError("TpImageFile", "Invalid File Size");
            
                
               List<SelectListItem>typelist=new List<SelectListItem>();
                foreach(TouristPlaceType ttype in _context.touristPlaceTypes)
                {
                    typelist.Add(
                        new SelectListItem()
                        {
                            Text= ttype.TouristPlaceTypeName,
                            Value=ttype.TouristPlaceTypeId.ToString()
                        });
                }
                ViewBag.TouristPlaceTypeId = typelist;
                return View(tp);
            }
            if (ModelState.IsValid)
            {
                tp.Files.SaveAs(Server.MapPath(tp.ImagePath));
                _context.touristPlaces.Add(tp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> typelist = new List<SelectListItem>();
                foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
                {
                    typelist.Add(
                        new SelectListItem()
                        {
                            Text= ttype.TouristPlaceTypeName,
                            Value=ttype.TouristPlaceTypeId.ToString()
                        });
                }
                ViewBag.TouristPlaceTypeId = typelist;
                return View(tp);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            TouristPlace tp=_context.touristPlaces.Include("touristPlaceType").ToList().Find(t=>t.TouristPlaceId== id);
            List<SelectListItem> typelist = new List<SelectListItem>();
            foreach (TouristPlaceType ttype in _context.touristPlaceTypes)
            {
                typelist.Add(
                    new SelectListItem()
                    {
                        Text= ttype.TouristPlaceTypeName,
                        Value=ttype.TouristPlaceTypeId.ToString()
                    });

            }
            ViewBag.TouristPlaceTypeId = typelist;
            return View(tp);
        }

        [HttpPost]
        public ActionResult Edit(TouristPlace tp)
        {
            tp.Files = Request.Files[0];
            if(tp.Files.ContentLength>0 && tp.Files.ContentLength < 100000)
            {
                if (!tp.Files.ContentType.Equals("image/jpeg"))
                {
                    ModelState.AddModelError("TpImageFile", "Invalid Image Type");
                    List<SelectListItem>typelist= new List<SelectListItem>();
                    foreach(TouristPlaceType ttype in _context.touristPlaceTypes)
                    {
                        typelist.Add(
                            new SelectListItem()
                            {
                                Text= ttype.TouristPlaceTypeName,
                                Value=ttype.TouristPlaceTypeId.ToString()
                            });
                    }
                    ViewBag.TouristPlaceTypeId = typelist;
                    return View(tp);
                }
                tp.ImagePath = "/Images/" + tp.Files.FileName;
            }
            else
            {
                tp.Files = null;
            }
            if (ModelState.IsValid)
            {
                TouristPlace etp = _context.touristPlaces.Find(tp.TouristPlaceId);
                etp.TouristPlaceName = tp.TouristPlaceName;
                etp.TouristPlaceDescription = tp.TouristPlaceDescription;
                etp.MoreInfo=tp.MoreInfo;
                if (tp.Files != null)
                {
                    if (!etp.ImagePath.Equals(tp.ImagePath))
                    {
                        etp.ImagePath=tp.ImagePath;
                        tp.Files.SaveAs(Server.MapPath(etp.ImagePath));
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                List<SelectListItem> typelist = new List<SelectListItem>();
                foreach(TouristPlaceType ttype in _context.touristPlaceTypes)
                {
                    typelist.Add(
                        new SelectListItem() { 
                        Text=ttype.TouristPlaceTypeName,
                        Value=ttype.TouristPlaceTypeId.ToString()
                        });
                }
                ViewBag.TouristPlaceTypeId = typelist;
                return View(tp);

            }
        }
        public ActionResult Delete(int id)
        {
            TouristPlace t = _context.touristPlaces.Find(id);
            _context.touristPlaces.Remove(t);
            _context.SaveChanges();
            List<TouristPlace> tlist = _context.touristPlaces.ToList();
            return RedirectToAction("Index", tlist);

        }
        public ActionResult Details(int id)
        {
            TouristPlace t = _context.touristPlaces.Find(id);
            return View(t);
        }
        
    }
}