using System;
using System.Collections.Generic;
using System.Threading;
using Scrappy;

string[] Sship = new string[]
{
	#region Frames
	null,
	// Up
	@" _^_ " + "\n" +
	@"/o-o\" + "\n" +
	@"[vvv]" + "\n",
	// Down
	@" _^_ " + "\n" +
	@"/o-o\" + "\n" +
	@"[^^^]" + "\n",
	// Left
	@"  ^_  " + "\n" +
	@" /__\ " + "\n" +
	@" [<<] " + "\n",
	// Right
	@"  _^  " + "\n" +
	@" /__\ " + "\n" +
	@" [>>] " + "\n",
	#endregion
};

string[] SshipShooting = new string[]
{
	#region Frames
	null,
	// Up
	@" |█| " + "\n" +
	@"/o-o\" + "\n" +
	@"[vvv]" + "\n",
	// Down
	@" _^_ " + "\n" +
	@"/o█o\" + "\n" +
	@"[^^^]" + "\n",
	// Left
	@"  __  " + "\n" +
	@" █__\ " + "\n" +
	@" [<<] " + "\n",
	// Right
	@"  __  " + "\n" +
	@" /__█ " + "\n" +
	@" [>>] " + "\n",
	#endregion
};


char[] Bullet = new char[]
{
	#region Frames
	default,
	'█', // Up
	'█', // Down
	'█', // Left
	'█', // Right
	#endregion
};

string Map =
#region Frames
    @"╔═══════════════════════════════════════════════════════════════════════════════════╗" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║     ═════                                                     ═════               ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                    ║                                              ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"║                                                                                   ║" + "\n" +
    @"╚═══════════════════════════════════════════════════════════════════════════════════╝" + "\n";
	#endregion

var Sships = new List<Sship>();
var Player = new Sship() { X = 05, Y = 03, IsPlayer = true };
var health = new Sship() { };


Sships.Add(Player);

Console.CursorVisible = false;
if (OperatingSystem.IsWindows())
{
	Console.WindowWidth = Math.Max(Console.WindowWidth, 105);
	Console.WindowHeight = Math.Max(Console.WindowHeight, 41);
}
Console.BackgroundColor = ConsoleColor.DarkRed;
Console.Clear();
Console.SetCursorPosition(0, 0);
Render(Map);
Console.WriteLine($"Health = {health.Health}");
Console.WriteLine();
Console.WriteLine("Use the (W, A, S, D) keys to move and the arrow keys to shoot.");

#region Render

static void Render(string @string, bool renderSpace = false)
{
	int x = Console.CursorLeft;
	int y = Console.CursorTop;
	foreach (char c in @string)
		if (c is '\n') Console.SetCursorPosition(x, ++y);
		else if (c is not ' ' || renderSpace) Console.Write(c);
		else Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
}

#endregion

