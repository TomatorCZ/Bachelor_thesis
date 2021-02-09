<?php

function HandleClick()
{
	global $counter;

	$counter = $counter + 1;
}


if (!isset($counter))
{
	$counter = 0;
	createTimer("timer1", 1000, "HandleClick");
	startTimer("timer1");
}

echo "<p>$counter</p>";
