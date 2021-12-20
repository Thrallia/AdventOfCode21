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
			//AoC5(@"..\..\..\..\inputs\day5.txt");
			//AoC6(@"..\..\..\..\inputs\day6.txt");
			//AoC7(@"..\..\..\..\inputs\day7.txt");
			//AoC8(@"..\..\..\..\inputs\day8.txt");
			AoC9(@"..\..\..\..\inputs\day9.txt");

		}

		private static void AoC9(string path)
		{
			List<string> wholeMap = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					wholeMap.Add(line);
				}
			}

			int[,] map = new int[wholeMap.First().Length, wholeMap.Count];

			int i = 0;
			int j = 0;
			foreach (var line in wholeMap)
			{
				foreach (var spot in line)
				{
					map[i, j] = int.Parse(spot.ToString());
					j++;
				}
				i++;
				j = 0;
			}

			List<int> lowPoints = new List<int>();
			List<(int x, int y)> lowCoords = new List<(int x, int y)>();
			List<(int, int)> dex = new List<(int, int)> { (1, 0), (-1, 0), (0, 1), (0, -1) };
			int maxX = map.GetLength(0);
			int maxY = map.GetLength(1);

			for (int x = 0; x < maxX; x++)
			{
				for (int y = 0; y < maxY; y++)
				{
					List<int> proxy = new List<int>();
					int point = map[x, y];
					foreach (var (p1, p2) in dex)
					{
						try
						{
							proxy.Add(map[x + p1, y + p2]);

						}
						catch (IndexOutOfRangeException)
						{ }
					}
					if (point < proxy.Min())
					{
						lowPoints.Add(point);
						lowCoords.Add((x, y));
					}
				}
			}

			List<int> basinSize = new List<int>();
			foreach (var lowCoord in lowCoords)
			{
				List<(int, int)> mapped = new List<(int, int)>();
				List<(int, int)> neighbors = new List<(int, int)>();

				neighbors.AddRange(Functions.GetValidNeighbors(map, lowCoord, dex, mapped));
				for (int k = 0; k < neighbors.Count; k++)
				{
					if (!mapped.Contains(neighbors[k]))
						mapped.Add(neighbors[k]);

					neighbors.AddRange(Functions.GetValidNeighbors(map, neighbors[k], dex, mapped));
				}
				basinSize.Add(mapped.Count());
			}

			basinSize.Sort();
			basinSize.Reverse();

			Console.WriteLine(lowPoints.Sum() + lowPoints.Count());
			Console.WriteLine(basinSize[0] * basinSize[1] * basinSize[2]);
			Console.ReadKey();
		}


		private static void AoC8(string path)
		{
			List<string> displayTest = new List<string>();
			List<string> outputs = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine().Split('|');
					displayTest.Add(line[0]);
					outputs.Add(line[1]);
				}
			}

			//part 1
			List<string> separatedOutputs = new List<string>();
			outputs.ForEach(x => separatedOutputs.AddRange(x.Split(' ', StringSplitOptions.RemoveEmptyEntries)));

			int count = 0;

			count += separatedOutputs.Count(x => x.Length == 2);
			count += separatedOutputs.Count(x => x.Length == 4);
			count += separatedOutputs.Count(x => x.Length == 3);
			count += separatedOutputs.Count(x => x.Length == 7);

			//part 2
			int iter = 0;
			List<string> results = new List<string>();
			foreach (var dis in displayTest)
			{
				var digits = dis.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string one = digits.Where(x => x.Length == 2).First();
				string seven = digits.Where(x => x.Length == 3).First();
				string four = digits.Where(x => x.Length == 4).First();
				string eight = digits.Where(x => x.Length == 7).First();
				string zero = string.Empty;
				string six = string.Empty;
				string nine = string.Empty;
				string two = string.Empty;
				string three = string.Empty;
				string five = string.Empty;

				var a = new string(seven.Except(one).ToArray());
				var bd = new string(four.Except(seven).ToArray());
				string d = string.Empty;
				string b = string.Empty;
				string c = string.Empty;
				string e = string.Empty;
				string f = string.Empty;
				string g = string.Empty;
				foreach (var dig in digits.Where(x => x.Length == 6))
				{
					if (dig.Except(bd).ToArray().Length == 5)
					{
						zero = dig;
					}
					else
					{
						if (dig.Except(one).ToArray().Length == 5)
						{
							six = dig;
						}
						else
						{
							nine = dig;
						}
					}
				}

				c = new string(one.Except(six).ToArray());
				e = new string(six.Except(nine).ToArray());
				d = new string(bd.Except(zero).ToArray());
				b = new string(bd.Except(d).ToArray());
				g = new string(nine.Except(four.Concat(a)).ToArray());
				f = new string(one.Except(c).ToArray());

				two = a + c + d + e + g;
				three = a + c + d + f + g;
				five = a + b + d + f + g;

				List<string> numbers = new List<string>();
				numbers.AddRange(new string[] { zero, one, two, three, four, five, six, seven, eight, nine });

				var output = outputs[iter].Split(' ', StringSplitOptions.RemoveEmptyEntries);
				string value = string.Empty;
				foreach (var digit in output)
				{
					foreach (var num in numbers)
					{
						if (Functions.RoughEqual(num, digit))
						{
							value += numbers.IndexOf(num);
							break;
						}
					}
				}
				results.Add(value);
				iter++;
			}
			List<int> finals = results.ConvertAll<int>(x => int.Parse(x));

			Console.WriteLine(count);
			Console.WriteLine(finals.Sum());
			Console.ReadKey();
		}

		private static void AoC7(string path)
		{
			List<int> crabs = new List<int>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					crabs = line.Split(',').ToList().ConvertAll<int>(x => int.Parse(x));
				}
			}

			int sum = crabs.Sum();
			int count = crabs.Count();
			int halfMean = (sum / count) / 2;

			List<int> fuelRange = new List<int>();
			fuelRange.Add(int.MaxValue);
			//for (int i = leastFuel - 4; i < leastFuel + 5; i++)
			//	fuelRange.Add(i);

			int offset = 0;

			do
			{
				int fuelUsed = 0;
				foreach (var crab in crabs)
				{
					int steps = Math.Abs(crab - (halfMean + offset));
					for (int i = 1; i <= steps; i++)
						fuelUsed += i;
				}
				offset++;
				fuelRange.Add(fuelUsed);
			} while (fuelRange.Last() < fuelRange[fuelRange.Count - 2]);


			Console.WriteLine(fuelRange.Min());
			Console.ReadKey();
		}

		private static void AoC6(string path)
		{
			List<int> lanterns = new List<int>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
					lanterns = line.Split(',').ToList().ConvertAll<int>(x => int.Parse(x));
				}
			}

			int maxDays = 256;
			int day = 0;

			Dictionary<int, long> fish = new Dictionary<int, long>();
			fish.Add(0, lanterns.Count(x => x == 0));
			fish.Add(1, lanterns.Count(x => x == 1));
			fish.Add(2, lanterns.Count(x => x == 2));
			fish.Add(3, lanterns.Count(x => x == 3));
			fish.Add(4, lanterns.Count(x => x == 4));
			fish.Add(5, lanterns.Count(x => x == 5));
			fish.Add(6, lanterns.Count(x => x == 6));
			fish.Add(7, lanterns.Count(x => x == 7));
			fish.Add(8, lanterns.Count(x => x == 8));

			do
			{
				//giving birth today
				long temp = fish[0];
				//progress each day
				fish[0] = fish[1];
				fish[1] = fish[2];
				fish[2] = fish[3];
				fish[3] = fish[4];
				fish[4] = fish[5];
				fish[5] = fish[6];
				fish[6] = fish[7];
				fish[7] = fish[8];
				//create new fish
				fish[8] = temp;
				//reset birth timer
				fish[6] = fish[6] + temp;

				day++;
			} while (day < maxDays);

			Console.WriteLine(fish.Sum(x => x.Value));
			Console.ReadKey();
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
			foreach (var input in inputs)
			{
				var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				ventLines.Add(new VentLines(parts[0], parts[2]));
			}

			Dictionary<Point, int> grid = new Dictionary<Point, int>();
			for (int i = 0; i <= Math.Max(ventLines.Max(x => x.Start.X), ventLines.Max(x => x.End.X)); i++)
			{
				for (int j = 0; j <= Math.Max(ventLines.Max(x => x.Start.Y), ventLines.Max(x => x.End.Y)); j++)
				{
					grid.Add(new Point(i, j), 0);
				}
			}
			var count = ventLines.Count(x => x.Straight());
			foreach (var ventLine in ventLines)
			{
				//if(ventLine.Straight())
				//{
				foreach (var point in ventLine.Points)
					grid[point]++;
				//}
			}

			Console.WriteLine(grid.Count(x => x.Value > 1));
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
						if (cardInput.Count > 0)
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
				if (cards.Count > 1)
					cards.RemoveAll(x => x.CheckForWin());
				else
				{
					if (cards.First().CheckForWin())
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

		private static void AoC(string path)
		{
			List<string> displayTest = new List<string>();
			using (StreamReader file = new StreamReader(path))
			{
				while (!file.EndOfStream)
				{
					var line = file.ReadLine();
				}
			}

			Console.WriteLine();
			Console.ReadKey();

		}
	}
}