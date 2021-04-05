#define no_suffix
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedioClinic.Views.Shared.Components.Footer
{
	public class Footer : ViewComponent
	{
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
