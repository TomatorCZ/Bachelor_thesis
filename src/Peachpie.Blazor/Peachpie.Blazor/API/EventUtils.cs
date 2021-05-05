using Pchp.Core;
using System;

namespace Peachpie.Blazor
{
    public static class EventHelper
    {
        public static void AddSimpleEventListener(Context ctx, object target, string eventName, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, EventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            var eventReflection = target.GetType().GetEvent(eventName);
            eventReflection.AddEventHandler(target, (EventHandler)HandlerDelegate);
        }

        public static void AddEventListener<TEventArgs>(Context ctx, object target, string eventName, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, TEventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            var eventReflection = target.GetType().GetEvent(eventName);
            eventReflection.AddEventHandler(target, (EventHandler<TEventArgs>)HandlerDelegate);
        }
    }
}
