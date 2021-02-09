<?php

if (!isset($counter))
	$counter = 0;

function render() 
{
	global $counter;
	echo "<p>$counter</p>";
	echo "<button onclick=\"window.eventManager.callEvent('HandleClick');\">Click me!</button>";
	registerEvent("HandleClick");
}

function HandleClick()
{
	global $counter;

	$counter = $counter + 1;
}

render();