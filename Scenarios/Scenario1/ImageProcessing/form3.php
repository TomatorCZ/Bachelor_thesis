<h1>Form3</h1>
<p>Data:</p>

<?php
	foreach($_FILES as $key => $value)
	{
		print_r($value);
	}
?>