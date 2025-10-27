using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConflictResolution.Services
{
	public class ConflictResolutionService
	{
		// If the Deleted and addnew of new html not touching the changes of latestContent compare to original -> going forward by patching those changes to latest content.
		// Else -> throw conflict exception.
		// The needs: We indentify the lines in text (by hash or index)? adapt text compare to return that info.
		public string Resolve(string originaHtml, string latestContent, string newHtml)
		{

		}
	}
}
