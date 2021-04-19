<?php namespace something;
use \Microsoft\AspNetCore\Components as Microsoft;

#[Microsoft\RouteAttribute("simpleComponent")]
class ExampleComponent extends \PhpBlazor\PhpComponent
{	
	public function BuildRenderTree($builder) : void 
	{
		global $msg;
		$builder->AddMarkupContent(0, "<h1>Simple component</h1>");
		$builder->AddMarkupContent(1, "<p>msg = " . $msg . "</p>");
	}
}