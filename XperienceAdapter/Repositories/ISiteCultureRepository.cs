using Core;
using System;
using System.Collections.Generic;
using System.Text;
using XperienceAdapter.Models;

namespace XperienceAdapter.Repositories
{
	public interface ISiteCultureRepository : IRepository<SiteCulture>
	{
		/// <summary>
		/// Default culture.
		/// </summary>
		SiteCulture? DefaultSiteCulture { get; }

		/// <summary>
		/// Gets culture by its RFC 5646 culture (e.g. "en-US").
		/// </summary>
		/// <param name="isoCode"></param>
		/// <returns></returns>
		SiteCulture? GetByExactIsoCode(string isoCode);
	}
}
