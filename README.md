# SosoEcs
## An easy to use Entity Component System Library

SosoEcs is a simple to use ECS library using the archetype architecture.

> 🚧 **Warning** Proceed at your own risk. This is a _VERY_ in-development library so it will change _a lot_. I'll try to keep the master branch stable, but expect many breaking changes.

## Features

- Straightforward System runners
- Minimal allocations
- Option to run systems in parallel
- Tiny entity struct with helpful functions

## Usage

```cs
struct Transform
{
	public Vector2 Position;
}
struct RigidBody
{
	public Vector2 Velocity;
}
struct Physics : ISystem<Transform, RigidBody>
{
	public void Update(ref Transform t0, ref RigidBody t1)
	{
		t0.Position += t1.Velocity * Time.Dt;
		if (t0.Position.X < 0 || t0.Position.X > Window.Width) t1.Velocity.X *= -1;
		if (t0.Position.Y < 0 || t0.Position.Y > Window.Height) t1.Velocity.Y *= -1;
	}
}

World Ecs = new World();
Ecs.RunParallel<Physics, Transform, RigidBody>();
```

## Dependancies

EzNet has no external dependancies
It targets netstandard2.1, net6.0, and net7.0. It uses latest C#
