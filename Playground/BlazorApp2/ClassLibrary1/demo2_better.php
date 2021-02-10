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

    public function GetPosition() : array
    {
        return $this->position;
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

    public function Move(float $time, array $maxposition) : void
    {
        $this->position["x"] = max(min($this->direction["x"] * $time + $this->position["x"], $maxposition["x"]),0); 
        $this->position["y"] = max(min($this->direction["y"] * $time + $this->position["y"], $maxposition["y"]),0); 
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
        return new self($position, ["x" => 0, "y" => self::DEFAULT_SPEED]);
    }
}

class Bullet extends MovableEntity
{
    const DEFAULT_SPEED = -1;

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
        return new self($position, ["x" => 0, "y" => self::DEFAULT_SPEED]);
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

    public static function CreateDefault(array $position) : Rocket
    {
        return new self($position, ["x" => self::DEFAULT_SPEED, "y" => self::DEFAULT_SPEED]);
    }
}

class Background extends Entity
{
    public function __construct(int $height) 
    {
        parent::__construct(["x" => 0, "y" => 0]);
        $this->tag["attributes"]["style"]["background-color"] = "grey";
        $this->tag["attributes"]["style"]["height"] = $height . "px";
        $this->tag["attributes"]["style"]["width"] = "100%";
        $this->tag["attributes"]["style"]["position"] = "absolute";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function CreateDefault()
    {
        return new self(700);
    }
}

class Application implements iHTMLWritable
{
    public array $GameState;
    private array $ApplicationSettings;
    private array $Controls;

    public function __construct(array $GameState, array $ApplicationSettings) 
    {
        $this->GameState = $GameState;
        $this->ApplicationSettings = $ApplicationSettings;
        $this->Controls = array();

        $this->setControllers();
    }

    private function setControllers()
    {
        $buttonRight= new Tag("button");
        $buttonRight["attributes"]["style"] = new CssCollection();
        $buttonRight["attributes"]["style"]["position"] = "absolute";
        $buttonRight["attributes"]["style"]["top"] = "700px";
        $buttonRight["attributes"]["style"]["left"] = "0px";
        $buttonRight->setEvent("mousedown","HandleMove",["'right'"]);
        $buttonRight->setEvent("mouseup","HandleMove",["'stay'"]);
        $buttonRight["content"][] = new Text("Move Right");

        $buttonFire= new Tag("button");
        $buttonFire["attributes"]["style"] = new CssCollection();
        $buttonFire["attributes"]["style"]["position"] = "absolute";
        $buttonFire["attributes"]["style"]["top"] = "700px";
        $buttonFire["attributes"]["style"]["left"] = "100px";
        $buttonFire->setEvent("click","HandleFire",[]);
        $buttonFire["content"][] = new Text("Fire");

        $buttonLeft= new Tag("button");
        $buttonLeft["attributes"]["style"] = new CssCollection();
        $buttonLeft["attributes"]["style"]["position"] = "absolute";
        $buttonLeft["attributes"]["style"]["top"] = "700px";
        $buttonLeft["attributes"]["style"]["left"] = "200px";
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

        $tag["attributes"]["style"] = new CssCollection();
        $tag["attributes"]["style"]["position"] = "relative";
        $tag["attributes"]["style"]["height"] = $this->ApplicationSettings["height"] . "px";
        $tag["attributes"]["style"]["width"] = $this->ApplicationSettings["width"] . "px";
        
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
function HandleTick() 
{
    global $app;
    $currentTime = time(); 
    $delta = 10;//$currentTime - $app->GameState["time"];
    $app->GameState["time"] = $currentTime;

    foreach($app->GameState["movable_entities"] as $entity)
    {
        $entity->Move($delta,  ["x" => 650, "y" => 650]);
    }

    $app->GameState["rocket"]->Move($delta, ["x" => 650, "y" => 650]);

    CallStateHasChanged();
}

function HandleMove(string $action) 
{
    global $app;

    if ($action === "right")
    {
        $app->GameState["rocket"]->ChangeDirection(["x" => 1,"y" => 0]);
    }
    else if ($action === "left")
    {
        $app->GameState["rocket"]->ChangeDirection(["x" => -1,"y" => 0]);
    }
    else if ($action === "stay")
    {
        $app->GameState["rocket"]->ChangeDirection(["x" => 0,"y" => 0]);
    }
}

function HandleFire() 
{
    global $app;

    $app->GameState["movable_entities"][] = Bullet::CreateDefault($app->GameState["rocket"]->GetPosition());
}

// ---Settings section---
function initialization()
{
    global $app;

    $gameState = [
        "static_entities" => [],
        "movable_entities" => [],
        "rocket" => null,
        "time" => time();
    ];
    $applicationSettings = [
        "sensitivity" => 100,
        "height" => 800,
        "width" => 700
    ];

    $gameState["rocket"] = Rocket::CreateDefault(["x" => 10, "y" => $applicationSettings["height"] - 150]);
    $gameState["static_entities"][] = Background::CreateDefault();

    $app = new Application($gameState, $applicationSettings);

    createTimer("timer3", $applicationSettings["sensitivity"], "HandleTick");
	startTimer("timer3");
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