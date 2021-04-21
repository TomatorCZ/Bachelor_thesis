<?php
function HandleKeyDown($e) : void
{
    global $app;
    $app->HandleKeyDown($e);
}

function HandleKeyUp() : void
{
    global $app;
    $app->HandleKeyUp();
}

function HandleMouseDownMoveRight() : void
{
    global $app;
    $app->HandleMouseDownMoveRight();
}

function HandleMouseUp() : void
{
    global $app;
    $app->HandleMouseUp();
}

function HandleMouseDownMoveLeft() : void
{
    global $app;
    $app->HandleMouseDownMoveLeft();
}

function HandleFire() : void
{
	global $app;
	$app->HandleFire();
}

