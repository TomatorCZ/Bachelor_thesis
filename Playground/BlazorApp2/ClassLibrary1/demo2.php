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
        $this->style["top"] = $this->position["x"] . "px";
        $this->style["right"] = $this->position["y"] . "px";
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
        $this->position["x"] = $this->x_direction * $time + $this->$position["x"]; 
        $this->position["y"] = $this->y_direction * $time + $this->$position["y"]; 
    }
} 

class Asteroid extends MovableEntity
{ 
    const DEFAULT_SPEED = 1;

    public function __construct(array $position, float $x_direction, float $y_direction)
    {
        parent::__construct($position, $x_direction, $y_direction);
        $this->$style = ["background-color" => "black", "height" => "50px", "width" => "50px", "position" => "absolute", "top" => "0px", "right" => "0px"];
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
        $this->$style = ["background-color" => "red", "height" => "40px", "width" => "10px", "position" => "absolute", "top" => "0px", "right" => "0px"];
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
        $this->style = ["background-color" => "blue", "height" => "50px", "width" => "50px", "position" => "absolute", "top" => "0px", "right" => "0px"];
    }

    public static function CreateDefault(array $position)
    {
        return new self($position, self::DEFAULT_SPEED, self::DEFAULT_SPEED);
    }
}

$GameState = [
    "time" => 0,
    "entities" => []
];

$GameState["entities"][] = Rocket::CreateDefault(["x" => 200, "y" => 200]);

function renderEntities()
{
    global $GameState;

    foreach ($GameState["entities"] as $entity)
    {
        echo $entity->ToHTMLEntity();
    }
}

function render()
{
    renderEntities();
}

render();