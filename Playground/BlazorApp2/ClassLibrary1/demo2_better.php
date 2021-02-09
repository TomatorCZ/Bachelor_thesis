<?php
// ---Declerative section---

interface iHtmlWritable
{
    public function WriteHTML() : void;
}

class Text implements iHtmlWritable
{
    private string $content;

    public function __construct($content)
    {
        $this->content = $content;
    }

    public function WriteHTML() : void
    {
        echo $this->content;
    }
}

class Tag implements iHtmlWritable, ArrayAccess
{
    protected string $name;
    protected array $attributes;
    protected array $content;

    public function __construct(string $name)
    {
        $this->name = $name;
        $this->attributes = array();
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
            throw new Exception("Unsupported!");
        }
    }

    public function offsetSet ($offset, $value) : void 
    {
        if (is_null($offset) || $offset === "name")
        {
            throw new Exception("Unsupported!");
        }
        else
        {
            if ($offset === "attributes" || $offset === "content")
            {
                $this->$offset = $value;
            }
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
            throw new Exception("Unsupported!");
        }    
    }

    public function WriteHTML() : void
    {
        echo "<$this->name";

        foreach($this->attributes as $key => $value)
        {
            echo " $key=\"$value\"";
        }

        echo ">";

        foreach($this->content as $value)
        {
            $value->WriteHTML();
        }
        
        echo "</$this->name>";
    }
}

class CssCollection implements ArrayAccess
{
    private array $styles; 

    public function __construct()
    {
        $this->style = array();
    }

    public function offsetExists($offset) : bool 
    {
        return isset($this->styles[$offset]);
    }

    public function offsetGet($offset) 
    {
        return $this->offsetExists($offset) ? $this->styles[$offset] : null;
    }

    public function offsetSet($offset, $value ) : void 
    {
        if (is_null($offset)) 
        {
            throw new Exception("Unsupported!");
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

abstract class Entity implements iHtmlWritable
{
    protected array $position;
    protected Tag $tag;

    public function __construct(array $position) 
    {
        $this->position = $position;
        $this->tag = new Tag("div");
        $this->tag["attributes"]["style"] = new CssCollection();
    }

    public function WriteHTML() : void
    {
        $this->tag["attributes"]["style"]["top"] = $this->position["y"] . "px";
        $this->tag["attributes"]["style"]["left"] = $this->position["x"] . "px";
        $this->tag->WriteHTML();
    }
}

abstract class MovableEntity extends Entity
{
    protected array $direction;

    public function __construct(array $position, array $direction) 
    {
        parent::__construct($position);
        $this->direction = $direction;
    }

    public function Move(float $time) : void
    {
        $this->position["x"] = $this->direction["x"] * $time + $this->position["x"]; 
        $this->position["y"] = $this->direction["y"] * $time + $this->position["y"]; 
    }

    public function ChangeDirection(array $direction) : void
    {
        $this->direction = $direction;
    }
}

class Asteroid extends MovableEntity
{ 
    const DEFAULT_SPEED = 1;

    public function __construct(array $position, array $direction)
    {
        parent::__construct($position, $direction);
        $this->tag["attributes"]["style"]["background-color"] = "black";
        $this->tag["attributes"]["style"]["height"] = "50px";
        $this->tag["attributes"]["style"]["width"] = "50px";
        $this->tag["attributes"]["style"]["position"] = "absolute";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, [0, self::DEFAULT_SPEED]);
    }
}

class Bullet extends MovableEntity
{
    const DEFAULT_SPEED = 1;

    public function __construct(array $position, array $direction)
    {
        parent::__construct($position, $direction);
        $this->tag["attributes"]["style"]["background-color"] = "red";
        $this->tag["attributes"]["style"]["height"] = "40px";
        $this->tag["attributes"]["style"]["width"] = "10px";
        $this->tag["attributes"]["style"]["position"] = "absolute";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, [0, self::DEFAULT_SPEED]);
    }
}

class Rocket extends MovableEntity
{
    const DEFAULT_SPEED = 0;

    public function __construct(array $position, array $direction)
    {
        parent::__construct($position, $direction);
        $this->tag["attributes"]["style"]["background-color"] = "blue";
        $this->tag["attributes"]["style"]["height"] = "50px";
        $this->tag["attributes"]["style"]["width"] = "50px";
        $this->tag["attributes"]["style"]["position"] = "absolute";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, [self::DEFAULT_SPEED, self::DEFAULT_SPEED]);
    }
}

class Background extends Entity
{
    public function __construct() 
    {
        parent::__construct(["x" => 0, "y" => 0]);
        $this->tag["attributes"]["style"]["background-color"] = "grey";
        $this->tag["attributes"]["style"]["height"] = "800px";
        $this->tag["attributes"]["style"]["width"] = "100%";
        $this->tag["attributes"]["style"]["position"] = "absolute";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function CreateDefault()
    {
        return new self();
    }
}

class Application implements iHTMLWritable
{
    private array $GameState;
    private array $ApplicationSettings;
    private array $Controls;

    public function __construct($GameState, $ApplicationSettings) 
    {
        $this->GameState = $GameState;
        $this->ApplicationSettings = $ApplicationSettings;
        $this->Controls = array();

        $this->setControllers();
    }

    private function setControllers()
    {
        $buttonRight= new Tag("button");
        $buttonRight->setEvent("mousedown","HandleMove",["'right'"]);
        $buttonRight->setEvent("mouseup","HandleMove",["'stay'"]);
        $buttonRight["content"][] = new Text("Move Right");

        $buttonFire= new Tag("button");
        $buttonFire->setEvent("click","HandleFire",[]);
        $buttonFire["content"][] = new Text("Fire");

        $buttonLeft= new Tag("button");
        $buttonLeft->setEvent("mousedown","HandleMove",["'left'"]);
        $buttonLeft->setEvent("mouseup","HandleMove",["'stay'"]);
        $buttonLeft["content"][] = new Text("Move Left");

        $this->Controls[] = $buttonRight;
        $this->Controls[] = $buttonFire;
        $this->Controls[] = $buttonLeft;
    }

    public function WriteHTML() : void 
    {
        $tag = new Tag("div");
        
        foreach($this->GameState["static_entities"] as $entity)
        {
            $tag["content"][] = $entity;
        }

        foreach($this->GameState["movable_entities"] as $entity)
        {
            $tag["content"][] = $entity;
        }

        $tag["content"][] = $this->GameState["rocket"];

        foreach($this->Controls as $control)
        {
            $tag["content"][] = $control;
        }

        $tag->WriteHTML();
    }
}

// ---Handlers section---
function HandleTick() {}
function HandleMove(string $action) {}
function HandleFire() {}

// ---Settings section---
function initialization()
{
    global $app;

    $gameState = [
        "static_entities" => [],
        "movable_entities" => [],
        "rocket" => null,
        "time" => time()
    ];
    $applicationSettings = [
        "sensitivity" => 10,
        "height" => 700
    ];

    $gameState["rocket"] = Rocket::CreateDefault(["x" => 10, "y" => $applicationSettings["height"] - 50]);

    $app = new Application($gameState, $applicationSettings);
}

// ---Executable section---
function render(iHtmlWritable $element) : void
{
    $element->WriteHTML();
}

if (!isset($app))
{
    initialization();
}

render($app);