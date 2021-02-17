﻿<?php namespace Asteroids;

require_once(__DIR__ . "\asteroids.php");

//TODO: It will be nice to not use whole names witch namespaces...

#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids")]
class AsteroidsComponent extends \Microsoft\AspNetCore\Components\ComponentBase
{	
	private $app;

	public function BuildRenderTree($builder) : void 
	{
		$this->app->writeWithTreeBuilder($builder, 0);
	}

	public function OnInitialized() : void 
	{
		require(__DIR__ . "/settings.php"); // once !!!
		$this->app = new Application($settings);		
	}

	public function ClickHandler($e)
	{
		\System\Console::WriteLine("Click");
	}
}