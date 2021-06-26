
How is code structured
===================

Model folder contains non-unity game code,
it is gameplay logic and maths, no rendering.

UnityScripts is mono behavoiours and things that do require unity.

In addition mode contains state and systems.
State classes contain only data and should as a rule be serializable and copiable.
System classes operate on the state and mutate it.
In a way this is MVC - model is model, systems are controllers and unity stuff is view.
We can swap engines preserving M and C

What this means is we have our own maths and physics classes in model, hence the BattleDyzx Vector.

In multiplayer games we need to only replicate the model.
