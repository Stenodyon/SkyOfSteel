using Godot;


public static class SwiftSpark
{
	public static float Range = 400;
	public static float HeadshotDamage = 20;
	public static float BodyshotDamage = 15;
	public static float LegshotDamage = 10;
	public static float FireCooldown = 14;


	public static void Fire(Items.Instance Item, Player UsingPlayer)
	{
		Hitscan.QueueFire(0, 0, Range, HeadshotDamage, BodyshotDamage, LegshotDamage);
		Hitscan.ApplyQueuedFire();

		UsingPlayer.SetCooldown(0, FireCooldown, true);

		UsingPlayer.SfxManager.FpThunderboltFire();
	}
}