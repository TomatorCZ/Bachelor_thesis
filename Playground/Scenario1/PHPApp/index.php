<?php namespace PHP;

use \Microsoft\AspNetCore\Components\Rendering;

#[\Microsoft\AspNetCore\Components\RouteAttribute("/php")]
class ScriptComponent extends \PHPBlazorInteropt\PhpComponent
{
	public function BuildRenderTree(RenderTreeBuilder $builder) : void 
	{
		$builder->AddMarkupContent(1, "<p>Hello from PHP!</p>");
	}
}