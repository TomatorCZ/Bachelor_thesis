<?php
function generateGraph($begin, $step, $count, $function)
{
	$graph = array();
    $end = $begin + $step * $count;
    for($begin; $begin <= $end; $begin += $step)
    {
        if ($function == "const") {
            $graph[] = [$begin,1];
        }
        elseif ($function == "linear") {
            $graph[] = [$begin,$begin];
        }
    }

	return $graph;
}

function CsvToString($csv)
{
	
	$result = "";
	foreach($csv as $line)
	{
		if (isset($line[0]) && ($line[1]))
		$result = $result . strval($line[0]) . "," . strval($line[1]) . "\n";
	}
	return $result;
}

if (isset($graph)) {
	if ($_GET["action"] == "download") {
		$file = CreateFile(CsvToString($graph),"text/csv", "graph.csv");
		DownLoadFile($file->id);
	}
	else {
		print_r($graph);
		require("/GraphVisualizer/downloadForm.php");
	}
}
else {
	if ($_GET["action"] === "parametersSetF") {
		function HandleFile($data, $id)
		{
			global $graph;
			$lines = explode("\n", base64_decode($data));
			$graph = array_map('str_getcsv', $lines);
			StateHasChanged();
		}

		GetBrowserFileContent($_FILES["file1"]->id, "HandleFile");
	}
	elseif ($_GET["action"] === "parametersSetG") {
		$start = intval($_GET["start"]);
		$count = intval($_GET["count"]);
		$step = intval($_GET["step"]);
		$graph = generateGraph($start, $step, $count, $_GET["function"]);
		StateHasChanged();
	}
	elseif ($_GET["action"] === "generate") {
		require("/GraphVisualizer/generateForm.php");
	}
	elseif ($_GET["action"] === "inputFile") {
		require("/GraphVisualizer/fileForm.php");
	}
	else {
		require("/GraphVisualizer/chooseForm.php");
	}
}
