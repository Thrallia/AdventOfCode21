using System;
using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdventOfCode
{
	class Program
	{
		static void Main(string[] args)
		{
			//AoC1(@"..\..\..\..\inputs\day1.txt");
			//AoC2(@"..\..\..\..\inputs\day2.txt");
			//AoC3(@"..\..\..\..\inputs\day3.txt");
			//AoC4(@"..\..\..\..\inputs\day4.txt");
			AoC5(@"..\..\..\..\inputs\day5.txt");
			//AoC6(@"..\..\..\..\inputs\day6.txt");
		}

		private static void AoC5(string path)
		{
			List<string> inputs = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					inputs.Add(line);
				}
			}

			List<VentLines> ventLines = new List<VentLines>();
			foreach(var input in inputs)
			{
				var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				ventLines.Add(new VentLines(parts[0], parts[2]));
			}

			Dictionary<Point, int> grid = new Dictionary<Point, int>();
			for(int i=0; i <= Math.Max(ventLines.Max(x=>x.Start.X), ventLines.Max(x => x.End.X));i++)
			{
				for (int j = 0; j <= Math.Max(ventLines.Max(x => x.Start.Y), ventLines.Max(x => x.End.Y)); j++)
				{
					grid.Add(new Point(i, j), 0);
				}
			}
			var count = ventLines.Count(x => x.Straight());
			foreach(var ventLine in ventLines)
			{
				//if(ventLine.Straight())
				//{
					foreach (var point in ventLine.Points)
						grid[point]++;
				//}
			}

			Console.WriteLine(grid.Count(x=>x.Value>1));
			Console.ReadKey();
		}

		private static void AoC4(string path)
		{
			string rawNums;
			List<BingoCard> cards = new List<BingoCard>();
			using (StreamReader file = new StreamReader(path))
			{
				List<string> cardInput = new List<string>();
				rawNums = file.ReadLine();
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					if (string.IsNullOrWhiteSpace(line))
					{
						if(cardInput.Count>0)
						{
							BingoCard card = new BingoCard();
							card.FillCard(cardInput);
							cards.Add(card);
						}
						cardInput.Clear();
					}
					else
					{
						cardInput.Add(line);
					}
				}
			}
			List<string> numbers = rawNums.Split(',').ToList();
			long final = CallNumbers(numbers, cards);

			Console.WriteLine(final);
			Console.ReadKey();
		}

		private static long CallNumbers(List<string> numbers, List<BingoCard> cards)
		{
			//part 1
			//foreach (var n in numbers)
			//{
			//	int num = int.Parse(n);
			//	cards.ForEach(x => x.SearchCard(num));

			//	foreach (var card in cards)
			//	{
			//		bool win = card.CheckForWin();
			//		if (win)
			//		{
			//			return num * card.GetUnmarkedSum();
			//		}
			//	}
			//}

			//part 2
			int count = 0;
			foreach (var n in numbers)
			{
				int num = int.Parse(n);
				cards.ForEach(x => x.SearchCard(num));

				//foreach (var card in cards)
				//{
				//	bool win = card.CheckForWin();
				//	if (win && cards.Count==1)
				//	{
				//		return card.GetUnmarkedSum() * num;
				//	}
				//}
				if(cards.Count>1)
					cards.RemoveAll(x => x.CheckForWin());
				else
				{
					if(cards.First().CheckForWin())
						return cards.First().GetUnmarkedSum() * num;
				}
			}
			return 0;
		}

		private static void AoC3(string path)
		{
			List<string> diag = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					diag.Add(line);
				}
			}
			int[] counts = new int[diag.First().Length];
			int max = diag.Count();
			foreach (var dig in diag)
			{
				for (int i = 0; i < dig.Length; i++)
				{
					if (dig[i] == '1')
						counts[i]++;
				}
			}

			//part 1
			int[] gamma = new int[counts.Length];
			int[] epsilon = new int[counts.Length];
			for (int i = 0; i < counts.Length; i++)
			{
				if ((float)counts[i] / (float)max > .5)
				{
					gamma[i] = 1;
					epsilon[i] = 0;
				}
				else
				{
					gamma[i] = 0;
					epsilon[i] = 1;
				}
			}
			int x = Functions.BinToInt(gamma);
			int y = Functions.BinToInt(epsilon);

			//part 2
			List<string> oxygen = new List<string>();
			oxygen.AddRange(diag);
			List<string> co2 = new List<string>();
			co2.AddRange(diag);
			int j = 0;
			do
			{
				oxygen = Functions.CheckSupportCriteria(oxygen, j, 'o');
				j++;
			} while (oxygen.Count > 1);
			int oxy = Functions.BinToInt(oxygen.First());

			j = 0;
			do
			{
				co2 = Functions.CheckSupportCriteria(co2, j, 'c');
				j++;
			} while (co2.Count > 1);
			int co = Functions.BinToInt(co2.First());

			Console.WriteLine(x * y);
			Console.WriteLine(oxy * co);
			Console.ReadKey();


		}
		private static void AoC1(string path)
		{
			OrderedList depths = new OrderedList();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					//var node = new LinkedListNode<int>(int.Parse(line));
					depths.AddNext(int.Parse(line));
				}
			}

			//part1
			int lastDepth = (int)depths.First().data;
			int count = 0;
			foreach (Node depth in depths)
			{
				if ((int)depth.data > lastDepth)
					count++;

				lastDepth = (int)depth.data;
			}

			//part2
			int lastSum = (int)depths.First().data + (int)depths.First().next.data + (int)depths.First().next.next.data;
			int countSum = 0;
			foreach (Node depth in depths)
			{
				if (depth.previous != null && depth.next != null)
				{
					int sum = (int)depth.previous.data + (int)depth.data + (int)depth.next.data;
					if (sum > lastSum)
						countSum++;
					lastSum = sum;
				}

			}


			Console.WriteLine("Part 1: " + count);
			Console.WriteLine("Part 2: " + countSum);
			Console.ReadKey();
		}

		private static void AoC2(string path)
		{
			List<string> course = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					course.Add(line);
				}
			}

			//part1
			int position = 0;
			int depth = 0;
			foreach (var leg in course)
			{
				var step = leg.Split(' ');
				switch (step[0])
				{
					case "forward":
						position += int.Parse(step[1]);
						break;
					case "up":
						depth -= int.Parse(step[1]);
						break;
					case "down":
						depth += int.Parse(step[1]);
						break;
				}
			}

			//part2
			int aim = 0;
			int x = 0;
			int y = 0;
			foreach (var leg in course)
			{
				var step = leg.Split(' ');
				switch (step[0])
				{
					case "forward":
						x += int.Parse(step[1]);
						y += int.Parse(step[1]) * aim;
						break;
					case "up":
						aim -= int.Parse(step[1]);
						break;
					case "down":
						aim += int.Parse(step[1]);
						break;
				}
			}

			Console.WriteLine("Part 1: " + (position * depth));
			Console.WriteLine("Part 2: " + (x * y));
			Console.ReadKey();
		}
	}
}
