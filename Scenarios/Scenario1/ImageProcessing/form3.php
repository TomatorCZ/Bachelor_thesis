<h1>Graph Inspector</h1>
<?php if ($_GET["action"] === "random") { ?>

<form action="form3" method="get">
	<select name="function">
        <option value="const" selected>Constant</option>
        <option value="linear">Linear</option>
    </select>
    <input name="start" type="number" min="0"/>
    <input name="step" type="number" min="1"/>
    <input name="count" type="number" min="1"/>
    <input type="hidden" name="action" value="look"/>
    <input type="hidden" name="from" value="generation"/>
    <input type="submit" value="Submit"/>
</form>

<?php } elseif ($_GET["action"] === "fileInput") { ?>

<form action="form3" method="get">
	<input type="file" name="file1"/>
    <input type="hidden" name="action" value="look"/>
    <input type="hidden" name="from" value="file"/>
	<input type="submit" value="Submit"/>
</form>
<?php } elseif ($_GET["action"] === "look") { ?>
<?php
    function DrawGraph($graph)
    {
        print_r($graph);
    }
?>


<?php
if (isset($graph))
{
    DrawGraph($graph);
} 
elseif ($_GET["from"] == "generation") {
    $start = intval($_GET["start"]);
    $count = intval($_GET["count"]);
    $step = intval($_GET["step"]);

    $graph = array();
    $end = $start + $step * $count;
    for($start; $start <= $end; $start += $step)
    {
        if ($_GET["function"] == "const") {
            $graph[$start] = 1;
        }
        elseif ($_GET["function"] == "linear") {
            $graph[$start] = $start;
        }
    }
    DrawGraph($graph);
} 
elseif ($_GET["from"] == "file") {
    function HandleFile($data, $id)
    {
        global $graph;
        

        $lines = explode("\n", base64_decode($data));
        $graph = array_map('str_getcsv', $lines);
        \System\Console::Writeline("done");
        StateHasChanged();
    }

    GetBrowserFileContent($_FILES["file1"]->id, "HandleFile");
}

?>



<?php } else { ?>

<form id="form1" action="form3" method="get">
    <input name="action" type="radio" value="random"/>
    <input name="action" type="radio" value="fileInput"/>
    <input type="submit" value="Submit"/>
</form>

<?php } ?>