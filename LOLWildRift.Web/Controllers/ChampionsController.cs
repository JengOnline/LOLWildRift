using Grpc.Core;
using LOLWildRift.Web.Models;
using LOLWildRift.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LOLWildRift.Web.Controllers
{
    public class ChampionsController : Controller
    {
        private readonly LOLWildRiftService services;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ChampionsController(LOLWildRiftService _services, IWebHostEnvironment _webHostEnvironment)
        {
            services = _services;
            webHostEnvironment = _webHostEnvironment;
        }

        // List: ChampionsController
        public async Task<ActionResult> Index()
        {
            ChampionsList champions = new ChampionsList();
            try
            {
                champions = await services.ChampionList();
            }
            catch (Exception)
            {

            }
            return View(champions.Champions);
        }

        // GET: ChampionsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            ChampionsEntity champions = new ChampionsEntity();
            try
            {
                champions = await services.GetChampion(id);
            }
            catch (Exception)
            {

            }
            return View(champions);
        }

        // GET: ChampionsController/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var roles = await services.RoleList();
                var lanes = await services.RecommededLaneList();

                roles.roles.Insert(0, new RoleEntity { ID = 0, ROLE_NAME = "Select" });
                lanes.Lanes.Insert(0, new RecommededLaneEntity { ID = 0, LANE = "Select" });
                ViewData["roles"] = roles.roles;
                ViewData["lanes"] = lanes.Lanes;


            }
            catch (Exception)
            {

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
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ChampionsImage");
                        string filePath = Path.Combine(uploadsFolder, championAdd.IMAGE_FILE.FileName);
                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            championAdd.IMAGE_FILE.CopyTo(fileSteam);
                        }

                        championAdd.IMAGE_PATH = "/ChampionsImage/" + championAdd.IMAGE_FILE.FileName;
                    }
                    result = await services.ChampionAddOrUpdate(championAdd);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChampionsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChampionsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ChampionsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChampionsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
