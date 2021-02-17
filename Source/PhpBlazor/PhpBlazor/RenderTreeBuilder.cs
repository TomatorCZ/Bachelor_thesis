using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Pchp.Core;
using System;

[assembly: PhpExtension]

namespace PhpBlazor
{
    [PhpType]
    public class RenderTreeBuilder
    {
        private Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder;
        PhpComponent component;

        public RenderTreeBuilder(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder, PhpComponent component)
        {
            this.builder = builder;
            this.component = component;
        }

        public void OpenElement(int sequence, string elementName) => builder.OpenElement(sequence, elementName);
        public void CloseElement() => builder.CloseElement();

        public void AddAttribute(int sequence, string name, string? value) => builder.AddAttribute(sequence, name, value);

        #region events
        public void AddEventCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create(component, new Action(() => value.Invoke(ctx))));
        }

        public void AddEventClipboardCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ClipboardEventArgs>(component, new Action<ClipboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventDragCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<DragEventArgs>(component, new Action<DragEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventErrorCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ErrorEventArgs>(component, new Action<ErrorEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventEventCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<EventArgs>(component, new Action<EventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventFocusCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<FocusEventArgs>(component, new Action<FocusEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventChangeCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ChangeEventArgs>(component, new Action<ChangeEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventKeyboardCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<KeyboardEventArgs>(component, new Action<KeyboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventMouseCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<MouseEventArgs>(component, new Action<MouseEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventPointerCallback(Context ctx, int sequence, string name, IPhpCallable value)     
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<PointerEventArgs>(component, new Action<PointerEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventWheelCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<WheelEventArgs>(component, new Action<WheelEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventProgressCallback(Context ctx, int sequence, string name, IPhpCallable value)    
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ProgressEventArgs>(component, new Action<ProgressEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventTouchCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            builder.AddAttribute(sequence, name, EventCallback.Factory.Create<TouchEventArgs>(component, new Action<TouchEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }
        #endregion
    }

}
