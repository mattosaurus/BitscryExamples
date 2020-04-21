using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AADGroupAuthorization.Models;
using Microsoft.AspNetCore.Authorization;
using AADGroupAuthorization.Services;
using Microsoft.Identity.Web;
using Microsoft.Graph;

namespace AADGroupAuthorization.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IMSGraphService _graphService;

        public HomeController(ILogger<HomeController> logger, ITokenAcquisition tokenAcquisition, IMSGraphService graphService)
        {
            _logger = logger;
            _tokenAcquisition = tokenAcquisition;
            _graphService = graphService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "<YOURGROUPID>")]
        public IActionResult Privacy()
        {
            return View();
        }

        [AuthorizeForScopes(Scopes = new[] { Infrastructure.Constants.ScopeUserRead })]
        public async Task<IActionResult> Profile()
        {
            string token = await _tokenAcquisition.GetAccessTokenForUserAsync(new[] { Infrastructure.Constants.ScopeUserRead, Infrastructure.Constants.ScopeDirectoryReadAll });

            User me = await _graphService.GetMeAsync(token);
            ViewData["Me"] = me;

            try
            {
                // Get user photo
                ViewData["Photo"] = await _graphService.GetMyPhotoAsync(token);
            }
            catch (System.Exception)
            {
                ViewData["Photo"] = null;
            }

            IList<Group> groups = await _graphService.GetMyMemberOfGroupsAsync(token);

            ViewData["Groups"] = groups;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
