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
        private Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder _builder;
        PhpComponent _component;

        public RenderTreeBuilder(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder, PhpComponent component)
        {
            this._builder = builder;
            this._component = component;
        }

        public void OpenElement(int sequence, string elementName) => _builder.OpenElement(sequence, elementName);
        public void CloseElement() => _builder.CloseElement();

        public void AddAttribute(int sequence, string name, string? value) => _builder.AddAttribute(sequence, name, value);

        public void AddContent(int sequence, string textContent) => _builder.AddContent(sequence, textContent);
        
        //TODO: Implement reminding function from RenderTreeBuilder...
        
        #region events
        public void AddEventCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create(_component, new Action(() => value.Invoke(ctx))));
        }

        public void AddEventClipboardCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ClipboardEventArgs>(_component, new Action<ClipboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventDragCallback(Context ctx, int sequence, string name, IPhpCallable value)
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<DragEventArgs>(_component, new Action<DragEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventErrorCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ErrorEventArgs>(_component, new Action<ErrorEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventEventCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<EventArgs>(_component, new Action<EventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventFocusCallback(Context ctx, int sequence, string name, IPhpCallable value)        
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<FocusEventArgs>(_component, new Action<FocusEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventChangeCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ChangeEventArgs>(_component, new Action<ChangeEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventKeyboardCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<KeyboardEventArgs>(_component, new Action<KeyboardEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventMouseCallback(Context ctx, int sequence, string name, IPhpCallable value)       
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<MouseEventArgs>(_component, new Action<MouseEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventPointerCallback(Context ctx, int sequence, string name, IPhpCallable value)     
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<PointerEventArgs>(_component, new Action<PointerEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventWheelCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<WheelEventArgs>(_component, new Action<WheelEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventProgressCallback(Context ctx, int sequence, string name, IPhpCallable value)    
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<ProgressEventArgs>(_component, new Action<ProgressEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }

        public void AddEventTouchCallback(Context ctx, int sequence, string name, IPhpCallable value)      
        {
            _builder.AddAttribute(sequence, name, EventCallback.Factory.Create<TouchEventArgs>(_component, new Action<TouchEventArgs>((e) => value.Invoke(ctx, PhpValue.FromClass(e)))));
        }
        #endregion
    }

}
