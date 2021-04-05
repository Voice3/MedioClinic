﻿#define no_suffix
using Business.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XperienceAdapter.Extensions;

namespace MedioClinic.Components.ViewComponents
{
	public class Navigation : ViewComponent
	{
		private readonly INavigationRepository _navigationRepository;

		public Navigation(INavigationRepository navigationRepository)
		{
			_navigationRepository = navigationRepository ?? throw new ArgumentNullException(nameof(navigationRepository));
		}

		public IViewComponentResult Invoke(string placement)
		{
			var currentCulture = Thread.CurrentThread.CurrentUICulture.ToSiteCulture();
			var navigation = _navigationRepository.GetNavigation(currentCulture);

			return View(placement, navigation);
		}
	}
}