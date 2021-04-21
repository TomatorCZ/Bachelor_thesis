﻿<?php namespace Asteroids1;

#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids1")]
class AsteroidsComponent extends \PhpBlazor\PhpComponent
{	
	private $app;
	private \PhpBlazor\Timer $timer;

	public function BuildRenderTree($builder) : void 
	{
		$this->app->writeWithTreeBuilder($builder, 0);
	}

	public function OnInitialized() : void 
	{
		parent::OnInitialized();

		require(__DIR__ . "/settings.php");
		require(__DIR__ . "\asteroids.php");
		$this->app = new \Asteroids1\Application($settings);	
		
		$this->timer = new \PhpBlazor\Timer($settings["speed"]);
		$this->timer->addEventElapsed(function($s, $e) {$this->ClickHandler($s, $e);});

		$this->timer->Start();
	}

	public function ClickHandler($sender, $e)
	{
		$this->app->tick();
		$this->StateHasChanged();
	}
}