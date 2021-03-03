using Pchp.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpBlazor
{
    public static class ComponentUtils
    {
        public static void StateHasChanged(Context ctx)
        {
            ((BlazorContext)ctx).ComponentStateHadChanged();
        }

        public static void CallAfterRender(Context ctx, IPhpCallable function) => ((BlazorContext)ctx).CallAfterRender(function);
    }

    /* Implemenation of BlazorUtilities....
    [PhpType]
    public interface iBlazorWritable
    {
        public void writeWithEcho(Context ctx);
        public int writeWithTreeBuilder(RenderTreeBuilder builder, int startIndex);
    }

    [PhpType]
    public class Text : iBlazorWritable
    {
        protected string content;

        public Text(string content)
        {
            this.content = content;
        }

        #region Conversions
        public static implicit operator string(Text text) => text.content;
        public static implicit operator Text(string text) => new Text(text);
        #endregion

        #region iBlazorWritable
        public void writeWithEcho(Context ctx)
        {
            ctx.Echo(content);
        }

        public int writeWithTreeBuilder(RenderTreeBuilder builder, int startIndex)
        {
            builder.AddContent(startIndex++, content);
            return startIndex;
        }
        #endregion
    }

    [PhpType]
    public class Tag : iBlazorWritable
    {
        protected string name;
        protected AttributeCollection attributes;
        protected List<iBlazorWritable> content;

        public Tag(string name)
        {
            this.name = name;
        }

        public PhpValue this[string index] 
        {
            get
            {
                switch (index)
                {
                    case "content":
                        return PhpValue.FromClass(content);
                    case "attributes":
                        return PhpValue.FromClass(attributes);
                    case "name":
                        return name;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        #region iBlazorWritable
        public void writeWithEcho(Context ctx)
        {
            throw new NotImplementedException();
        }

        public int writeWithTreeBuilder(RenderTreeBuilder builder, int startIndex)
        {
            builder.OpenElement(startIndex++, name);

            startIndex = attributes.writeWithTreeBuilder(builder, startIndex);

            content.ForEach(x => startIndex = x.writeWithTreeBuilder(builder, startIndex));

            builder.CloseElement();
            return startIndex;
        }
        #endregion
    }

    [PhpType]
    public class AttributeCollection : iBlazorWritable
    {
        protected Dictionary<string, string> keyValueAttributes;
        protected List<string> valueAttributes;
        protected Dictionary<string, IPhpCallable> events;
        protected CssBuilder styles;
        protected ClassBuilder classes;

        public AttributeCollection()
        {
            keyValueAttributes = new Dictionary<string, string>();
            valueAttributes = new List<string>();
            events = new Dictionary<string, IPhpCallable>();
        }

        public void addEvent(string name, IPhpCallable handler)
        {
            events.Add(name, handler);
        }

        public void removeEvent(string name)
        {
            if (events.ContainsKey(name))
                events.Remove(name);
        }

        public PhpValue this[string index] 
        {
            get
            {
                if (keyValueAttributes.ContainsKey(index))
                    return keyValueAttributes[index];
                else if (valueAttributes.Contains(index))
                    return index;
                else if (events.ContainsKey(index))
                    return PhpValue.FromClass(events[index]);
                else if (index == "styles")
                {
                    styles ??= new CssBuilder();
                    return PhpValue.FromClass(styles);
                }
                else if (index == "class")
                {
                    classes ??= new ClassBuilder();
                    return PhpValue.FromClass(classes);
                }
                else
                {
                    return PhpValue.Null;
                }
            }
            set
            {
                if (String.IsNullOrEmpty(index))
                {
                    valueAttributes.Add(value.ToString());
                }
                else if (index != "styles" || index != "classes")
                {
                    keyValueAttributes.Add(index, value.ToString());
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        #region iBlazorWritable
        public void writeWithEcho(Context ctx)
        {
            throw new NotImplementedException();
        }

        public int writeWithTreeBuilder(RenderTreeBuilder builder, int startIndex)
        {
            foreach (var item in keyValueAttributes)
            {
                builder.AddAttribute(startIndex++, item.Key, item.Value);
            }

            valueAttributes.ForEach(x => builder.AddAttribute(startIndex++, x, null));

            var ctx = Context.CreateEmpty();
            foreach (var item in events)
            {
                //item.Value.Invoke(ctx, )
            }
            throw new NotImplementedException();
        }
        #endregion
    }

    public class CssBuilder
    { }

    public class ClassBuilder
    { }
    */
}
