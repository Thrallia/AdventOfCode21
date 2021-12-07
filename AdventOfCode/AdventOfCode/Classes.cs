using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

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
			while(current != null)
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
				while(current.next !=null)
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
}
