<?php

interface iHtmlWritable
{
    public function ToHTMLEntity() : string;
}

function CreateDiv(array $style, iHtmlWritable $content = null) : string
{
    $result = "";
    $result = $result . "<div";
    
    if (!empty($style))
    {
        $result = $result . " style=\"";

        foreach ($style as $key => $value)
            $result = $result . "$key:$value;";
    
        $result = $result . "\"";
    }

    $result = $result . ">";

    if ($content != null)
        $result = $result . $content->ToHTMLEntity();

    $result = $result . "</div>";

    return $result;
}

abstract class Entity implements iHtmlWritable
{
    protected array $position;
    protected $style = [];

    public function __construct(array $position) 
    {
        $this->position = $position;
    }

    public function ToHTMLEntity() : string
    {
        $this->style["top"] = $this->position["y"] . "px";
        $this->style["left"] = $this->position["x"] . "px";
        return CreateDiv($this->style);
    }
}

abstract class MovableEntity extends Entity
{
    protected float $x_direction;
    protected float $y_direction;

    public function __construct(array $position, float $x_direction, float $y_direction) 
    {
        parent::__construct($position);
        $this->x_direction = $x_direction;
        $this->y_direction = $y_direction;
    }

    public function Move(float $time) : void
    {
        $this->position["x"] = $this->x_direction * $time + $this->position["x"]; 
        $this->position["y"] = $this->y_direction * $time + $this->position["y"]; 
    }

    public function ChangeDirection(float $x = 0, float $y = 0) : void
    {
        $this->x_direction = $x;
        $this->y_direction = $y;
    }
} 

class Asteroid extends MovableEntity
{ 
    const DEFAULT_SPEED = 1;

    public function __construct(array $position, float $x_direction, float $y_direction)
    {
        parent::__construct($position, $x_direction, $y_direction);
        $this->style = ["background-color" => "black", "height" => "50px", "width" => "50px", "position" => "absolute", "top" => "0px", "left" => "0px"];
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, 0, self::DEFAULT_SPEED);
    }
}

class Bullet extends MovableEntity
{
    const DEFAULT_SPEED = 1;

    function __construct(array $position, float $x_direction, float $y_direction)
    {
        parent::__construct($position, $x_direction, $y_direction);
        $this->style = ["background-color" => "red", "height" => "40px", "width" => "10px", "position" => "absolute", "top" => "0px", "left" => "0px"];
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, 0, self::DEFAULT_SPEED);
    }
}

class Rocket extends MovableEntity
{
    const DEFAULT_SPEED = 0;

    function __construct(array $position, float $x_direction, float $y_direction)
    {
        parent::__construct($position, $x_direction, $y_direction);
        $this->style = ["background-color" => "blue", "height" => "50px", "width" => "50px", "position" => "absolute", "top" => "0px", "left" => "0px"];
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, self::DEFAULT_SPEED, self::DEFAULT_SPEED);
    }
}

class Background extends Entity
{
    public function __construct() 
    {
        parent::__construct(["x" => 0, "y" => 0]);
        $this->style = ["background-color" => "grey", "height" => "800px", "width" => "100%", "position" => "absolute", "top" => "0px", "left" => "0px"];
    }

    public static function CreateDefault()
    {
        return new self();
    }
}

class EntityArray implements iHTMLWritable
{
    protected array $entites;

    public function __construct(array $entites) 
    {
        $this->entites = $entites;
    }

    public function ToHTMLEntity() : string
    {
        $result = "";
        foreach ($this->entites as $entity)
        {
            $result = $result . $entity->ToHTMLEntity();
        }
        
        return $result;
    }
}

//Action handlers
function handle_tick()
{
    global $GameState;

    $time = $GameState["time"] + $GameState["sensitivity"];

    foreach ($GameState["movable_entities"] as $entity)
    {
        $entity->Move($time - $GameState["time"]);
    }

    $GameState["rocket"]->Move($time - $GameState["time"]);
}

function handle_move_right()
{
    global $GameState;
    $GameState["rocket"]->ChangeDirection(1, 0);
}

function handle_move_left()
{
    global $GameState;
    $GameState["rocket"]->ChangeDirection(-1, 0);
}

function handle_stay()
{
    global $GameState;
    $GameState["rocket"]->ChangeDirection(0);
}

function handle_fire()
{
    throw new Exception("NotImplemented");
}

// Render
function renderControls()
{
    echo "<button onmousedown=\"window.eventManager.callEvent('handle_move_right');\" onmouseup=\"window.eventManager.callEvent('handle_stay');\">Move right</button>";

    echo "<button onclick=\"window.eventManager.callEvent('handle_fire');\">Fire</button>";

    echo "<button onmousedown=\"window.eventManager.callEvent('handle_move_left');\" onmouseup=\"window.eventManager.callEvent('handle_stay');\">Move left</button>";
}

function renderBoard()
{
    global $GameState;
    $style = ["height" => $GameState["height"] . "px", "position" => "relative"];
    
    echo CreateDiv($style, new EntityArray(array_merge($GameState["static_entities"], $GameState["movable_entities"], [$GameState["rocket"]])));
}

function render()
{
    renderBoard();
    renderControls();
}

function initGame() : array
{
    $GameState = [
        "time" => 0,
        "sensitivity" => 10,
        "static_entities" => [],
        "movable_entities" => [],
        "rocket" => null,
        // Window properties
        "height" => 800
    ];

    // Init Rocket
    $GameState["rocket"] = Rocket::CreateDefault(["x" => 10, "y" => $GameState["height"] - 50]);
    
    // Init Background
    $GameState["static_entities"][] = Background::CreateDefault();
    
    // Init Asteroids

    // Handlers
    registerEvent("handle_move_right");
    registerEvent("handle_move_left");
    registerEvent("handle_stay");

	createTimer("timer2", $GameState["sensitivity"], "handle_tick");
	startTimer("timer2");

    return $GameState;
}

// Game state
if (!isset($GameState))
{
    $GameState = initGame();
}

render();


