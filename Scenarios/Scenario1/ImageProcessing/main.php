<?php
function encodeImage($img)
{
    ob_start();
    imagejpeg($img, NULL, 100);
    $bin = ob_get_clean();
    return base64_encode($bin);
}

function HandleData($data)
{
    global $image, $content, $url;

    $tokens = split(',', $data);
    $content = "image/jpeg";
    $image = imagecreatefromstring(base64_decode($tokens[1]));

    $b64 = encodeImage($image);
    $url = CreateUrl(ToString($b64), ToString($content));

    CallAfterRender(AfterRender);
    StateHasChanged();
}

function HandleOnChange()
{
    $files = GetFilesInfo('input1');
    GetData($files[0]->id, 'HandleData');
}

function HandleOnClick()
{
    global $image, $content, $url;
    imagefilter($image, IMG_FILTER_GRAYSCALE);
    $url = CreateUrl(ToString(encodeImage($image)), ToString($content));
    
    CallAfterRender(AfterRender);
    StateHasChanged();
}

function HandleNew()
{
    unset($GLOBALS[image]);

    StateHasChanged();
}

function HandleSave() 
{
    global $image, $content, $url;

    $b64 = encodeImage($image);

    DownloadData(ToString($b64), ToString($content), "myImg.jpg");
}

function AfterRender()
{
    global $url;
    CallJsVoid('window.php.fileUtils.setUrlTo', "img1", "src",  ToString($url));
}
?>

<h1>Image processing demo</h1>
<?php if (isset($image)) { ?>
<img id="img1" alt="picture" src=""></img>

<button onclick="window.php.interop.callPhpVoid('HandleOnClick');">Black&White</button>
<button onclick="window.php.interop.callPhpVoid('HandleSave');">Save</button>
<button onclick="window.php.interop.callPhpVoid('HandleNew');">New</button>

<?php } else { ?>

<label for="picture">Choose a picture:</label>
<input id="input1" type="file" name="picture" accept="image/jpeg" onchange="window.php.interop.callPhpVoid('HandleOnChange');">

<?php }