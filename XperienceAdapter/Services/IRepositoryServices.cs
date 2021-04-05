using CMS.Base;
using CMS.Helpers;
using Kentico.Content.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using XperienceAdapter.Repositories;

namespace XperienceAdapter.Services
{
	public interface IRepositoryServices
	{
		ISiteService SiteService { get; }

		ISiteContextService SiteContextService { get; }

		IPageRetriever PageRetriever { get; }

		IPageUrlRetriever PageUrlRetriever { get; }

		IPageAttachmentUrlRetriever PageAttachmentUrlRetriever { get; }

		ISiteCultureRepository SiteCultureRepository { get; }

		IProgressiveCache ProgressiveCache { get; }
	}
}
