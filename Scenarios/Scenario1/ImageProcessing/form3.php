<h1>Form3</h1>
<p>Data:</p>

<?php
	function HandleFileData($data, $id)
	{
		global $file;
		$file = $data;
		StateHasChanged();
	}

	if (isset($file))
	{
		foreach($_FILES as $key => $value)
		{
			echo "<p>" . "Field name: " . $value->fieldName . "</p>";
			echo "<p>" . "Name: " . $value->name . "</p>";
			echo "<p>" . "Size: " . $value->size . "</p>";
			echo "<p>" . "Type: " . $value->type . "</p>";
			echo "<p>" . "Id: " . $value->id . "</p>";
			echo "<p>" .$file . "</p>"; 
		}
	}
	else
	{
		GetBrowserFileContent($_FILES["file1"]->id, "HandleFileData");
	}
?>