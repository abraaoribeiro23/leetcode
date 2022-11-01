using Xunit;

namespace TwoSum
{
    public class TwoSum
    {
        //Input, Target and Output
        public static IEnumerable<object[]> GetNumbers()
        {

            yield return new object[]
            {
                new[] { 2, 7, 11, 15 },
                9,
                new[] { 0, 1 }
            };
            yield return new object[]
            {
                new[] { 3,2,4 },
                6,
                new[] { 1, 2 }
            };
            yield return new object[]
            {
                new[] { 3,3 },
                6,
                new[] { 0, 1 }
            };
        }

        [Theory]
        [MemberData(nameof(GetNumbers))]
        public void TwoSumSuccessTest(int[] nums, int target, int[] expected)
        {
            Assert.True(nums.Length is >= 2 and <= 104);
            Assert.True(target is >= -109 and <= 109);
            Assert.True(nums.All(i => i is >= -109 and <= 109));

            int[] result = TwoSumInts(nums,target);

            Assert.True(expected.All(i => result.Contains(i)));
        }

        private static int[] TwoSumInts(int[] nums, int target)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                for (var j = i + 1; j < nums.Length; j++)
                {
                    var sum = nums[i] + nums[j];
                    if (sum == target)
                    {
                        return new[] { i, j };
                    }
                }
            }

            return null;
        }
    }
}