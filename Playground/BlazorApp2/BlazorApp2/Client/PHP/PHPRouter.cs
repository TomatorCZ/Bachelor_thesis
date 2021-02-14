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
using System.Threading;
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

		private TimerManager _timerManager { get; set; }

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
			Assembly phpassembly1 = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "ClassLibrary1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			Assembly phpassembly2 = AppDomain.CurrentDomain.GetAssemblies().First(x => x.FullName == "Asteroids, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			Context.AddScriptReference(phpassembly1);
			Context.AddScriptReference(phpassembly2);

			_event = new EventHelper(JSRuntime, this);
			_timerManager = new TimerManager();
			_ctx = CreateContext(_event);

			

			NavManager.LocationChanged += handleLocationChanged;
			
		}

		private Context CreateContext(EventHelper @event)
		{
			var ctx = Context.CreateEmpty(); ;
			ctx.DeclareFunction("logPHP", new Action<string>((msg) => Console.WriteLine(msg)));
			ctx.DeclareFunction("stateHasChangedPHP", new Action(() => this.StateHasChanged()));

			ctx.DeclareFunction("createTimerPHP", new Action<string, double, string>((name, interval, method) => _timerManager.AddTimer(name,interval,() => HandleEvent((ctx=> ctx.Call(method))))));
			ctx.DeclareFunction("startTimerPHP", new Action<string>((name) => _timerManager.StartTimer(name)));
			ctx.DeclareFunction("stopTimerPHP", new Action<string>((name) => _timerManager.StopTimer(name)));
			ctx.DeclareFunction("deleteTimerPHP", new Action<string>((name) => _timerManager.RemoveTimer(name)));
			return ctx;
		}

		private void handleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			_ctx = CreateContext(_event);

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

			_ctx = CreateContext(_event);
			assignQuerryString(_ctx);

			_exeScript = Context.TryGetDeclaredScript(Script);
		}

		public void HandleEvent(Action<Context> action)
		{
			action(_ctx);
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

			objRef = DotNetObjectReference.Create(this);
			((IJSInProcessRuntime)_runtime).InvokeVoid("eventManager.setEventHandler", objRef);
		}

		[JSInvokable]
		public void CallHandler(string method, params object[] args)
		{
			_php.HandleEvent((ctx) => ctx.Call(method, args));
		}

		void IDisposable.Dispose()
		{
			objRef?.Dispose();
		}
	}


	public class TimerManager
	{
		private Dictionary<string, System.Timers.Timer> _timers;

		public TimerManager()
		{
			_timers = new Dictionary<string, System.Timers.Timer>();
		}

		public void AddTimer(string name, double interval, Action elapsed)
		{
			if (!_timers.ContainsKey(name))
			{
				_timers.Add(name, new System.Timers.Timer(interval));
				_timers[name].Elapsed += (sender, e) => elapsed();
				//_timers[name].Elapsed += (sender, e) => Console.WriteLine("Tick");
			}
		}

		public void StartTimer(string name)
		{
			if (_timers.TryGetValue(name, out var timer))
			{
				timer.Start();
			}
		}

		public void StopTimer(string name)
		{
			if (_timers.TryGetValue(name, out var timer))
			{
				timer.Stop();
			}
		}

		public void RemoveTimer(string name)
		{
			_timers.Remove(name);
		}

	}
}
