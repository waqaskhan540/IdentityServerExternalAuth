using IdentityServerExternalAuth.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerExternalAuth.Controllers
{
    public class HomeController:Controller
    {
        private readonly IProviderRepository _providersRepository;

        public HomeController(IProviderRepository providersRepository)
        {
            _providersRepository = providersRepository ?? throw new ArgumentNullException(nameof(providersRepository));
        }
        public  IActionResult Index()
        {
            ViewBag.Providers = _providersRepository.Get();
            return View();
        }
    }
}
