<h1>Graph Inspector</h1>
<?php
	foreach($_GET as $key => $value)
	{
		echo "<p>$key => $value</p>";
	}
?>

<?php if ($_GET["action"] === "random") { ?>

<form action="form3" method="get">
	<select name="function">
        <option value="const" selected>Constant</option>
        <option value="linear">Linear</option>
        <option value="expo">Exponential</option>
        <option value="quad">Quadratic</option>
    </select>
    <input name="start" type="number" min="0"/>
    <input name="step" type="number" min="1"/>
    <input name="count" type="number" min="1"/>
    <input type="hidden" name="action" value="look"/>
    <input type="submit" value="Submit"/>
</form>

<?php } elseif ($_GET["action"] === "fileInput") { ?>

<form action="form3" method="get">
	<input type="file" name="file1"/>
    <input type="hidden" name="action" value="look"/>
	<input type="submit" value="Submit"/>
</form>

<?php } elseif ($_GET["action"] === "look") { ?>



<?php } else { ?>

<form id="form1" action="form3" method="get">
    <input name="action" type="radio" value="random"/>
    <input name="action" type="radio" value="fileInput"/>
    <input type="submit" value="Submit"/>
</form>




<?php } ?>