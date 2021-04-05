using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
	public class NavigationItem : BasicPageWithUrlSlug
	{
		/// <summary>
		/// Relative URL.
		/// </summary>
		public string? RelativeUrl { get; set; }

		/// <summary>
		/// Parent navigation item, if any.
		/// </summary>
		public NavigationItem? Parent { get; set; }

		/// <summary>
		/// All parent navigation items, up to the root.
		/// </summary>
		public List<NavigationItem> AllParents { get; } = new List<NavigationItem>();

		/// <summary>
		/// Child navigation items.
		/// </summary>
		public List<NavigationItem> ChildItems { get; } = new List<NavigationItem>();
	}
}
