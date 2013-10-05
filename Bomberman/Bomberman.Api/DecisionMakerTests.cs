using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;
using Xunit.Extensions;
using System.Linq;

namespace Bomberman.Api
{
    public class Items
    {
        public const string Empty = "*";
        public const string Bomber = "☺";
        public const string Wall = "☼";

    }

    public class DecisionMakerTests
    {
        [Theory, NSubDataAttribute]
        public void should_respond_in_allowed_way(DecisionMaker decisionMaker, string board)
        {
            decisionMaker.NextMove(board).Should().BeOneOf("up", "down", "left", "right", "act");
        }
    }

    public class BoardFactoryTests
    {
        [Fact]
        public void should_create_empty_board()
        {
            var expected = new StringBuilder(Items.Wall.Times(3)).AppendLine()
                .Append(Items.Wall).Append(Items.Empty).AppendLine(Items.Wall)
                .Append(Items.Wall.Times(3))
                .ToString();
            BoardFactory.New(3).Should().Be(expected);
        }
    }

    public class StringExtensionsTests
    {
        [Fact]
        public void should_do_times()
        {
            "w".Times(4).Should().Be("wwww");
        }
    }

    public class ItemAtTests
    {
        private readonly string board = BoardFactory.New(10);

        [Theory]
        [InlineData(2,2,Items.Empty)]
        [InlineData(0,0,Items.Wall)]
        [InlineData(1,1,Items.Empty)]
        [InlineData(9,9,Items.Wall)]
        public void should_return_item_at_board(int row, int col, string expected)
        {
            board.ItemAt(row: row, col: col).Should().Be(expected);
        }

        [Fact]
        public void should_insert_items()
        {
            var newBoard = board.Put(row: 5, col: 4, item: "8");
            Console.Out.WriteLine("{0}", newBoard);
            newBoard.ItemAt(5, 4).Should().Be("8");
        }
    }

    public static class StringExtensions
    {
        public static string Times(this string s, int n)
        {
            return string.Join("", Enumerable.Repeat(s, n));
        }

        public static string Put(this string s, int row, int col, string item)
        {
            var lines = Split(s);
            lines[row] = lines[row].Remove(col, 1).Insert(col, item);
            return String.Join(Environment.NewLine, lines);
        }


        public static string ItemAt(this string board, int row, int col)
        {
            return Split(board)[row][col].ToString();
        }

        private static string[] Split(string board)
        {
            return board.Split(new []{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static IEnumerable<char> GetTail(this string board, int row, int col)
        {
            Func<IEnumerable<char>, int, IEnumerable<char>> skipRows = null;
            skipRows = (s, i) =>
                {
                    if (i == 0)
                    {
                        return s;
                    }
                    var n = Environment.NewLine;
                    return skipRows(s.SkipWhile(c => c != n[0]).Skip(n.Length), i - 1);
                };
            var enumerable = skipRows(board, row).Skip(col);
            return enumerable;
        }
        
        private static IEnumerable<char> GetHead(this string board, int row, int col)
        {
            Func<IEnumerable<char>, int, IEnumerable<char>> skipRows = null;
            skipRows = (s, i) =>
                {
                    if (i == 0)
                    {
                        return s;
                    }
                    var n = Environment.NewLine;
                    return skipRows(s.TakeWhile(c => c != n[0]).Take(n.Length), i - 1);
                };
            var enumerable = skipRows(board, row).Take(col);
            return enumerable;
        }
    }

    public class BoardFactory
    {
        public static string New(int i)
        {
            var middle = string.Join(Environment.NewLine, Enumerable.Repeat(string.Format("{0}{1}{0}", Items.Wall, string.Join("", Enumerable.Repeat(Items.Empty, i - 2))), i - 2));
            return String.Format("{0}{1}{2}{1}{0}", Items.Wall.Times(i), Environment.NewLine, middle);
        }
    }

}