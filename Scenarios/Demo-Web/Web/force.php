<?php
class force
{
}

namespace something;
#[\Microsoft\AspNetCore\Components\RouteAttribute("component")]
class ExampleComponent extends \PhpBlazor\PhpComponent
{	
	public function BuildRenderTree($builder) : void 
	{
		$builder->AddContent(0, "<h1>component</h1>");
	}
}