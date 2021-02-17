<?php namespace Asteroids;

require_once(__DIR__ . "\blazorUtilities.php");
// ---Entities---

abstract class Entity extends \BlazorUtilities\Tag
{
    protected array $position;
    protected array $size;

    public function __construct(array $position, array $size) 
    {
        parent::__construct("div");
        $this->position = $position;
        $this->size = $size;
    }

    public function getPosition() : array
    {
        return $this->position;
    }

    public function getSize() : array
    {
        return $this->size;
    }
}

abstract class MovableEntity extends Entity 
{
    protected array $direction;

    public function __construct(array $position, array $direction, array $size) 
    {
        parent::__construct($position, $size);
        $this->direction = $direction;
    }

    public function move(float $time) : void
    {
        $this->position["x"] = $this->direction["x"] * $time + $this->position["x"]; 
        $this->position["y"] = $this->direction["y"] * $time + $this->position["y"]; 
    }

    public function moveWithBounderies(float $time, array $bounderies) : void
    {
        $this->position["x"] = max(min($this->direction["x"] * $time + $this->position["x"], $bounderies["max_x"]),$bounderies["min_x"]); 
        $this->position["y"] = max(min($this->direction["y"] * $time + $this->position["y"], $bounderies["max_y"]),$bounderies["min_y"]); 
    }

    public function changeDirection(array $direction) : void
    {
        $this->direction = $direction;
    }
}

class Background extends Entity 
{
    public function __construct(array $size) 
    {
        parent::__construct(["x" => 0, "y" => 0], $size);

        $this["attributes"]["class"]->add("asteroids-background");

        $this["attributes"]["style"]["height"] = $size["height"] . "px";
        $this["attributes"]["style"]["width"] = $size["width"] . "px";
        $this["attributes"]["style"]["top"] = "0px";
        $this["attributes"]["style"]["left"] = "0px";
    }

    public static function createDefault() : Background
    {
        return new self(["height" => 700, "width" => 700]);
    }
}

class Asteroid extends MovableEntity 
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this["attributes"]["class"]->add("asteroids-asteroid");

        $this["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this["attributes"]["style"]["z-index"] = 1;
        $this["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this["attributes"]["style"]["top"] = $this->position["y"] . "px";
        $this["attributes"]["style"]["left"] = $this->position["x"] . "px";
    }

    public static function createDefault(array $position) : Asteroid
    {
        return new self($position, ["x" => 0, "y" => 1], ["height" => 50, "width" => 50]);
    }

    public function penetration(array $position, array $size) : bool
    {
        $endAsteroidX = $this->position["x"] + $this->size["width"];
        $endAsteroidY = $this->position["y"] + $this->size["height"];
        $endOtherX = $position["x"] + $size["width"];
        $endOtherY = $position["y"] + $size["height"];

        return !($endAsteroidX < $position["x"] || $endOtherX < $this->position["x"]) && !($endAsteroidY < $position["y"] || $endOtherY < $this->position["y"]);
    }
}

class Bullet extends MovableEntity 
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this["attributes"]["class"]->add("asteroids-bullet");

        $this["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this["attributes"]["style"]["z-index"] = 1;
        $this["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this["attributes"]["style"]["top"] = $this->position["y"] . "px";
        $this["attributes"]["style"]["left"] = $this->position["x"] . "px";
    }

    public static function createDefault(array $position)
    {
        return new self($position, ["x" => 0, "y" => -1],  ["height" => 20, "width" => 10]);
    }
}

class Rocket extends MovableEntity
{
    public function __construct(array $position, array $direction, array $size)
    {
        parent::__construct($position, $direction, $size);

        $this["attributes"]["class"]->add("asteroids-rocket");

        $this["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this["attributes"]["style"]["z-index"] = 1;
        $this["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this["attributes"]["style"]["top"] = $this->position["y"] . "px";
        $this["attributes"]["style"]["left"] = $this->position["x"] . "px";
    }

    public static function createDefault(array $position)
    {
        $width = 40;
        $height = 40;

        return new self($position, ["x" => 0, "y" => 0], ["height" => $height, "width" => $width]);
    }
}

class Application extends \BlazorUtilities\Tag 
{
    private $rocket;
    private $bullets;
    private $asteroids;
    private $backgroung;
    private $time;

    private array $gameSettings;

    public function __construct(array $settings) 
    {
        parent::__construct("div");
        $this->gameSettings = $settings;
        
        $this["attributes"]["class"]->add("asteroids-app");
        $this["attributes"]["style"]["height"] = $this->gameSettings["height"] . "px";
        $this["attributes"]["style"]["width"] = $this->gameSettings["width"] . "px";
        $this["attributes"]["tabindex"] = 0;

        $this->initGame();
    }

    public function initGame() : void
    {
        $this->bullets = array();
        $this->asteroids = array();
        $this->background = Background::createDefault();
        $this["content"][] = &$this->background; 

        // 40 is a default height of rocket
        $this->rocket = Rocket::createDefault(["x" => 0, "y" => $this->gameSettings["height"] - 140 ]);
        $this["content"][] = &$this->rocket;

        $this->time = 0;
    }
}
