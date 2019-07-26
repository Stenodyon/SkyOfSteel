using Godot;
using static Godot.Mathf;
using static System.Diagnostics.Debug;


public class Hitscan : Spatial
{
	public static bool DrawHits = false;

	public static int NextRecoilDirection; //1 for right, -1 for left


	public static Hitscan Self;

	Hitscan()
	{
		if(Engine.EditorHint) {return;}

		Self = this;
		Reset();
	}


	public static void Reset()
	{
		NextRecoilDirection = 1;
	}


	public static void Fire(float VerticalAngle, float HorizontalAngle, float Range, float HDmg, float BDmg, float LDmg)
	{
		Assert(NextRecoilDirection == 1 || NextRecoilDirection == -1);
		Player Plr = Game.PossessedPlayer;

		{
			PhysicsDirectSpaceState State = Self.GetWorld().DirectSpaceState;

			Vector3 Origin = Plr.Cam.GlobalTransform.origin;
			Vector3 Endpoint = Origin + new Vector3(0, 0, Range)
				.Rotated(new Vector3(1, 0, 0), Deg2Rad(-Plr.LookVertical - VerticalAngle))
				.Rotated(new Vector3(0, 1, 0), Deg2Rad(Plr.LookHorizontal + HorizontalAngle));

			Godot.Collections.Dictionary Results = State.IntersectRay(Origin, Endpoint, null, 2);
			if(Results.Count > 0) //We hit something
			{
				if(DrawHits)
					World.DebugPlot((Vector3)Results["position"]);

				if(Results["collider"] is HitboxClass Hitbox)
				{
					Player HitPlr = Hitbox.OwningPlayer;
					switch(Hitbox.Type)
					{
						case HitboxClass.TYPE.HEAD:
							HitPlr.RpcId(HitPlr.Id, nameof(Player.ApplyDamage), HDmg);
							break;
						case HitboxClass.TYPE.BODY:
							HitPlr.RpcId(HitPlr.Id, nameof(Player.ApplyDamage), BDmg);
							break;
						case HitboxClass.TYPE.LEGS:
							HitPlr.RpcId(HitPlr.Id, nameof(Player.ApplyDamage), LDmg);
							break;
					}
				}
			}
		}
	}


	public static void ApplyRecoil(float VerticalRecoil, float HorizontalRecoil)
	{
		Player Plr = Game.PossessedPlayer;
		Plr.ApplyLookVertical(VerticalRecoil);
		Plr.LookHorizontal -= HorizontalRecoil*NextRecoilDirection;
		Plr.SetRotationDegrees(new Vector3(0, Plr.LookHorizontal, 0));

		NextRecoilDirection *= -1;
	}
}
