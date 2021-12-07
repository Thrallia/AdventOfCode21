using System;
using System;
using System.Collections;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	class Program
	{
		static void Main(string[] args)
		{
			//Aoc1(@"..\..\..\..\inputs\day1.txt");
			Aoc2(@"..\..\..\..\inputs\day2.txt");
			//Aoc3(@"..\..\..\..\inputs\day3.txt");
			//Aoc4(@"..\..\..\..\inputs\day4.txt");
			//Aoc5(@"..\..\..\..\inputs\day5.txt");
			//Aoc6(@"..\..\..\..\inputs\day6.txt");
		}

		private static void Aoc1(string path)
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

		private static void Aoc2(string path)
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
