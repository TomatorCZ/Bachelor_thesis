<?php namespace HtmlUtilities;

class Timer
{
    const TIMERPREFIX = "timer"; 

    private static int $idForNextTimer = 0;
    private int $id;

    public static function create(int $interval, string $elapsedHandler) : Timer 
    {
        $result = new Timer(self::$idForNextTimer, $elapsedHandler, $interval);
        self::$idForNextTimer++;

        return $result;
    }

    private function __construct(int $id, string $elapsedHandler, int $interval) 
    {
        $this->id = $id;
        createTimerPHP(self::TIMERPREFIX . $id, $interval, $elapsedHandler);
    }

    function __destruct() 
    {
        stopTimerPHP(self::TIMERPREFIX . $this->id);
        deleteTimerPHP(self::TIMERPREFIX . $this->id);
    }
    
    public function start() : void 
    {
        startTimerPHP(self::TIMERPREFIX . $this->id);
    }
    
    public function stop() : void 
    {
        stopTimerPHP(self::TIMERPREFIX . $this->id);
    }
}

interface iHtmlWritable
{
    public function writeHTML() : void;
}

class Tag implements iHtmlWritable, \ArrayAccess
{
    protected string $name;
    protected AttributeCollection $attributes;
    protected array $content;

    public function __construct(string $name) 
    {
        $this->name = $name;
        $this->attributes = new AttributeCollection();
        $this->content = array();
    }

    public function setEvent(string $event, string $handler, array $arguments) : void 
    {
        if (empty($arguments))
        {
            $this->attributes["on$event"] = "window.eventManager.callEvent('$handler');";
        }
        else
        {
            $this->attributes["on$event"] = "window.eventManager.callEvent('$handler', " . implode(", ", $arguments) . ");";
        }
    }

    public function offsetExists ($offset) : bool 
    {
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
            $this->$offset = array();
        }
        else
        {
            throw new \Exception("Unsupported!");
        }   
    }

    public function writeHTML() : void 
    {
        echo "<$this->name";
     
        $this->attributes->writeHTML();
     
        echo ">";

        foreach($this->content as $value)
        {
            $value->writeHTML();
        }

        echo "</$this->name>";
    }
}

class Text implements iHtmlWritable
{
    private string $content;

    public function __construct($content)
    {
        $this->content = $content;
    }

    public function writeHTML() : void
    {
        echo $this->content;
    }
}

class AttributeCollection implements \ArrayAccess, iHtmlWritable
{
    private array $attributes;

    public function __construct()
    {
        $this->attributes = array();
    }

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

    public function writeHTML() : void 
    {
        foreach($this->attributes as $key => $value)
        {
            echo " $key=\"$value\"";
        }
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

function render(iHtmlWritable $element) : void
{
    $element->writeHTML();
}

function logToConsole(string $msg) : void 
{
    logPHP($msg);
}

function stateHasChanged() : void 
{
    stateHasChangedPHP();
}