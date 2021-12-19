using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
	public static class Functions
	{
		//from day 12
		public static long LCM(long a, long b)
		{
			return a * b / GCD(a, b);
		}

		public static long GCD(long a, long b)
		{
			a = Math.Abs(a);
			b = Math.Abs(b);

			// Pull out remainders.
			for (; ; )
			{
				long remainder = a % b;
				if (remainder == 0) return b;
				a = b;
				b = remainder;
			};
		}

		//Got from Stackoverflow
		//from day 8
		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
		{
			if (length == 1) return list.Select(t => new T[] { t });

			return GetPermutations(list, length - 1)
				.SelectMany(t => list.Where(e => !t.Contains(e)),
					(t1, t2) => t1.Concat(new T[] { t2 }));
		}

		public static int BinToInt(int[] val)
		{
			return Convert.ToInt32(string.Join("", val), 2);
		}
		public static int BinToInt(string val)
		{
			return Convert.ToInt32(val, 2);
		}

		public static List<string> CheckSupportCriteria(List<string> list, int i, char criteria)
		{
			int ones = 0;
			int zeros = 0;
			int max = list.Count();
			foreach (var supp in list)
				if (supp[i] == '1')
					ones++;
				else
					zeros++;
			if (criteria == 'o')
			{
				if (ones >= zeros)
					return list.Where(x => x[i] == '1').ToList();
				else
					return list.Where(x => x[i] == '0').ToList();
			}
			else
			{
				if (zeros <= ones)
					return list.Where(x => x[i] == '0').ToList();
				else
					return list.Where(x => x[i] == '1').ToList();
			}
		}


		//from day 4
		public static bool calcPassGroups(char[] digits)
		{
			var counts = digits.GroupBy(x => x).Select(x => x.Count());
			if (counts.Contains(2)) //if(counts.Max > 1) //Part 1
				return true;
			else
				return false;
		}

		//from day 4
		public static bool calcPassAsc(char[] digits)
		{
			for (int i = 1; i < digits.Length; i++)
			{
				if (digits[i] - digits[i - 1] < 0)
				{
					return true;
				}
			}
			return false;
		}

		public static bool RoughEqual(string pattern, string input)
		{
			if (input.Length==pattern.Length && input.Except(pattern).ToArray().Length == 0)
				return true;
			else
				return false;
			
		}

	}
}
