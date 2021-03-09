using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
	public static class Log
	{
		private static readonly Action<ILogger, string, string, Exception> _navigatingToScript = LoggerMessage.Define<string, string>(LogLevel.Information, new EventId(1, "NavigatingToScript"), "Navigating to script {Script} in response to path '{Path}'.");
		private static readonly Action<ILogger, string, Exception> _querryParameters = LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "NavigatingToScript"), "Querry parameters\n{parameters}");

		public static void NavigatingToScript(ILogger logger, string script, string path)
		{
			_navigatingToScript(logger, script, path, null);
		}

		public static void QuerryParameters(ILogger logger, Dictionary<string, StringValues> parameters)
		{
			var parametersString = "";
            foreach (var item in parameters)
            {
				parametersString += $"{item.Key} : {item.Value}\n";
			}
			_querryParameters(logger, parametersString, null);
		}
	}
}
