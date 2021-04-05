﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Models
{
	public class NamePerexText : BasicPageWithUrlSlug
	{
		public override IEnumerable<string> SourceColumns => base.SourceColumns.Concat(new[] { "Perex", "Text" });

		/// <summary>
		/// Page perex.
		/// </summary>
		public string? Perex { get; set; }

		/// <summary>
		/// Page text.
		/// </summary>
		public string? Text { get; set; }
	}
}