while (Sships.Contains(Player)) // && Sships.Count > 0)
{
	#region Sship Updates

	foreach (var sship in Sships)
	{
		#region Shooting Update

		if (sship.IsShooting)
		{
			sship.Bullet = new Bullet()
			{
				X = sship.Direction switch
				{
					Direction.Left => sship.X - 3,
					Direction.Right => sship.X + 3,
					_ => sship.X,
				},
				Y = sship.Direction switch
				{
					Direction.Up => sship.Y - 2,
					Direction.Down => sship.Y + 2,
					_ => sship.Y,
				},
				Direction = sship.Direction,
			};
			sship.IsShooting = false;
			continue;
		}

		#endregion

		#region MoveCheck

		bool MoveCheck(Sship movingSship, int X, int Y)
		{
			//foreach (var sship in Sships)
			//{
			//	if (sship == movingSship)
			//	{
			//		continue;
			//	}
			//	if (Math.Abs(sship.X - X) <= 4 && Math.Abs(sship.Y - Y) <= 2)
			//	{
			//		return false; // collision with another sship
			//	}
			//}
			if (X < 3 || X > 81 || Y < 2 || Y > 31)
			{
				return false; // collision with border walls
			}
			if (3 < X && X < 13 && 11 < Y && Y < 15)
			{
				return false; // collision with left blockade
			}
			if (34 < X && X < 40 && 2 < Y && Y < 8)
			{
				return false; // collision with top blockade
			}
			if (34 < X && X < 40 && 19 < Y && Y < 25)
			{
				return false; // collision with bottom blockade
			}
			if (61 < X && X < 71 && 11 < Y && Y < 15)
			{
				return false; // collision with right blockade
			}
			return true;
		}

		#endregion

		#region Move

		void TryMove(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					if (MoveCheck(sship, sship.X, sship.Y - 1))
					{
						Console.SetCursorPosition(sship.X - 2, sship.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X - 1, sship.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X, sship.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 1, sship.Y + 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 2, sship.Y + 1);
						Console.Write(' ');
						sship.Y--;
					}
					break;
				case Direction.Down:
					if (MoveCheck(sship, sship.X, sship.Y + 1))
					{
						Console.SetCursorPosition(sship.X - 2, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X - 1, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 1, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 2, sship.Y - 1);
						Console.Write(' ');
						sship.Y++;
					}
					break;
				case Direction.Left:
					if (MoveCheck(sship, sship.X - 1, sship.Y))
					{
						Console.SetCursorPosition(sship.X + 2, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 2, sship.Y);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X + 2, sship.Y + 1);
						Console.Write(' ');
						sship.X--;
					}
					break;
				case Direction.Right:
					if (MoveCheck(sship, sship.X + 1, sship.Y))
					{
						Console.SetCursorPosition(sship.X - 2, sship.Y - 1);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X - 2, sship.Y);
						Console.Write(' ');
						Console.SetCursorPosition(sship.X - 2, sship.Y + 1);
						Console.Write(' ');
						sship.X++;
					}
					break;
			}
		}

		#endregion

		if (sship.IsPlayer)
		{
			#region Player Controlled

			Direction? move = null;
			Direction? shoot = null;


			while (Console.KeyAvailable)
			{
				switch (Console.ReadKey(true).Key)
				{
					// Move Direction
					case ConsoleKey.W: move = move.HasValue ? Direction.Null : Direction.Up; break;
					case ConsoleKey.S: move = move.HasValue ? Direction.Null : Direction.Down; break;
					case ConsoleKey.A: move = move.HasValue ? Direction.Null : Direction.Left; break;
					case ConsoleKey.D: move = move.HasValue ? Direction.Null : Direction.Right; break;

					// Shoot Direction
					case ConsoleKey.UpArrow: shoot = shoot.HasValue ? Direction.Null : Direction.Up; break;
					case ConsoleKey.DownArrow: shoot = shoot.HasValue ? Direction.Null : Direction.Down; break;
					case ConsoleKey.LeftArrow: shoot = shoot.HasValue ? Direction.Null : Direction.Left; break;
					case ConsoleKey.RightArrow: shoot = shoot.HasValue ? Direction.Null : Direction.Right; break;

					// Close Game
					case ConsoleKey.Escape:
						Console.Clear();
						Console.WriteLine(
							"---------------------" +
							"| Scrappy was closed.|" +
							"---------------------");
						return;
				}
				while (Console.KeyAvailable)
				{
					Console.ReadKey(true);
				}
			}

			sship.IsShooting = shoot.HasValue && shoot.Value is not Direction.Null && sship.Bullet is null;
			if (sship.IsShooting)
			{
				sship.Direction = shoot ?? sship.Direction;
			}

			if (move.HasValue)
			{
				sship.Direction = move ?? sship.Direction;
				TryMove(move.Value);
			}
			#endregion
		}
		//else
		//{
		//	#region Computer Controled

		//	int randomIndex = random.Next(0, 6);
		//	if (randomIndex < 4)
		//		TryMove((Direction)randomIndex + 1);

		//	if (sship.Bullet is null)
		//	{
		//		Direction shoot = Math.Abs(sship.X - Player.X) > Math.Abs(sship.Y - Player.Y)
		//			? (sship.X < Player.X ? Direction.Right : Direction.Left)
		//			: (sship.Y > Player.Y ? Direction.Up : Direction.Down);
		//		sship.Direction = shoot;
		//		sship.IsShooting = true;
		//	}

			#endregion
		//}

		#region Render Sship

		Console.SetCursorPosition(sship.X - 2, sship.Y - 1);
		Render(sship.IsShooting ? SshipShooting[(int)sship.Direction] : Sship[(int)sship.Direction], true);
		//sship.IsExploding? SshipExploding[sship.ExplodingFrame % 2]
		#endregion
	}

	//#endregion

	#region Bullet Updates

	bool BulletCollisionCheck(Bullet bullet, out Sship collidingSship)
	{
		collidingSship = null;
		//foreach (var sship in Sships)
		//{
		//	if (Math.Abs(bullet.X - sship.X) < 3 && Math.Abs(bullet.Y - sship.Y) < 2)
		//	{
		//		collidingSship = sship;
		//		return true;
		//	}
		//}
		if (bullet.X is 0 || bullet.X is 74 || bullet.Y is 0 || bullet.Y is 27)
		{
			return true;
		}
		if (5 < bullet.X && bullet.X < 11 && bullet.Y == 13)
		{
			return true; // collision with left blockade
		}
		if (bullet.X == 37 && 3 < bullet.Y && bullet.Y < 7)
		{
			return true; // collision with top blockade
		}
		if (bullet.X == 37 && 20 < bullet.Y && bullet.Y < 24)
		{
			return true; // collision with bottom blockade
		}
		if (63 < bullet.X && bullet.X < 69 && bullet.Y == 13)
		{
			return true; // collision with right blockade
		}
		return false;
	}

	foreach (var sship in Sships)
	{
	
		if (sship.Bullet is not null)
		{
			var bullet = sship.Bullet;
			Console.SetCursorPosition(bullet.X, bullet.Y);
			Console.Write(' ');
			switch (bullet.Direction)
			{
				case Direction.Up: bullet.Y--; break;
				case Direction.Down: bullet.Y++; break;
				case Direction.Left: bullet.X--; break;
				case Direction.Right: bullet.X++; break;
			}
			Console.SetCursorPosition(bullet.X, bullet.Y);
			bool collision = BulletCollisionCheck(bullet, out Sship collisionSship);
			Console.Write(collision ? '█' : Bullet[(int)bullet.Direction]);
			if (collision)
			{
				if (collisionSship is not null && --collisionSship.Health <= 0)
				{
					collisionSship.ExplodingFrame = 1;
				}
				sship.Bullet = null;
			}
		}
	}

	#region Removing Dead Sships

	for (int i = 0; i < Sships.Count; i++)
	{
		if (Sships[i].ExplodingFrame > 10)
		{
			Sships.RemoveAt(i);
			i--;
		}
	}

	#endregion

	#endregion

	Console.SetCursorPosition(0, 0);
	Render(Map);
	Thread.Sleep(TimeSpan.FromMilliseconds(50));
}

Console.SetCursorPosition(0, 33);
//Console.Write(Sships.Contains(Player) ? "You Win." : "You Lose.");
Console.ReadLine();

enum Direction
{
	Null = 0,
	Up = 1,
	Down = 2,
	Left = 3,
	Right = 4,
}


class Bullet
{
	public int X;
	public int Y;
	public Direction Direction;
}

