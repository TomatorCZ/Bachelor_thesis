<?php
namespace foo;
class Foo implements \ArrayAccess
{
	private array $ar;
	
	public function __construct()
	{
		$this->ar = array();
	}

	public function offsetExists ($offset ) 
	{
		return $offset == "ar";
	}

	public function offsetGet ($offset )
	{
		return $this->ar;
	}

	public function offsetSet ($offset , $value )
	{
		$this->ar = $value;
	}

	public function offsetUnset ($offset )
	{
		unset($this->ar);
	}

	public function write()
	{
		foreach($this->ar as $a)
		{
			echo $a;
		}
	}
}

echo "test1";

$b = new Foo();
$b["ar"][] = "Neco";
$b->write();

echo "test2";
$c = new \ClassLibrary2\Class2("NecoDalsi");
$c["ar"][] = "Neco";
$c->write();
