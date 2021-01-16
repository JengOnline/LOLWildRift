using LOLWildRift.Web.Models;
using LOLWildRift.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Web.Controllers
{
    public class ChampionsController : Controller
    {
        private readonly LOLWildRiftService _services;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string sessionKeyLogin = "_authen";
        public ChampionsController(LOLWildRiftService services, IWebHostEnvironment webHostEnvironment)
        {
            _services = services;
            _webHostEnvironment = webHostEnvironment;
        }

        // List: ChampionsController
        public async Task<ActionResult> Index(string searchString)
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                ViewData[sessionKeyLogin] = string.IsNullOrEmpty(HttpContext.Session.GetString(sessionKeyLogin)) 
                    ? string.Empty 
                    : HttpContext.Session.GetString(sessionKeyLogin);

                champions = await _services.ChampionList();
                if (!champions.Error)
                {
                    foreach (var data in champions.Champions)
                    {
                        if (data.HISTORY != null && data.HISTORY.Length > 10)
                        {
                            data.HISTORY = data.HISTORY.Substring(0, 50) + "...";
                        }
                    }

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        @ViewData["CurrentFilter"] = searchString;
                        var resultObj = champions.Champions.Where(c => c.NAME.ToUpper().Contains(searchString.ToUpper())
                        || c.LANE.ToUpper().Contains(searchString.ToUpper())
                        || c.ROLE.ToUpper().Contains(searchString.ToUpper()));
                        return View(resultObj.ToList());
                    }
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage");
            }
            return View(champions.Champions);
        }

        // GET: ChampionsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ChampionsEntity champions = new ChampionsEntity();
            try
            {
                champions = await _services.GetChampion(id);
                if (champions.Error)
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage");
            }
            return View(champions);
        }

        // GET: ChampionsController/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var roles = await _services.RoleList();
                var lanes = await _services.RecommededLaneList();

                roles.roles.Insert(0, new RoleEntity { ID = 0, ROLE_NAME = "Select" });
                lanes.Lanes.Insert(0, new RecommededLaneEntity { ID = 0, LANE = "Select" });
                ViewData["roles"] = roles.roles;
                ViewData["lanes"] = lanes.Lanes;
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage");
            }
            return View();
        }

        // POST: ChampionsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)  
        public async Task<ActionResult> Create(ChampionAddEntity championAdd)
        {
            try
            {
                ResultEntity result = new ResultEntity();
                if (!String.IsNullOrEmpty(championAdd.NAME))
                {
                    if (championAdd.IMAGE_FILE != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ChampionsImage");
                        string filePath = Path.Combine(uploadsFolder, championAdd.IMAGE_FILE.FileName);
                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            championAdd.IMAGE_FILE.CopyTo(fileSteam);
                        }

                        championAdd.IMAGE_PATH = "/ChampionsImage/" + championAdd.IMAGE_FILE.FileName;
                    }

                    result = await _services.ChampionAddOrUpdate(championAdd);

                    if (!result.RESULT)
                    {
                        return RedirectToAction("ErrorPage");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // GET: ChampionsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var roles = await _services.RoleList();
                var lanes = await _services.RecommededLaneList();

                roles.roles.Insert(0, new RoleEntity { ID = 0, ROLE_NAME = "Select" });
                lanes.Lanes.Insert(0, new RecommededLaneEntity { ID = 0, LANE = "Select" });
                ViewData["roles"] = roles.roles;
                ViewData["lanes"] = lanes.Lanes;

                var champions = await _services.GetChampion(id);

                ChampionAddEntity championsAdd = new ChampionAddEntity()
                {
                    ID = champions.ID,
                    NAME = champions.NAME,
                    HISTORY = champions.HISTORY,
                    STATS_DAMAGE = champions.STATS_DAMAGE,
                    STATS_DIFFICULITY = champions.STATS_DIFFICULITY,
                    STATS_TOUGHNESS = champions.STATS_TOUGHNESS,
                    STATS_UTILITY = champions.STATS_UTILITY,
                    ROLE_ID = roles.roles.Find(x => x.ROLE_NAME == champions.ROLE).ID,
                    RECOMMENDED_LANE_ID = lanes.Lanes.Find(x => x.LANE == champions.LANE).ID,
                    IMAGE_PATH = champions.IMAGE_PATH
                };

                return View(championsAdd);
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // POST: ChampionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ChampionAddEntity championAdd)
        {
            try
            {
                ResultEntity result = new ResultEntity();
                if (!String.IsNullOrEmpty(championAdd.NAME))
                {
                    if (championAdd.IMAGE_FILE != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ChampionsImage");
                        string filePath = Path.Combine(uploadsFolder, championAdd.IMAGE_FILE.FileName);
                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            championAdd.IMAGE_FILE.CopyTo(fileSteam);
                        }

                        championAdd.IMAGE_PATH = "/ChampionsImage/" + championAdd.IMAGE_FILE.FileName;
                    }
                    result = await _services.ChampionAddOrUpdate(championAdd);
                    if (!result.RESULT)
                    {
                        return RedirectToAction("ErrorPage");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        // GET: ChampionsController/Delete/5
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

        // POST: ChampionsController/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, string name)
        {
            try
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ChampionsImage");
                string filePath = Path.Combine(uploadsFolder, name.Trim() + ".PNG");
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                var result = await _services.ChampionDelete(id);
                if (!result.RESULT)
                {
                    return RedirectToAction("ErrorPage");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }


        [HttpGet]
        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginEntity login)
        {
            if (login.Username == "admin" && login.Password == "1234")
            {
                HttpContext.Session.SetString(sessionKeyLogin, "PASS");
            }
            return RedirectToAction("Index");
        }

        
        public ActionResult Logout()
        {
            HttpContext.Session.SetString(sessionKeyLogin, string.Empty);
            return RedirectToAction("Index");
        }


    }
}
