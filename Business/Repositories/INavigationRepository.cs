using Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XperienceAdapter.Models;

namespace Business.Repositories
{
	public interface INavigationRepository
	{
		/// <summary>
		/// Gets navigation hierarchies of all site cultures.
		/// </summary>
		/// <returns>Dictionary with navigation hierarchies per each site culture.</returns>
		Dictionary<SiteCulture, NavigationItem> GetWholeNavigation();

		/// <summary>
		/// Gets a navigation hierarchy for a specified or actual site culture, further constrained by the starting node alias path.
		/// </summary>
		/// <param name="siteCulture">Site culture.</param>
		/// <returns>Navigation item in a given culture.</returns>
		NavigationItem GetNavigation(SiteCulture? siteCulture = default);

		/// <summary>
		/// Traverses the hierarchy to find a navigation item by node ID.
		/// </summary>
		/// <param name="nodeId">Node ID.</param>
		/// <param name="startPointItem">Starting point navigation item.</param>
		/// <returns>Navigation item.</returns>
		NavigationItem? GetNavigationItemByNodeId(int nodeId, NavigationItem startPointItem);
	}
}
