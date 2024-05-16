// Decompiled with JetBrains decompiler
// Type: LacieEngine.Core.Direction
// Assembly: Lacie Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6B8AC25B-99FD-45E1-8F51-579BC4CB3E3A
// Assembly location: D:\GodotPCKExplorer\Paper Lily\exe\.mono\assemblies\Release\Lacie Engine.dll

using Godot;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace LacieEngine.Core
{
  [Serializable]
  public class Direction : ISerializable
  {
	public static readonly Direction None = (Direction) (byte) 0;
	public static readonly Direction Left = (Direction) (byte) 1;
	public static readonly Direction Up = (Direction) (byte) 2;
	public static readonly Direction UpLeft = (Direction) (byte) 3;
	public static readonly Direction Right = (Direction) (byte) 4;
	public static readonly Direction UpRight = (Direction) (byte) 6;
	public static readonly Direction Down = (Direction) (byte) 8;
	public static readonly Direction DownLeft = (Direction) (byte) 9;
	public static readonly Direction DownRight = (Direction) (byte) 12;
	public const byte NONE = 0;
	public const byte LEFT = 1;
	public const byte UP = 2;
	public const byte UP_LEFT = 3;
	public const byte RIGHT = 4;
	public const byte UP_RIGHT = 6;
	public const byte DOWN = 8;
	public const byte DOWN_LEFT = 9;
	public const byte DOWN_RIGHT = 12;
	public const string STR_LEFT = "left";
	public const string STR_UP_LEFT = "upLeft";
	public const string STR_UP = "up";
	public const string STR_UP_RIGHT = "upRight";
	public const string STR_RIGHT = "right";
	public const string STR_DOWN_RIGHT = "downRight";
	public const string STR_DOWN = "down";
	public const string STR_DOWN_LEFT = "downLeft";
	public const string STR_NONE = "none";
	public const string STR_OTHER = "other";
	public static readonly Vector2 VECTOR_LEFT = Vector2.Left;
	public static readonly Vector2 VECTOR_UP_LEFT = (Vector2.Up + Vector2.Left).Normalized();
	public static readonly Vector2 VECTOR_UP = Vector2.Up;
	public static readonly Vector2 VECTOR_UP_RIGHT = (Vector2.Up + Vector2.Right).Normalized();
	public static readonly Vector2 VECTOR_RIGHT = Vector2.Right;
	public static readonly Vector2 VECTOR_DOWN_RIGHT = (Vector2.Down + Vector2.Right).Normalized();
	public static readonly Vector2 VECTOR_DOWN = Vector2.Down;
	public static readonly Vector2 VECTOR_DOWN_LEFT = (Vector2.Down + Vector2.Left).Normalized();
	public static readonly Vector2 VECTOR_NONE = Vector2.Zero;
	private readonly byte _value;

	private Direction(byte value) => this._value = value;

	public static implicit operator byte(Direction val) => val._value;

	public static implicit operator Direction(byte val) => new Direction(val);

	public static implicit operator Direction.Enum(Direction val) => (Direction.Enum) val._value;

	public static implicit operator Direction(Direction.Enum val) => new Direction((byte) val);

	public static bool operator ==(Direction val1, Direction val2) => val1.Equals((object) val2);

	public static bool operator !=(Direction val1, Direction val2) => !val1.Equals((object) val2);

	public static Direction operator +(Direction val1, Direction val2)
	{
	  return Direction.AddDirection(val1, val2);
	}

	public Direction.Enum ToEnum() => (Direction.Enum) this;

	public override string ToString()
	{
	  string str;
	  switch (this._value)
	  {
		case 0:
		  str = "none";
		  break;
		case 1:
		  str = "left";
		  break;
		case 2:
		  str = "up";
		  break;
		case 3:
		  str = "upLeft";
		  break;
		case 4:
		  str = "right";
		  break;
		case 6:
		  str = "upRight";
		  break;
		case 8:
		  str = "down";
		  break;
		case 9:
		  str = "downLeft";
		  break;
		case 12:
		  str = "downRight";
		  break;
		default:
		  str = "other[" + this._value.ToString() + "]";
		  break;
	  }
	  return str;
	}

	public Vector2 ToVector()
	{
	  switch (this._value)
	  {
		case 0:
		  return Vector2.Zero;
		case 1:
		  return Direction.VECTOR_LEFT;
		case 2:
		  return Direction.VECTOR_UP;
		case 3:
		  return Direction.VECTOR_UP_LEFT;
		case 4:
		  return Direction.VECTOR_RIGHT;
		case 6:
		  return Direction.VECTOR_UP_RIGHT;
		case 8:
		  return Direction.VECTOR_DOWN;
		case 9:
		  return Direction.VECTOR_DOWN_LEFT;
		case 12:
		  return Direction.VECTOR_DOWN_RIGHT;
		default:
		  throw new ArgumentException("Provided direction [" + this._value.ToString() + "] cannot be converted to Vector");
	  }
	}

	public Direction FlattenX()
	{
	  Direction direction;
	  switch (this._value)
	  {
		case 1:
		  direction = Direction.Left;
		  break;
		case 3:
		  direction = Direction.Left;
		  break;
		case 4:
		  direction = Direction.Right;
		  break;
		case 6:
		  direction = Direction.Right;
		  break;
		case 9:
		  direction = Direction.Left;
		  break;
		case 12:
		  direction = Direction.Right;
		  break;
		default:
		  direction = Direction.None;
		  break;
	  }
	  return direction;
	}

	public Direction FlattenY()
	{
	  Direction direction;
	  switch (this._value)
	  {
		case 2:
		  direction = Direction.Up;
		  break;
		case 3:
		  direction = Direction.Up;
		  break;
		case 6:
		  direction = Direction.Up;
		  break;
		case 8:
		  direction = Direction.Down;
		  break;
		case 9:
		  direction = Direction.Down;
		  break;
		case 12:
		  direction = Direction.Down;
		  break;
		default:
		  direction = Direction.None;
		  break;
	  }
	  return direction;
	}

	public bool HasX() => this.FlattenX() != Direction.None;

	public bool HasY() => this.FlattenY() != Direction.None;

	public bool HasDirection(Direction direction)
	{
	  int num1 = (int) (byte) this;
	  byte num2 = (byte) direction;
	  int num3 = (int) num2;
	  return (num1 & num3) == (int) num2;
	}

	public static Direction AddDirection(Direction direction, Direction newDirection)
	{
	  return (Direction) (byte) ((uint) direction._value | (uint) newDirection._value);
	}

	public Direction GetOpposite()
	{
	  Direction opposite;
	  switch (this._value)
	  {
		case 1:
		  opposite = Direction.Right;
		  break;
		case 2:
		  opposite = Direction.Down;
		  break;
		case 3:
		  opposite = Direction.DownRight;
		  break;
		case 4:
		  opposite = Direction.Left;
		  break;
		case 6:
		  opposite = Direction.DownLeft;
		  break;
		case 8:
		  opposite = Direction.Up;
		  break;
		case 9:
		  opposite = Direction.UpRight;
		  break;
		case 12:
		  opposite = Direction.UpLeft;
		  break;
		default:
		  opposite = Direction.None;
		  break;
	  }
	  return opposite;
	}

	public static Direction FromVector(Vector2 direction)
	{
	  if ((double) direction.x < 0.0 && (double) direction.y < 0.0)
		return Direction.UpLeft;
	  if ((double) direction.x < 0.0 && (double) direction.y > 0.0)
		return Direction.DownLeft;
	  if ((double) direction.x > 0.0 && (double) direction.y < 0.0)
		return Direction.UpRight;
	  if ((double) direction.x > 0.0 && (double) direction.y > 0.0)
		return Direction.DownRight;
	  if ((double) direction.x < 0.0)
		return Direction.Left;
	  if ((double) direction.x > 0.0)
		return Direction.Right;
	  if ((double) direction.y < 0.0)
		return Direction.Up;
	  return (double) direction.y > 0.0 ? Direction.Down : Direction.None;
	}

	public static Direction FromVectorAnalog(Vector2 direction)
	{
	  if ((double) direction.x < 0.0 && (double) direction.y < 0.0)
		return Decide1of3(direction, Direction.Up, Direction.Left, Direction.UpLeft);
	  if ((double) direction.x < 0.0 && (double) direction.y > 0.0)
		return Decide1of3(direction, Direction.Down, Direction.Left, Direction.DownLeft);
	  if ((double) direction.x > 0.0 && (double) direction.y < 0.0)
		return Decide1of3(direction, Direction.Up, Direction.Right, Direction.UpRight);
	  if ((double) direction.x > 0.0 && (double) direction.y > 0.0)
		return Decide1of3(direction, Direction.Down, Direction.Right, Direction.DownRight);
	  if ((double) direction.x < 0.0)
		return Direction.Left;
	  if ((double) direction.x > 0.0)
		return Direction.Right;
	  if ((double) direction.y < 0.0)
		return Direction.Up;
	  return (double) direction.y > 0.0 ? Direction.Down : Direction.None;

	  static Direction Decide1of3(
		Vector2 direction,
		Direction dOne,
		Direction dTwo,
		Direction dThree)
	  {
		float num1 = direction.Project(dOne.ToVector()).Length();
		float num2 = direction.Project(dTwo.ToVector()).Length();
		float num3 = direction.Project(dThree.ToVector()).Length();
		return (double) num1 > (double) num2 ? ((double) num1 > (double) num3 ? dOne : dThree) : ((double) num2 > (double) num3 ? dTwo : dThree);
	  }
	}

	public static Direction FromString(string direction)
	{
	  Direction direction1;
	  if (direction != null)
	  {
		switch (direction.Length)
		{
		  case 2:
			if (direction == "up")
			{
			  direction1 = Direction.Up;
			  goto label_20;
			}
			else
			  break;
		  case 4:
			switch (direction[0])
			{
			  case 'd':
				if (direction == "down")
				{
				  direction1 = Direction.Down;
				  goto label_20;
				}
				else
				  break;
			  case 'l':
				if (direction == "left")
				{
				  direction1 = Direction.Left;
				  goto label_20;
				}
				else
				  break;
			}
			break;
		  case 5:
			if (direction == "right")
			{
			  direction1 = Direction.Right;
			  goto label_20;
			}
			else
			  break;
		  case 6:
			if (direction == "upLeft")
			{
			  direction1 = Direction.UpLeft;
			  goto label_20;
			}
			else
			  break;
		  case 7:
			if (direction == "upRight")
			{
			  direction1 = Direction.UpRight;
			  goto label_20;
			}
			else
			  break;
		  case 8:
			if (direction == "downLeft")
			{
			  direction1 = Direction.DownLeft;
			  goto label_20;
			}
			else
			  break;
		  case 9:
			if (direction == "downRight")
			{
			  direction1 = Direction.DownRight;
			  goto label_20;
			}
			else
			  break;
		}
	  }
	  direction1 = Direction.None;
label_20:
	  return direction1;
	}

	public static Direction FromByte(byte direction) => (Direction) direction;

	public static Direction FromInput(string direction)
	{
	  Direction direction1;
	  switch (direction)
	  {
		case "input_left":
		  direction1 = Direction.Left;
		  break;
		case "input_up":
		  direction1 = Direction.Up;
		  break;
		case "input_right":
		  direction1 = Direction.Right;
		  break;
		case "input_down":
		  direction1 = Direction.Down;
		  break;
		default:
		  direction1 = Direction.None;
		  break;
	  }
	  return direction1;
	}

	public static Direction Random4D()
	{
	  int num = new Random().Next(4);
	  if (num < 1)
		return Direction.Left;
	  if (num < 2)
		return Direction.Up;
	  return num < 3 ? Direction.Right : Direction.Down;
	}

	public override bool Equals(object obj)
	{
	  Direction direction = obj as Direction;
	  return obj != null && (int) this._value == (int) direction._value;
	}

	public override int GetHashCode() => this._value.GetHashCode();

	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
	  info.AddValue("value", this._value);
	}

	public Direction(SerializationInfo info, StreamingContext context)
	{
	  this._value = info.GetByte("value");
	}

	[Flags]
	public enum Enum : byte
	{
	  None = 0,
	  Left = 1,
	  Up = 2,
	  UpLeft = Up | Left, // 0x03
	  Right = 4,
	  UpRight = Right | Up, // 0x06
	  Down = 8,
	  DownLeft = Down | Left, // 0x09
	  DownRight = Down | Right, // 0x0C
	}
  }
}
