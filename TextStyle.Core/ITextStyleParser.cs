using System;
using System.Collections.Generic;
using Occur.TextStyles.Core;

namespace Occur.TextStyles.Core
{
	public interface ITextStyleParser
	{
		Dictionary<string, TextStyleParameters> Parse (string css);

		string ParseToCSSString (string tagName, TextStyleParameters style);
	}
}

