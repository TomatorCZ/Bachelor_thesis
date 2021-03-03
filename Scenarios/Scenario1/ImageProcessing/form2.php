<h1>Form2</h1>
<p>Data:</p>

<?php
	foreach($_POST as $key => $value)
	{
		echo "<p>$key => $value</p>";
	}
?>