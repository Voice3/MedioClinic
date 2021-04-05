using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XperienceAdapter.Models;

namespace Business.Models
{
	public class BasicPageWithUrlSlug : BasePage
	{
		public override IEnumerable<string> SourceColumns => base.SourceColumns.Concat(new[] { "UrlSlug" });

		/// <summary>
		/// URL slug used for conventional routing.
		/// </summary>
		public string? UrlSlug { get; set; }
	}
}
