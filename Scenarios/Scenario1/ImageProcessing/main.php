<?php

function HandleImg(string $stringImage) : void 
{ 
    global $image;
    $base_to_php = explode(',', $stringImage);
    $data = base64_decode($base_to_php[1]);
    $image = imagecreatefromstring($data);
   
    global $url;
    Foo<string>("Ahoj");
    //$url = CallJS<string>("createUrl", base64_encode($image));

    StateHasChanged();
}

function HandleBlackAndWhite() : void
{
    global $image;
    imagefilter($image, IMG_FILTER_GRAYSCALE);
    imagefilter($image, IMG_FILTER_CONTRAST, -100);
    StateHasChanged();
}

function HandleNew() : void
{
    global $image;
    unset($image);
    StateHasChanged();
}

?>

<h1>Image processing demo</h1>
<?php
    if(isset($image)) {
?>

<img alt="The image" src="<?php echo $url;?>"/>
<button onclick="window.php.callCallback('HandleBlackAndWhite');">Black&White</button>
<button onclick="window.myUtils.saveImg();">SaveAs</button>
<button onclick="window.php.callCallback('HandleNew');">New</button>

<?php } else { ?>

<label for="picture">Choose a picture:</label>
<input type="file" id="picture" name="picture" accept="image/png, image/jpeg" onchange="window.myUtils.loadImg(this);">
<button onclick="window.myUtils.processImg('HandleImg');">GO</button>

<?php } ?>

