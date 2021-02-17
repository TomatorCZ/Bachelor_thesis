using Pchp.Core;
using System;

[assembly: PhpExtension]

namespace PhpBlazor
{
    public static class EventHelper
    {
        public static void addSimpleEventListener(Context ctx, object target, string eventName, IPhpCallable handler)
        {
            void HandlerDelegate(object sender, EventArgs args)
            {
                handler.Invoke(ctx, PhpValue.FromClr(sender), PhpValue.FromClass(args));
            }

            var eventReflection = target.GetType().GetEvent(eventName);
            eventReflection.AddEventHandler(target, (EventHandler)HandlerDelegate);
        }

        public static void addEventListener<TEventArgs>(Context ctx, object target, string eventName, IPhpCallable handler)
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
