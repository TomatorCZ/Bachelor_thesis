<?php
    require("../headers/defaultHeader.php");
?>

<?php
if (isset($_GET["developer"])) { 
    $name = $_GET["developer"];
    require("developer$name.php");
} else {
?>
<h1>Developers</h1>
<p>Something about developers...</p>
<p>Get more info about <a href="/community/developers.php?developer=Richard">Richard</a>.</p>
<p>Get more info about <a href="/community/developers.php?developer=Danesh">Danesh</a>.</p>
<?php
}
?>

<?php
    require("../footers/defaultFooter.php");
?>



