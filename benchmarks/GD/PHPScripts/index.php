<?php

$started = microtime(true);
$redimg = imagecreatetruecolor(1920, 1080);
$end = microtime(true);
$diff = $end - $started;

echo "<p>\n";

echo "The function <b>imagecreatetruecolor(1920, 1080);</b>\n";

echo "Started: $started\n";

echo "Ended: $end\n";

echo "State: ";
if ($redimg)
	echo "Success\n";
else 
	echo "Failed\n";

echo "Total elapsed time: $diff\n";

echo "</p>";
