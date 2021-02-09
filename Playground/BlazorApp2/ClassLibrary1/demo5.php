<?php

if (!isset($fooIndicator))
{
	echo "Foo was not called";
	echo "<button onclick=\"window.eventManager.callEvent('foo', 'Ahoj');\">Call it!</button>";
}
else
{
	echo "Foo was called";
	echo $fooIndicator;
}

function foo($msg)
{
	global $fooIndicator;
	$fooIndicator = $msg;
	CallStateHasChanged();
}

