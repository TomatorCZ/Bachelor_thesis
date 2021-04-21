﻿<?php namespace Asteroids2;

#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids2")]
class AsteroidsComponent extends \PhpBlazor\PhpComponent
{	
	private \PhpBlazor\Timer $timer;

	public function BuildRenderTree($builder) : void 
	{
		global $app;
		$builder->AddMarkupContent(0, $app->ToString($builder,0));
	}

	public function OnInitialized() : void 
	{
		parent::OnInitialized();

		require(__DIR__ . "/settings.php");
		require(__DIR__ . "\asteroids.php");
		require(__DIR__ . "\handlers.php");

		global $app;
		$app = new \Asteroids2\Application($settings);	
		
		$this->timer = new \PhpBlazor\Timer($settings["speed"]);
		$this->timer->addEventElapsed(function($s, $e) {$this->ClickHandler($s, $e);});
		$this->timer->Start();
	}

	public function ClickHandler($sender, $e)
	{
		global $app;
		$app->tick();
		$this->StateHasChanged();
	}
}

