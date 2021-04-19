<?php namespace something;
#[\Microsoft\AspNetCore\Components\RouteAttribute("simpleComponent")]
class ExampleComponent extends \PhpBlazor\PhpComponent
{	
	public function BuildRenderTree($builder) : void 
	{
		global $msg;
		$builder->AddMarkupContent(0, "<h1>Simple php component</h1>");
		$builder->AddMarkupContent(1, "<p>msg = " . $msg . "</p>");
	}
}