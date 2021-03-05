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

    [PhpType]
    public interface iBlazorWritable
    {
        public int writeWithTreeBuilder(Context ctx, RenderTreeBuilder builder, int startIndex);
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
        public int writeWithTreeBuilder(Context ctx, RenderTreeBuilder builder, int startIndex)
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

        public Tag():this("div")
        { }

        public Tag(string name)
        {
            this.name = name;
            attributes = new AttributeCollection();
            content = new List<iBlazorWritable>();
        }

        public void __construct(string name)
        {
            this.name = name;
        }

        public PhpAlias this[string index] 
        {
            get
            {
                switch (index)
                {
                    case "content":
                        return PhpValue.FromClass(content).AsPhpAlias();
                    case "attributes":
                        return PhpValue.FromClass(attributes).AsPhpAlias();
                    case "name":
                        return new PhpAlias(name);
                    default:
                        throw new ArgumentException();
                }
            }
        }

        #region iBlazorWritable
        public int writeWithTreeBuilder(Context ctx, RenderTreeBuilder builder, int startIndex)
        {
            builder.OpenElement(startIndex++, name);

            startIndex = attributes.writeWithTreeBuilder(ctx, builder, startIndex);

            content.ForEach(x => startIndex = x.writeWithTreeBuilder(ctx, builder, startIndex));

            builder.CloseElement();
            return startIndex;
        }
        #endregion
    }
    
    [PhpType]
    public class AttributeCollection : iBlazorWritable, ArrayAccess
    {
        protected PhpArray attributes;
        protected Dictionary<string, IPhpCallable> events;
        protected CssBuilder styles;
        protected ClassBuilder classes;

        public AttributeCollection()
        {
            attributes = new PhpArray();
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

        #region iBlazorWritable
        public int writeWithTreeBuilder(Context ctx, RenderTreeBuilder builder, int startIndex)
        {
            foreach (var item in attributes)
            {
                if (!item.Key.IsString)
                    builder.AddAttribute(startIndex++, item.Value.ToString(), null);
                else
                    builder.AddAttribute(startIndex++, item.Key.String, item.Value.ToString());
            }

            if (styles != null)
                builder.AddAttribute(startIndex++, "style", styles.ToString());

            if (classes != null)
                builder.AddAttribute(startIndex++, "class", classes.ToString());

            foreach (var item in events)
            {
                item.Value.Invoke(ctx, startIndex++, PhpValue.FromClass(builder));
            }

            return startIndex;
        }
        #endregion

        #region ArrayAccess
        public PhpValue offsetGet(PhpValue offset)
        {
            if (attributes.ContainsKey(offset))
                return attributes[offset].AsPhpAlias();
            else if (offset.AsString() == "style")
            {
                styles ??= new CssBuilder();
                return PhpValue.FromClass(styles).AsPhpAlias();
            }
            else if (offset.AsString() == "class")
            {
                classes ??= new ClassBuilder();
                return PhpValue.FromClass(classes).AsPhpAlias();
            }
            else
                throw new ArgumentException();
        }

        public void offsetSet(PhpValue offset, PhpValue value)
        {
            attributes[offset] = value;
        }

        public void offsetUnset(PhpValue offset)
        {
            attributes.Remove(offset);
        }

        public bool offsetExists(PhpValue offset)
        {
            return attributes.ContainsKey(offset);
        }
        #endregion
    }
    
    [PhpType]
    public class CssBuilder : ArrayAccess
    {
        protected Dictionary<string, string> _styles;

        public CssBuilder()
        {
            _styles = new Dictionary<string, string>();
        }

        #region ArrayAccess
        public bool offsetExists(PhpValue offset)
        {
            return _styles.ContainsKey(offset.AsString());
        }

        public PhpValue offsetGet(PhpValue offset)
        {
            if (offsetExists(offset))
                return _styles[offset.AsString()];
            else
                return PhpValue.False;
        }

        public void offsetSet(PhpValue offset, PhpValue value)
        {
            _styles[offset.AsString()] = value.AsString();
        }

        public void offsetUnset(PhpValue offset)
        {
            _styles.Remove(offset.AsString());
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _styles)
            {
                sb.Append(item.Key);
                sb.Append(":");
                sb.Append(item.Value);
                sb.Append(";");
            }

            return sb.ToString();
        }
    }

    [PhpType]
    public class ClassBuilder
    {
        protected List<string> _classes;

        public ClassBuilder()
        {
            _classes = new List<string>();
        }

        public void add(string @class) => _classes.Add(@class);

        public void remove(string @class)
        {
            _classes.Remove(@class);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in _classes)
            {
                sb.Append(item);
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
