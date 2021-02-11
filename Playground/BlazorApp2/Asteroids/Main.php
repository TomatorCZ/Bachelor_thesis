<?php
require_once(__DIR__ . "\HtmlUtilities.php");
require_once(__DIR__ . "\Asteroids.php");

if (!isset($app))
{
    require_once(__DIR__ . "/Settings.php");
    $app = new \Asteroids\Application($settings);
    $app->run();
}

\HtmlUtilities\render($app);

// ---Handlers---
function HandleTick() : void
{
    global $app;
    $app->tick();
    \HtmlUtilities\stateHasChanged();
}

function HandleMove(string $action) : void
{
    global $app;
    $app->move($action);
}

function HandleFire() : void
{
    global $app;
    $app->fire();
}

function HandleReload() : void
{
    global $app;
    $app->shutdown();
    $app->initGame();
    \HtmlUtilities\stateHasChanged();
} 