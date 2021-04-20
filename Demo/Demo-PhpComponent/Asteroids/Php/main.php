﻿<?php namespace Asteroids;

require_once(__DIR__ . "\asteroids.php");

#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids")]
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
		$this->app = new Application($settings);	
		
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