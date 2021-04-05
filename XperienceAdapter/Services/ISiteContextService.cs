using Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace XperienceAdapter.Services
{
	public interface ISiteContextService : IService
	{
		/// <summary>
		/// Indicates if preview mode is enabled.
		/// </summary>
		bool IsPreviewEnabled { get; }
	}
}
