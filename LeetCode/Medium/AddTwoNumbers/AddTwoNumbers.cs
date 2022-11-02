using Xunit;

namespace AddTwoNumbers
{
    public class AddTwoNumbers
    {
        public static IEnumerable<object[]> GetNumbers()
        {

            yield return new object[]
            {
                new ListNode(2, new ListNode(4, new ListNode(3))),
                new ListNode(5, new ListNode(6, new ListNode(4))),
                new ListNode(7, new ListNode(0, new ListNode(8))),
            };
            yield return new object[]
            {
                new ListNode(9),
                new ListNode(1, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9, new ListNode(9)))))))))),
                new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0, new ListNode(0,new ListNode(1))))))))))),
            };
        }

        [Theory]
        [MemberData(nameof(GetNumbers))]
        public void AddTwoNumbersSuccessTest(ListNode l1, ListNode l2, ListNode? expected)
        {
            var result = SumNodes(l1, l2);
            Assert.Equal(expected, result);
        }

        private static ListNode SumNodes(ListNode? l1, ListNode? l2, int incrementValue = 0)
        {
            var firstVal = l1?.val ?? 0;
            var secVal = l2?.val ?? 0;

            var actualNodeVal = firstVal + secVal + incrementValue;

            var nextValIncrement = 0;

            if (actualNodeVal >= 10)
            {
                nextValIncrement = actualNodeVal / 10;
                actualNodeVal %= 10;
            }

            ListNode nextNode = null;

            if(l1 != null && l1.HasNext() || l2 != null && l2.HasNext() || nextValIncrement>0)
                nextNode = SumNodes(l1?.next,l2?.next, nextValIncrement);

            var newNode = new ListNode(actualNodeVal, nextNode);

            return newNode;
        }
    }

    public static class ListNodeExtensions
    {
        public static bool HasNext(this ListNode list)
        {
            return list.next != null;
        }
    }

    public class ListNode
    {
        public int val;
        public ListNode? next;
        public ListNode(int val = 0, ListNode? next = null)
        {
            this.val = val;
            this.next = next;
        }
    }
}