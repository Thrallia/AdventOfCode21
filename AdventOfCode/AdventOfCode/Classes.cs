using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace AdventOfCode
{
	public class OrderedList : IEnumerable
	{
		Node root;

		public OrderedList()
		{ }
		public OrderedList(Node node)
		{
			root = node;
		}

		public void TraverseList()
		{
			Node current = root;
			while (current != null)
			{
				Console.WriteLine(current.data);
				current = current.next;
			}
		}

		public void AddNext(object data)
		{
			Node node = new Node(data);

			if (root == null)
				root = node;
			else
			{
				Node current = root;
				while (current.next != null)
				{
					current = current.next;
				}
				current.next = node;
				node.previous = current;
			}
		}

		public Node First()
		{
			return root;
		}

		public IEnumerator GetEnumerator()
		{
			return Enumerator();
		}

		public IEnumerator Enumerator()
		{
			var current = root;
			while (current != null)
			{
				yield return current;
				current = current.next;
			}
		}
	}

	public class Node
	{
		public Node previous;
		public Node next;
		public object data;

		public Node(object obj)
		{
			data = obj;
		}
	}

	public class BingoCard
	{
		BingoSpace[,] Card = new BingoSpace[5, 5];
		int[] row = new int[5];
		int[] col = new int[5];

		public void FillCard(List<string> input)
		{

			int x = 0;
			foreach (var inp in input)
			{
				row[x] = 0;
				col[x] = 0;
				int y = 0;
				var parts = inp.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				foreach (var i in parts)
				{
					BingoSpace space = new BingoSpace(int.Parse(i));
					Card[x, y] = space;
					y++;
				}
				x++;
			}
		}

		public void SearchCard(int i)
		{
			for (int x = 0; x < Card.GetLength(0); x++)
			{
				for (int y = 0; y < Card.GetLength(1); y++)
				{
					if (Card[x, y].GetValue() == i)
					{
						Card[x, y].MarkSpace();
						row[x]++;
						col[y]++;
					}
				}
			}
		}

		public bool CheckForWin()
		{
			bool win = false;
			//check rows and columns
			for (int i = 0; i < row.Length; i++)
			{
				if (row[i] == 5)
				{
					win = true;
				}
				if (col[i] == 5)
				{
					win = true;
				}
			}
			//DO NOT CARE ABOUT DIAGONALS
			//check both diagonals
			//if (Card[0, 0].GetStatus() && Card[1, 1].GetStatus() && Card[2, 2].GetStatus() && Card[3, 3].GetStatus() && Card[4, 4].GetStatus())
			//{
			//	win = true;
			//}
			//if (Card[0, 4].GetStatus() && Card[1, 3].GetStatus() && Card[2, 2].GetStatus() && Card[3, 1].GetStatus() && Card[4, 0].GetStatus())
			//{
			//	win = true;
			//}

			return win;
		}

		public int GetUnmarkedSum()
		{
			int sum = 0;
			for (int x = 0; x < Card.GetLength(0); x++)
			{
				for (int y = 0; y < Card.GetLength(1); y++)
				{
					if (!Card[x, y].GetStatus()) 
						sum += Card[x, y].GetValue();
				}
			}
			return sum;
		}

	}

	public class BingoSpace
	{
		int value;
		bool called;

		public BingoSpace(int val)
		{
			value = val;
			called = false;
		}

		public void MarkSpace()
		{
			called = true;
		}

		public bool GetStatus()
		{
			return called;
		}

		public int GetValue()
		{
			return value;
		}
	}

	public class VentLines
	{
		public Point Start;
		public Point End;
		public List<Point> Points;

		public VentLines(string start, string end)
		{
			var startSplit = start.Split(',');
			Start.X = int.Parse(startSplit[0]);
			Start.Y = int.Parse(startSplit[1]);

			var endSplit = end.Split(',');
			End.X = int.Parse(endSplit[0]);
			End.Y = int.Parse(endSplit[1]);

			Points = GetRange(Start.X, End.X)
				.Zip(GetRange(Start.Y, End.Y))
			.Select(zip => new Point(zip.First, zip.Second))
			.ToList();
		}

		private IEnumerable<int> GetRange(int s, int e)
		{
			IEnumerable<int> points;

			if (s == e)
				points= Enumerable.Repeat(s, int.MaxValue);
			else
			{
				if (s < e)
					points = Enumerable.Range(s, Math.Abs(s-e) + 1);
				else
					points = Enumerable.Range(e, Math.Abs(s - e) + 1).Reverse();
			}

			return points;
		}

		public bool Straight()
		{
			if (Start.X == End.X || Start.Y == End.Y)
				return true;
			else
				return false;
		}

		public int GetXDistance()
		{
			return Math.Abs(Start.X - End.X);
		}

		public int GetYDistance()
		{
			return Math.Abs(Start.Y - End.Y);
		}
	}
}
