using System;
using System.Text;

namespace UnitTestExample
{
	public class Program
	{
		static void Main(string[] args)
		{
			bool?[] grid = new bool?[Width * Height];
			bool player = true, lined = false;
			int key = 0;

			do
			{
				Console.Clear();
				Console.WriteLine(GridToString(grid));

				if ((key = Console.ReadKey().KeyChar - '1') < 0 || key > 8 || grid[key].HasValue)
					continue;

				grid[key] = player;
				if (lined = IsLined(grid))
					break;
				player ^= true;

			} while (!IsFull(grid));

			Console.Clear();

			Console.WriteLine(lined ? "Player '{0}' won" : "Draw!", player ? "x" : "y");
			Console.ReadLine();
		}

		public static string GridToString(bool?[] grid)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < grid.Length;)
			{
				sb.Append(GridItemToChar(grid[i++], i.ToString()[0]));
				if (i % Width == 0)
					sb.Append('\n');
			}
			return sb.ToString();
		}

		public static string GridItemToChar(bool? item, char index) =>
			!item.HasValue ? $" {index} "
						   : item.Value == true ? " x " : " o ";

		public static bool IsFull(bool?[] grid)
		{
			for (int i = 0; i < grid.Length; i++)
				if (!grid[i].HasValue)
					return false;
			return true;
		}

		public static bool IsLined(bool?[] grid)
		{
			if (grid == null)
				throw new ArgumentNullException(nameof(grid));
			if (Width < 1)
				throw new ApplicationException(nameof(Width));

			for (int i = 0; i < Height; i++)
				if (IsSameForAll(GridSplit(grid, i * Width, Width, k => k)))
					return true;

			for (int i = 0; i < Width; i++)
				if (IsSameForAll(GridSplit(grid, i, Width, k => k * Width)))
					return true;

			return IsSameForAll(GridSplit(grid, 0, Width, k => k * Width + k))
				|| IsSameForAll(GridSplit(grid, Width - 1, Width, k => k * Width - k));
		}

		public static bool?[] GridSplit(bool?[] grid, int startIndex, int count, Func<int, int> offset)
		{
			bool?[] chunk = new bool?[count];
			for (int i = 0; i < count; i++)
				chunk[i] = grid[startIndex + offset(i)];
			return chunk;
		}

		public static bool IsSameForAll(bool?[] line)
		{
			if (!line[0].HasValue)
				return false;
			bool current = line[0].Value;
			for (int i = 1; i < line.Length; i++)
				if (!line[i].HasValue || line[i].Value != current)
					return false;
			return true;
		}

		internal static readonly int Width = 3;
		internal static readonly int Height = 3;
	}
}
