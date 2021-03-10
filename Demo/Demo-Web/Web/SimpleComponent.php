<?php namespace something;
#[\Microsoft\AspNetCore\Components\RouteAttribute("simpleComponent")]
class ExampleComponent extends \PhpBlazor\PhpComponent
{	
	public function BuildRenderTree($builder) : void 
	{
		$builder->AddMarkupContent(0, "<h1>Simple php component</h1>");
	}
}