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
		private static readonly Action<ILogger, string, Exception> _renderingToScript = LoggerMessage.Define<string>(LogLevel.Information, new EventId(1, "RenderingScript"), "Rendering script '{Script}'.");
		private static readonly Action<ILogger, string, Exception> _navigating = LoggerMessage.Define<string>(LogLevel.Information, new EventId(2, "Navigating"), "Navigating to path '{Path}'.");
		private static readonly Action<ILogger, string, Exception> _querryParameters = LoggerMessage.Define<string>(LogLevel.Information, new EventId(3, "GettingQuerry"), "Querry parameters\n{Parameters}");
		private static readonly Action<ILogger, string, Exception> _fetchingFiles = LoggerMessage.Define<string>(LogLevel.Information, new EventId(4, "FetchingFiles"), "Fetched Files\n{Files}");

		public static void Navigating(ILogger logger, string path)
		{
			_navigating(logger, path, null);
		}

		public static void RenderingScript(ILogger logger, string script)
		{
			_renderingToScript(logger, script, null);
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

		public static void FetchingFiles(ILogger logger, List<FormFile> files)
		{
			var filesString = "";
			foreach (var item in files)
			{
				filesString += $"fieldName: {item.fieldName}, id : {item.id}, name : {item.name}, size : {item.size}, type : {item.type}\n";
			}
			_fetchingFiles(logger, filesString, null);
		}
	}
}
