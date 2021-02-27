<?php
function encodeImage($img)
{
    ob_start();
    imagepng($img);
    $bin = ob_get_clean();
    return base64_encode($bin);
}

function HandleData($data)
{
    global $image, $content, $url;

    $tokens = split(',', $data);
    $content = $tokens[0];
    $image = imagecreatefromstring(base64_decode($tokens[1]));
    if (!$image)
    {
        echo "Image loading failed\n";
    }

    $b64 = encodeImage($image);

    $url = \PhpBlazor\GenericHelper::CallJs<string>('window.php.fileUtils.createUrlObject',  ToString($b64), ToString($tokens[0]));

    CallAfterRender(AfterRender);
    StateHasChanged();
}

function HandleOnChange()
{
    $files = \PhpBlazor\GenericHelper::CallJsArray<\PhpBlazor\BrowserFile>('window.php.fileUtils.getFilesInfo', 'input1');

    CallJsCustomVoid('window.php.fileUtils.getData', $files[0]->id, 'HandleData');
}

function HandleOnClick()
{
    global $image, $content, $url;
    imagefilter($image, IMG_FILTER_GRAYSCALE);
    $url = \PhpBlazor\GenericHelper::CallJs<string>('window.php.fileUtils.createUrlObject',  ToString(encodeImage($image)), ToString($content));
    
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

    CallJsVoid('window.php.fileUtils.downloadData',  ToString($b64), ToString($content));
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
<input id="input1" type="file" id="picture" name="picture" accept="image/png, image/jpeg" onchange="window.php.interop.callPhpVoid('HandleOnChange');">

<?php }