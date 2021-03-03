<h1>Form4</h1>
<p>Data:</p>

<?php
	function printFile($file)
	{
		echo "<p>" . "Field name: " . $file->fieldName . "</p>";
		echo "<p>" . "Name: " . $file->name . "</p>";
		echo "<p>" . "Size: " . $file->size . "</p>";
		echo "<p>" . "Type: " . $file->type . "</p>";
		echo "<p>" . "Id: " . $file->id . "</p>";
	}

	foreach($_FILES as $key => $value)
	{
		echo "<h3>$key</h3>";
		printFile($value);
	}
?>