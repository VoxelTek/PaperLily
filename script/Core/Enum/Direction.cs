using System;
using System.Runtime.Serialization;
using Godot;

namespace LacieEngine.Core
{
	[Serializable]
	public class Direction : ISerializable
	{
		[Flags]
		public enum Enum : byte
		{
			None = 0,
			Left = 1,
			Up = 2,
			UpLeft = 3,
			Right = 4,
			UpRight = 6,
			Down = 8,
			DownLeft = 9,
			DownRight = 0xC
		}

		public static readonly Direction None = (byte)0;

		public static readonly Direction Left = (byte)1;

		public static readonly Direction Up = (byte)2;

		public static readonly Direction UpLeft = (byte)3;

		public static readonly Direction Right = (byte)4;

		public static readonly Direction UpRight = (byte)6;

		public static readonly Direction Down = (byte)8;

		public static readonly Direction DownLeft = (byte)9;

		public static readonly Direction DownRight = (byte)12;

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

		private Direction(byte value)
		{
			_value = value;
		}

		public static implicit operator byte(Direction val)
		{
			return val._value;
		}

		public static implicit operator Direction(byte val)
		{
			return new Direction(val);
		}

		public static implicit operator Enum(Direction val)
		{
			return (Enum)val._value;
		}

		public static implicit operator Direction(Enum val)
		{
			return new Direction((byte)val);
		}

		public static bool operator ==(Direction val1, Direction val2)
		{
			return val1.Equals(val2);
		}

		public static bool operator !=(Direction val1, Direction val2)
		{
			return !val1.Equals(val2);
		}

		public static Direction operator +(Direction val1, Direction val2)
		{
			return AddDirection(val1, val2);
		}

		public Enum ToEnum()
		{
			return this;
		}

		public override string ToString()
		{
			return _value switch
			{
				1 => "left", 
				3 => "upLeft", 
				2 => "up", 
				6 => "upRight", 
				4 => "right", 
				12 => "downRight", 
				8 => "down", 
				9 => "downLeft", 
				0 => "none", 
				_ => "other[" + _value + "]", 
			};
		}

		public Vector2 ToVector()
		{
			return _value switch
			{
				1 => VECTOR_LEFT, 
				3 => VECTOR_UP_LEFT, 
				2 => VECTOR_UP, 
				6 => VECTOR_UP_RIGHT, 
				4 => VECTOR_RIGHT, 
				12 => VECTOR_DOWN_RIGHT, 
				8 => VECTOR_DOWN, 
				9 => VECTOR_DOWN_LEFT, 
				0 => Vector2.Zero, 
				_ => throw new ArgumentException("Provided direction [" + _value + "] cannot be converted to Vector"), 
			};
		}

		public Direction FlattenX()
		{
			return _value switch
			{
				1 => Left, 
				3 => Left, 
				6 => Right, 
				4 => Right, 
				12 => Right, 
				9 => Left, 
				_ => None, 
			};
		}

		public Direction FlattenY()
		{
			return _value switch
			{
				3 => Up, 
				2 => Up, 
				6 => Up, 
				12 => Down, 
				8 => Down, 
				9 => Down, 
				_ => None, 
			};
		}

		public bool HasX()
		{
			return FlattenX() != None;
		}

		public bool HasY()
		{
			return FlattenY() != None;
		}

		public bool HasDirection(Direction direction)
		{
			byte num = this;
			byte otherDir = direction;
			return (num & otherDir) == otherDir;
		}

		public static Direction AddDirection(Direction direction, Direction newDirection)
		{
			return (byte)(direction._value | newDirection._value);
		}

		public Direction GetOpposite()
		{
			return _value switch
			{
				1 => Right, 
				3 => DownRight, 
				2 => Down, 
				6 => DownLeft, 
				4 => Left, 
				12 => UpLeft, 
				8 => Up, 
				9 => UpRight, 
				_ => None, 
			};
		}

