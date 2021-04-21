<?php

echo "<p>GD2 started at" . date("h:i:sa") . "</p>";
$redimg = imagecreatetruecolor(1920, 1080);
if ($redimg)
	echo "<p>Success</p>";
echo "<p>GD2 ended at" . date("h:i:sa") . "</p>";