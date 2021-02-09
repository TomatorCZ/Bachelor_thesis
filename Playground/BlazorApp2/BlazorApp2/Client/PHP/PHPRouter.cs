using Devsense.PHP.Syntax;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Pchp.Core;
using Pchp.Core.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml;

namespace BlazorApp2.Client.PHP
{
	[Route("/{Script}")]
	public class PHPRouter : ComponentBase
	{
		[Parameter] public string Script { get; set; }
		[Inject] NavigationManager NavManager { get; set; }
		[Inject] IJSRuntime JSRuntime { get; set; }

		private Context _ctx { get; set; }
		private Context.ScriptInfo _exeScript { get; set; }
		private EventHelper _event { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			string toRender = evalScript();
			builder.AddMarkupContent(1, toRender);
		}

		private string evalScript()
		{
			if (!_exeScript.IsValid)
			{
				throw new Exception("Invalid Script!");
			}

			using MemoryStream buffer = new MemoryStream();
			using StreamWriter writer = new StreamWriter(buffer, System.Text.Encoding.UTF8);
			_ctx.Output = writer;
			_exeScript.Evaluate(_ctx, _ctx.Globals, null);

			writer.Flush();
			buffer.Position = 0;

			using StreamReader reader = new StreamReader(buffer, System.Text.Encoding.UTF8);
			return reader.ReadToEnd();
		}

		protected override void OnInitialized()
		{
			base.OnInitialized();
			_ctx = Context.CreateEmpty();
			_event = new EventHelper(JSRuntime, this);

			_ctx.DeclareFunction("registerEvent", new Action<string>((method) => _event.RegisterEvent(method)));

			NavManager.LocationChanged += handleLocationChanged;
			assignQuerryString(_ctx);
		}

		private void handleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			_ctx.Get.Clear();
			assignQuerryString(_ctx);
			StateHasChanged();
		}

		private void assignQuerryString(Context ctx)
		{
			//https://chrissainty.com/working-with-query-strings-in-blazor/
			var uri = NavManager.ToAbsoluteUri(NavManager.Uri);

			QueryHelpers.ParseQuery(uri.Query).Foreach((item) => ctx.Get.Add(item.Key, item.Value));
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Assembly phpassembly = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "ClassLibrary1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			Context.AddScriptReference(phpassembly);

			_exeScript = Context.TryGetDeclaredScript(Script);
		}

		public void HandleEvent(Action<Context> action)
		{
			Console.WriteLine(_ctx.Globals["counter"]);
			action(_ctx);
			Console.WriteLine(_ctx.Globals["counter"]);
			StateHasChanged();
			Console.WriteLine(_ctx.Globals["counter"]);
		}
	}

	public class EventHelper : IDisposable
	{
		private PHPRouter _php;
		private readonly IJSRuntime _runtime;
		private DotNetObjectReference<EventHelper> objRef;

		public EventHelper(IJSRuntime runtime, PHPRouter php)
		{
			_runtime = runtime;
			_php = php;
		}

		[JSInvokable]
		public void CallHandler(string method)
		{
			_php.HandleEvent((ctx) => ctx.Call(method));
		}

		public void RegisterEvent(string method)
		{
			objRef ??= DotNetObjectReference.Create(this);

			((IJSInProcessRuntime)_runtime).InvokeVoid("eventManager.assignEvent", method, objRef);
		}

		void IDisposable.Dispose()
		{
			objRef?.Dispose();
		}
	}
}
