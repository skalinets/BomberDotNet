using System;
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


    public static class StringExtensions
    {
        public static string Times(this string s, int n)
        {
            return string.Join("", Enumerable.Repeat(s, n));
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