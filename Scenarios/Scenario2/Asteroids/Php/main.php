﻿<?php namespace Asteroids;

require_once(__DIR__ . "\asteroids.php");

//TODO: It will be nice to not use whole names witch namespaces...

#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids")]
class AsteroidsComponent extends \Microsoft\AspNetCore\Components\ComponentBase
{	
	private $app;

	public function BuildRenderTree($builder) : void 
	{
		$builder->OpenElement(0,"button");
		//\Microsoft\AspNetCore\Components\EventCallback::Factory->Create($this, function() {});
		//$this->AddAttribute(1,"onclick", \Microsoft\AspNetCore\Components\EventCallback::Factory::Create($this, ClickHander));
		$builder->AddContent(2,"Click");
		$builder->CloseElement();

		//$this->app->writeWithTreeBuilder($builder, 0);
	}

	public function OnInitialized() : void 
	{
		$foo = \Microsoft\AspNetCore\Components\EventCallback;
		/*$class = new \ReflectionClass($foo);


		$methods = $class->getMethods();
		foreach($methods as $method)
		{
			\System\Console::WriteLine($method->getName());
		}
		*/
		$reflect = new \ReflectionClass($foo);
		$props   = $reflect->getProperties(\ReflectionProperty::IS_PUBLIC);

		foreach ($props as $prop) {
			\System\Console::WriteLine($prop->getName());
		}


		require(__DIR__ . "/settings.php"); // once !!!
		$this->app = new Application($settings);		
	}

	public function ClickHandler($e)
	{
		\System\Console::WriteLine("Click");
	}
}