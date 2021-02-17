<?php namespace BlazorUtilities;

interface iBlazorWritable
{
	public function writeWithEcho() : void;

	public function writeWithTreeBuilder($builder, int $startIndex) : int;
}

class Tag implements iBlazorWritable, \ArrayAccess
{
	protected string $name;
    protected AttributeCollection $attributes;
    protected array $content;

    public function __construct(string $name) 
    {
        $this->name = $name;
    }

	// ArrayAccess

	public function offsetExists ($offset) : bool 
	{
        if ($offset === "attributes" && !$this->attributes)
        {
            $this->attributes = new AttributeCollection();
		}

        if ($offset === "content" && !$this->content)
        {
            $this->content = array();
		}


		return $offset === "name" || $offset === "attributes" || $offset === "content";
	}

	public function &offsetGet ($offset) 
	{
		if ($this->offsetExists($offset))
		{
			return $this->$offset;
		}
		else
		{
			throw new \Exception("Unsupported!");
		}
	}

	public function offsetSet ($offset, $value) : void 
	{
		if (is_null($offset) || ($offset !== "attributes" && $offset !== "content"))
        {
            throw new \Exception("Unsupported!");
        }
        else
        {
            $this->$offset = $value;
        }
	}

	public function offsetUnset ($offset) : void 
	{
		if ($offset === "attributes" || $offset === "content")
        {
            unset($this->$offset);
        }
        else
        {
            throw new \Exception("Unsupported!");
        }  
	}

	//iBlazorWritable

	public function writeWithEcho() : void 
    {
        //TODO: Implement
        // Event has to be called from Javascript!
        throw new \Exception("Not Implemented");
    }

	public function writeWithTreeBuilder($builder, int $startIndex) : int
    {
        $builder->OpenElement($startIndex++, $this->name);

        $startIndex = $this["attributes"]->writeWithTreeBuilder($builder, $startIndex);

        foreach ($this->content as $fragment)
        {
            $startIndex = $fragment->writeWithTreeBuilder($builder, $startIndex);
        }

        $builder->CloseElement();

        return $startIndex;
    }
}

class Text implements iBlazorWritable
{
    private string $content;

    public function __construct($content)
    {
        $this->content = $content;
    }
    
	//iBlazorWritable

	public function writeWithEcho() : void 
    {
        //TODO: Implement
        throw new \Exception("Not Implemented");
    }

	public function writeWithTreeBuilder($builder, int $startIndex) : int 
    {
        //TODO: Implement
        throw new \Exception("Not Implemented");
    }
}

class AttributeCollection implements \ArrayAccess, iBlazorWritable
{
    private array $attributes;

    public function __construct()
    {
        $this->attributes = array();
    }
    
	// ArrayAccess

    public function offsetExists ($offset) : bool 
    {
        return isset($this->attributes[$offset]);
    }

    public function &offsetGet ($offset) 
    {
        if ($this->offsetExists($offset))
        {
            return $this->attributes[$offset];
        }
        else if ($offset === "style")
        {
            $this->attributes[$offset] = new CssCollection();
            return $this->attributes[$offset];
        }
        else if ($offset === "class")
        {
            $this->attributes[$offset] = new ClassCollection();
            return $this->attributes[$offset];
        }
        {
            throw new \Exception("Unsupported!");
        }
    }

    public function offsetSet ($offset, $value) : void 
    {
        if (is_null($offset) || $offset === "style"  || $offset === "class")
        {
            throw new \Exception("Unsupported!");
        }
        else
        {
            $this->attributes[$offset] = $value;
        }
    }

    public function offsetUnset ($offset) : void 
    {
        unset($this->attributes[$offset]);
    }
    
	//iBlazorWritable

	public function writeWithEcho() : void 
    {
        //TODO: Implement
        throw new \Exception("Not Implemented");
    }

	public function writeWithTreeBuilder($builder, int $startIndex) : int 
    {
        foreach($this->attributes as $key => $value)
        {
            $builder->AddAttribute($startIndex++, strval($key), strval($value));
        }

        return $startIndex;
    }
}

class CssCollection implements \ArrayAccess 
{
    private array $styles;

    public function __construct()
    {
        $this->styles = array();
    }

    public function offsetExists($offset) : bool 
    {
        return isset($this->styles[$offset]);
    }

    public function offsetGet($offset) 
    {
        if ($this->offsetExists($offset))
        {
            return $this->styles[$offset] ;
        }
        else
        {
            throw new \Exception("Unsupported!");
        }
    }

    public function offsetSet($offset, $value ) : void 
    {
        if (is_null($offset)) 
        {
            throw new \Exception("Unsupported!");
        } else 
        {
            $this->styles[$offset] = $value;
        }
    }

    public function offsetUnset($offset) : void 
    {
        unset($this->styles[$offset]);
    }

    public function __toString () : string
    {
        $result = ""; 
        foreach($this->styles as $key => $value)
        {
            $result = $result . "$key:$value;";
        }

        return $result;
    }
}

class ClassCollection
{
    private array $classes;

    public function __construct()
    {
        $this->classes = array();
    }

    public function add(string $class) : void 
    {
        $this->classes["$class"] = true;
    }

    public function remove(string $class) : void 
    {
            unset($this->classes["$class"]);
    }

    public function __toString () : string 
    {
        $result = ""; 
        foreach($this->classes as $key => $value)
        {
            $result = $result . "$key ";
        }

        return $result;
    }
}
