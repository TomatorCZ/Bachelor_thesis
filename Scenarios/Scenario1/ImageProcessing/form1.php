<h1>Form1</h1>
<p>Data:</p>

<?php
	foreach($_GET as $key => $value)
	{
		echo "<p>$key => $value</p>";
	}
?>