		public static Direction FromVector(Vector2 direction)
		{
			if (direction.x < 0f && direction.y < 0f)
			{
				return UpLeft;
			}
			if (direction.x < 0f && direction.y > 0f)
			{
				return DownLeft;
			}
			if (direction.x > 0f && direction.y < 0f)
			{
				return UpRight;
			}
			if (direction.x > 0f && direction.y > 0f)
			{
				return DownRight;
			}
			if (direction.x < 0f)
			{
				return Left;
			}
			if (direction.x > 0f)
			{
				return Right;
			}
			if (direction.y < 0f)
			{
				return Up;
			}
			if (direction.y > 0f)
			{
				return Down;
			}
			return None;
		}

		public static Direction FromVectorAnalog(Vector2 direction)
		{
			if (direction.x < 0f && direction.y < 0f)
			{
				return Decide1of3(direction, Up, Left, UpLeft);
			}
			if (direction.x < 0f && direction.y > 0f)
			{
				return Decide1of3(direction, Down, Left, DownLeft);
			}
			if (direction.x > 0f && direction.y < 0f)
			{
				return Decide1of3(direction, Up, Right, UpRight);
			}
			if (direction.x > 0f && direction.y > 0f)
			{
				return Decide1of3(direction, Down, Right, DownRight);
			}
			if (direction.x < 0f)
			{
				return Left;
			}
			if (direction.x > 0f)
			{
				return Right;
			}
			if (direction.y < 0f)
			{
				return Up;
			}
			if (direction.y > 0f)
			{
				return Down;
			}
			return None;
			static Direction Decide1of3(Vector2 direction, Direction dOne, Direction dTwo, Direction dThree)
			{
				float one = direction.Project(dOne.ToVector()).Length();
				float two = direction.Project(dTwo.ToVector()).Length();
				float three = direction.Project(dThree.ToVector()).Length();
				if (one > two)
				{
					if (one > three)
					{
						return dOne;
					}
					return dThree;
				}
				if (two > three)
				{
					return dTwo;
				}
				return dThree;
			}
		}

		public static Direction FromString(string direction)
		{
			if (direction != null)
			{
				switch (direction.Length)
				{
				case 4:
					switch (direction[0])
					{
					case 'l':
						if (!(direction == "left"))
						{
							break;
						}
						return Left;
					case 'd':
						if (!(direction == "down"))
						{
							break;
						}
						return Down;
					}
					break;
				case 6:
					if (!(direction == "upLeft"))
					{
						break;
					}
					return UpLeft;
				case 2:
					if (!(direction == "up"))
					{
						break;
					}
					return Up;
				case 7:
					if (!(direction == "upRight"))
					{
						break;
					}
					return UpRight;
				case 5:
					if (!(direction == "right"))
					{
						break;
					}
					return Right;
				case 9:
					if (!(direction == "downRight"))
					{
						break;
					}
					return DownRight;
				case 8:
					if (!(direction == "downLeft"))
					{
						break;
					}
					return DownLeft;
				}
			}
			return None;
		}

		public static Direction FromByte(byte direction)
		{
			return direction;
		}

		public static Direction FromInput(string direction)
		{
			return direction switch
			{
				"input_left" => Left, 
				"input_up" => Up, 
				"input_right" => Right, 
				"input_down" => Down, 
				_ => None, 
			};
		}

		public static Direction Random4D()
		{
			int val = new Random().Next(4);
			if (val < 1)
			{
				return Left;
			}
			if (val < 2)
			{
				return Up;
			}
			if (val < 3)
			{
				return Right;
			}
			return Down;
		}

		public override bool Equals(object obj)
		{
			Direction direction = obj as Direction;
			if (obj != null)
			{
				return _value == direction._value;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _value.GetHashCode();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("value", _value);
		}

		public Direction(SerializationInfo info, StreamingContext context)
		{
			_value = info.GetByte("value");
		}
	}
}
