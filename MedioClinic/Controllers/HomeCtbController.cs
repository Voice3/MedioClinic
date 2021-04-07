﻿using Business.Models;
using CMS.DocumentEngine;
using Core.Configuration;
using Kentico.Content.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;
using MedioClinic.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XperienceAdapter.Repositories;

[assembly: RegisterPageRoute(CMS.DocumentEngine.Types.MedioClinic.HomePage.CLASS_NAME, typeof(HomeCtbController))]
namespace MedioClinic.Controllers
{
    public class HomeCtbController : BaseController
    {
        private readonly IPageDataContextRetriever _pageDataContextRetriever;

        private readonly IPageRepository<HomePage, CMS.DocumentEngine.Types.MedioClinic.HomePage> _homePageRepository;

        private readonly IPageRepository<CompanyService, CMS.DocumentEngine.Types.MedioClinic.CompanyService> _companyServiceRepository;

        public HomeCtbController(
            ILogger<HomeCtbController> logger,
            IOptionsMonitor<XperienceOptions> optionsMonitor,
            IPageDataContextRetriever pageDataContextRetriever,
            IPageRepository<HomePage, CMS.DocumentEngine.Types.MedioClinic.HomePage> homePageRepository,
            IPageRepository<CompanyService, CMS.DocumentEngine.Types.MedioClinic.CompanyService> companyServiceRepository)
            : base(logger, optionsMonitor)
        {
            _pageDataContextRetriever = pageDataContextRetriever ?? throw new ArgumentNullException(nameof(pageDataContextRetriever));
            _homePageRepository = homePageRepository ?? throw new ArgumentNullException(nameof(homePageRepository));
            _companyServiceRepository = companyServiceRepository ?? throw new ArgumentNullException(nameof(companyServiceRepository));
        }

		public async Task<IActionResult> Index(CancellationToken cancellationToken)
		{
			if (_pageDataContextRetriever.TryRetrieve<CMS.DocumentEngine.Types.MedioClinic.HomePage>(out var pageDataContext)
				&& pageDataContext.Page != null)
			{
				var homePath = pageDataContext.Page.NodeAliasPath;

				var homePage = (await _homePageRepository.GetPagesInCurrentCultureAsync(
					cancellationToken,
					filter => filter
						.Path(homePath, PathTypeEnum.Single)
						.TopN(1),
					buildCacheAction: cache => cache
						.Key($"{nameof(HomeCtbController)}|HomePage")
						.Dependencies((_, builder) => builder
							.PageType(CMS.DocumentEngine.Types.MedioClinic.HomePage.CLASS_NAME)),
					includeAttachments: true))
						.FirstOrDefault();

				var companyServices = await _companyServiceRepository.GetPagesInCurrentCultureAsync(
					cancellationToken,
					filter => filter
						.Path(homePath, PathTypeEnum.Children),
					buildCacheAction: cache => cache
						.Key($"{nameof(HomeCtbController)}|CompanyServices")
						.Dependencies((_, builder) => builder
							.PageType(CMS.DocumentEngine.Types.MedioClinic.CompanyService.CLASS_NAME)
							.PagePath(homePath, PathTypeEnum.Children)
							.PageOrder()));

				if (homePage != null && companyServices?.Any() == true)
				{
					var data = (homePage, companyServices);
					var viewModel = GetPageViewModel<(HomePage, IEnumerable<CompanyService>)>(pageDataContext.Metadata, data);

					return View("Home/Index", viewModel);
				}
			}

			return NotFound();
		}
	}
}
