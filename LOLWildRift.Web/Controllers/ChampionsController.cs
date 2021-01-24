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

        public async Task<ActionResult> Index(string searchString)
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                AlreadyLoggedIn();

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

        public async Task<ActionResult> Create()
        {
            try
            {
                var roles = await _services.RoleList();
                var lanes = await _services.RecommendedLaneList();

                roles.roles.Insert(0, new RoleEntity { ID = 0, ROLE_NAME = "Select" });
                lanes.Lanes.Insert(0, new RecommendedLaneEntity { ID = 0, LANE = "Select" });
                ViewData["roles"] = roles.roles;
                ViewData["lanes"] = lanes.Lanes;
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var roles = await _services.RoleList();
                var lanes = await _services.RecommendedLaneList();

                roles.roles.Insert(0, new RoleEntity { ID = 0, ROLE_NAME = "Select" });
                lanes.Lanes.Insert(0, new RecommendedLaneEntity { ID = 0, LANE = "Select" });
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

        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }

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
            HttpContext.Session.SetString(sessionKeyLogin, string.Empty);
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
            try
            {
                if (login.Username == "admin" && login.Password == "1234")
                {
                    HttpContext.Session.SetString(sessionKeyLogin, "PASS");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult Logout()
        {
            HttpContext.Session.SetString(sessionKeyLogin, string.Empty);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Role()
        {
            try
            {
                AlreadyLoggedIn();
                var result = await _services.RoleList();
                return View(result.roles);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult RoleCreate()
        {
            try
            {
                AlreadyLoggedIn();
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RoleCreate(RoleEntity role)
        {
            try
            {
                var result = await _services.RoleAddOrUpdate(role);
                if (result.RESULT)
                {
                    return RedirectToAction("Role");
                }
                else return RedirectToAction("ErrorPage");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPut]
        public async Task<ActionResult> RoleEdit(int id, string role)
        {
            try
            {
                var req = new RoleEntity() { ID = id, ROLE_NAME = role };
                var result = await _services.RoleAddOrUpdate(req);
                return Ok(result);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RoleDelete(int id)
        {
            try
            {
                var result = await _services.RoleDelete(id);
                if (result.RESULT)
                {
                    return RedirectToAction("Role");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public async Task<ActionResult> RecommendedLane()
        {
            try
            {
                AlreadyLoggedIn();
                var result = await _services.RecommendedLaneList();
                return View(result.Lanes);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public ActionResult RecommendedLaneCreate()
        {
            try
            {
                AlreadyLoggedIn();
                return View();
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RecommendedLaneCreate(RecommendedLaneEntity recommendedLane)
        {
            try
            {
                var result = await _services.RecommendedLaneAddOrUpdate(recommendedLane);
                if (result.RESULT)
                {
                    return RedirectToAction("RecommendedLane");
                }
                else return RedirectToAction("ErrorPage");
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpPut]
        public async Task<ActionResult> RecommendedLaneEdit(int id, string lane)
        {
            try
            {
                var req = new RecommendedLaneEntity() { ID = id, LANE = lane };
                var result = await _services.RecommendedLaneAddOrUpdate(req);
                return Ok(result);
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> RecommendedLaneDelete(int id)
        {
            try
            {
                var result = await _services.RecommendedLaneDelete(id);
                if (result.RESULT)
                {
                    return RedirectToAction("RecommendedLane");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
            catch
            {
                return RedirectToAction("ErrorPage");
            }
        }

        

        private void AlreadyLoggedIn()
        {
            ViewData[sessionKeyLogin] = string.IsNullOrEmpty(HttpContext.Session.GetString(sessionKeyLogin))
                 ? string.Empty
                 : HttpContext.Session.GetString(sessionKeyLogin);
        }


    }
}
