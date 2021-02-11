<?php namespace Asteroids;

require_once(__DIR__ . "\HtmlUtilities.php");

// ---Entities---

abstract class Entity implements \HtmlUtilities\iHtmlWritable 
{
    protected array $position;
    protected array $size;
    protected \HtmlUtilities\Tag $tag;

    public function __construct(array $position, array $size) 
    {
        $this->position = $position;
        $this->size = $size;
        $this->tag = new \HtmlUtilities\Tag("div");
    }

    public function writeHTML() : void
    {
        $this->tag["attributes"]["style"]["top"] = $this->position["y"] . "px";
        $this->tag["attributes"]["style"]["left"] = $this->position["x"] . "px";
        $this->tag->writeHTML();
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

        $this->tag["attributes"]["class"]->add("asteroids-background");

        $this->tag["attributes"]["style"]["height"] = $size["height"] . "px";
        $this->tag["attributes"]["style"]["width"] = $size["width"] . "px";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
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

        $this->tag["attributes"]["class"]->add("asteroids-asteroid");

        $this->tag["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this->tag["attributes"]["style"]["z-index"] = 1;
        $this->tag["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
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

        $this->tag["attributes"]["class"]->add("asteroids-bullet");

        $this->tag["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this->tag["attributes"]["style"]["z-index"] = 1;
        $this->tag["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
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

        $this->tag["attributes"]["class"]->add("asteroids-rocket");

        $this->tag["attributes"]["style"]["height"] = $this->size["height"] . "px";
        $this->tag["attributes"]["style"]["z-index"] = 1;
        $this->tag["attributes"]["style"]["width"] = $this->size["width"] . "px";
        $this->tag["attributes"]["style"]["top"] = "0px";
        $this->tag["attributes"]["style"]["left"] = "0px";
    }

    public static function createDefault(array $position)
    {
        $width = 40;
        $height = 40;

        return new self($position, ["x" => 0, "y" => 0], ["height" => $height, "width" => $width]);
    }
}

class Application implements \HtmlUtilities\iHTMLWritable 
{
    private array $gameState;
    private array $gameSettings;
    private bool $running;
    private \HtmlUtilities\Timer $timer;
    private \HtmlUtilities\Tag $tag;

    public function __construct(array $settings) 
    {
        $this->gameSettings = $settings;
        $this->gameState = array();
        $this->running = false;
        $this->timer = \HtmlUtilities\Timer::create($settings["speed"], "HandleTick");
        
        $this->tag = new \HtmlUtilities\Tag("div");
        $this->tag["attributes"]["class"]->add("asteroids-app");
        $this->tag["attributes"]["style"]["height"] = $this->gameSettings["height"] . "px";
        $this->tag["attributes"]["style"]["width"] = $this->gameSettings["width"] . "px";
        $this->tag["attributes"]["tabindex"] = 0;

        $this->addControls();

        $this->initGame();
    }

    private function addControls() : void
    {
        $button= new \HtmlUtilities\Tag("button");
        $button["attributes"]["style"]["position"] = "absolute";
        $button["attributes"]["style"]["top"] = "700px";
        $button["attributes"]["style"]["left"] = "0px";
        $button->setEvent("mousedown","HandleMove",["'right'"]);
        $button->setEvent("mouseup","HandleMove",["'stay'"]);
        $button["content"][] = new \HtmlUtilities\Text("Move Right");

        $this->tag["content"][] = $button;
        unset($button);

        $button= new \HtmlUtilities\Tag("button");
        $button["attributes"]["style"]["position"] = "absolute";
        $button["attributes"]["style"]["top"] = "700px";
        $button["attributes"]["style"]["left"] = "100px";
        $button->setEvent("click","HandleFire",[]);
        $button["content"][] = new \HtmlUtilities\Text("Fire");

        $this->tag["content"][] = $button;
        unset($button);

        $button= new \HtmlUtilities\Tag("button");
        $button["attributes"]["style"]["position"] = "absolute";
        $button["attributes"]["style"]["top"] = "700px";
        $button["attributes"]["style"]["left"] = "200px";
        $button->setEvent("mousedown","HandleMove",["'left'"]);
        $button->setEvent("mouseup","HandleMove",["'stay'"]);
        $button["content"][] = new \HtmlUtilities\Text("Move Left");

        $this->tag["content"][] = $button;
        unset($button);
    }

    public function run() : void 
    {
        if (!$this->running)
        {
            $this->running = true;
            $this->timer->start();
        }
    }

    public function shutdown() : void 
    {
        if ($this->running)
        {
            $this->timer->stop();
            $this->running = false;
        }
    }

    public function initGame() : void
    {
        $this->gameState["bullets"] = array();
        $this->gameState["asteroids"] = array();
        $this->gameState["background"] = Background::createDefault();

        // 40 is a default height of rocket
        $this->gameState["rocket"] = Rocket::createDefault(["x" => 0, "y" => $this->gameSettings["height"] - 140 ]);
        
        $this->gameState["destroyed"] = 0;
        $this->gameState["time"] = 0;
    }

    public function tick() : void 
    {
        // Move
        $newTime = $this->gameState["time"] + $this->gameSettings["sensitivity"]; 
        $delta = $newTime - $this->gameState["time"];
        $this->gameState["time"] = $newTime;

        $max_x = $this->gameSettings["width"] - $this->gameState["rocket"]->getSize()["width"];
        $bounderies = ["max_x" => $max_x, "max_y" => $this->gameSettings["height"] - 150, "min_x"=> 0, "min_y"=> 0];
        
        foreach($this->gameState["asteroids"] as $entity)
        {
            $entity->moveWithBounderies($delta, $bounderies);
        }
        foreach($this->gameState["bullets"] as $entity)
        {
            $entity->move($delta, $bounderies);
        }
        $bounderies["max_y"]+=10;
        $this->gameState["rocket"]->moveWithBounderies($delta,$bounderies);

        // Destroy asteroids and bullets
        $bulletsDestroyed = array();
        $asteroidsDestroyed = array();

        foreach($this->gameState["bullets"] as $keyBullet => $bullet)
        {
            foreach($this->gameState["asteroids"] as $keyAsteroid => $asteroid)
            {
                if ($asteroid->penetration($bullet->getPosition(), $bullet->getSize()))
                {
                    $asteroidsDestroyed[$keyAsteroid] = true;
                    $bulletsDestroyed[$keyBullet]= true;
                    $this->gameState["destroyed"]++;
                    break;
                }
            }

            if ($bullet->getPosition()["y"] <= 0)
            {
                $bulletsDestroyed[$keyBullet]= true;
            }
        }

        foreach($this->gameState["asteroids"] as $keyAsteroid => $asteroid)
        {
            if ($asteroid->getPosition()["y"] >= 650)
            {
                $asteroidsDestroyed[$keyAsteroid] = true;
            }
        }

        $this->gameState["bullets"] = array_diff_key($this->gameState["bullets"], $bulletsDestroyed);
        $this->gameState["asteroids"] = array_diff_key($this->gameState["asteroids"], $asteroidsDestroyed);

        // Create asteroids
        if ($this->gameState["time"] % $this->gameSettings["asteroidFrequency"] === 0)
        {
            $this->gameState["asteroids"][] = Asteroid::createDefault(["x" => rand (0, $this->gameSettings["width"] - 50), "y"=> 0]);
        }
    }

    public function move(string $action) : void 
    {
        if ($action === "right")
        {
            $this->gameState["rocket"]->changeDirection(["x" => 1,"y" => 0]);
        }
        else if ($action === "left")
        {
            $this->gameState["rocket"]->changeDirection(["x" => -1,"y" => 0]);
        }
        else if ($action === "stay")
        {
            $this->gameState["rocket"]->changeDirection(["x" => 0,"y" => 0]);
        }
    }

    public function fire() : void 
    {
        $this->gameState["bullets"][] = Bullet::CreateDefault($this->gameState["rocket"]->GetPosition());
    }

    public function writeHTML() : void 
    {
        unset($this->tag["content"]);
        $this->addControls();

        foreach($this->gameState["bullets"] as $entity)
        {
            $this->tag["content"][] = $entity;
        }

        foreach($this->gameState["asteroids"] as $entity)
        {
            $this->tag["content"][] = $entity;
        }

        $this->tag["content"][] = $this->gameState["rocket"];
        $this->tag["content"][] = $this->gameState["background"];

        $this->tag->writeHTML();
    }
}

