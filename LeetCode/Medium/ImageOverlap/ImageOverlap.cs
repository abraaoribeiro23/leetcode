using Xunit;
using Xunit.Abstractions;

namespace ImageOverlap
{
    public class ImageOverlap
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ImageOverlap(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static IEnumerable<object[]> GetNumbers()
        {

            yield return new object[]
            {
                new[] { new[] { 1 } },
                new[] { new[] { 1 } },
                1
            };
            yield return new object[]
            {
                new[] { new[] { 0 } },
                new[] { new[] { 0 } },
                0
            };
            yield return new object[]
            {
                new[] { new[] { 1, 1, 0 }, new[] { 0, 1, 0 }, new[] { 0, 1, 0 }, },
                new[] { new[] { 0, 0, 0 }, new[] { 1, 0, 0 }, new[] { 0, 1, 0 }, },
                2
            };
        }

        [Theory]
        [MemberData(nameof(GetNumbers))]
        public void LargestOverlapTest(int[][] img1, int[][] img2, int expected)
        {
            Assert.True(HasValidSquareMatrice(img1));
            Assert.True(HasValidSquareMatrice(img2));
            Assert.True(HasValidSquareSize(img1));
            Assert.True(HasValidSquareSize(img2));
            Assert.True(HasValidValues(img1));
            Assert.True(HasValidValues(img2));

            var result = Main(img1, img2);
            Assert.Equal(expected, result);
        }

        private int Main(int[][] img1, int[][] img2)
        {
            _testOutputHelper.PrintMatrix(img1, "Matrix 1");
            _testOutputHelper.PrintMatrix(img2, "Matrix 2");

            var result = CountOverlap(img1, img2);
            var resultRd = SearchTo(img1, img2, MovimentsEnum.R, MovimentsEnum.D);
            var resultDl = SearchTo(img1, img2, MovimentsEnum.D, MovimentsEnum.L);
            var resultLu = SearchTo(img1, img2, MovimentsEnum.L, MovimentsEnum.U);
            var resultUr = SearchTo(img1, img2, MovimentsEnum.U, MovimentsEnum.R);

            _testOutputHelper.WriteLine("Largest Overlap 0: " + result);
            _testOutputHelper.WriteLine("Largest Overlap Right/Down: " + resultRd);
            _testOutputHelper.WriteLine("Largest Overlap Down/Left: " + resultDl);
            _testOutputHelper.WriteLine("Largest Overlap Left/Up: " + resultLu);
            _testOutputHelper.WriteLine("Largest Overlap Up/Right: " + resultUr);

            result = GetLargest(result, resultRd);
            result = GetLargest(result, resultDl);
            result = GetLargest(result, resultLu);
            result = GetLargest(result, resultUr);

            _testOutputHelper.WriteLine("Largest Overlap: " + result);

            return result;
        }

        private static int GetLargest(int n1, int n2) => n1 > n2 ? n1 : n2;

        private static int SearchTo(int[][] img1, int[][] img2, MovimentsEnum moviment, MovimentsEnum secundaryMoviment,
            int iteration = 1)
        {
            var count = 0;

            var matrix = img1.DeepCopy();

            matrix.Move(moviment);

            var countOverlap = CountOverlap(matrix, img2);

            var maxIteration = img1.Length - 1;

            if (iteration < maxIteration)
            {
                count = SearchTo(matrix, img2, moviment, secundaryMoviment, iteration + 1);
            }

            var secondaryCount = SearchToSide(matrix, img2, secundaryMoviment);

            var largestOverlap = count > countOverlap ? count : countOverlap;
            largestOverlap = secondaryCount > largestOverlap ? secondaryCount : largestOverlap;

            return largestOverlap;
        }
        
        private static int SearchToSide(int[][] img1, int[][] img2, MovimentsEnum side, int iteration = 1)
        {
            var count = 0;

            var matrix = img1.DeepCopy();

            matrix.Move(side);

            var countOverlap = CountOverlap(matrix, img2);

            var maxIteration = img1.Length - 1;

            if (iteration < maxIteration)
            {
                count = SearchToSide(matrix, img2, side, iteration + 1);
            }

            var largestOverlap = count > countOverlap ? count : countOverlap;

            return largestOverlap;
        }

        private static bool HasValidSquareMatrice(int[][] img)
        {
            return img.All(t => img.Length == t.Length);
        }

        private static bool HasValidSquareSize(int[][] img)
        {
            return img.Length is >= 1 and <= 30;
        }

        private static bool HasValidValues(int[][] img)
        {
            return img.All(t => t.All(i => i is 1 or 0));
        }

        private static int CountOverlap(int[][] img1, int[][] img2)
        {
            var count = 0;
            for (var index = 0; index < img1.Length; index++)
            {
                var xs1 = img1[index];
                var xs2 = img2[index];
                for (var x = 0; x <= img1.Length - 1; x++)
                {
                    if(xs1[x] == 1 && xs2[x] == 1)
                        count++;
                }
            }
            return count;
        }
    }

    internal static class MatrixExtensions
    {
        public static void Move(this int[][] img, MovimentsEnum side)
        {
            if(side == MovimentsEnum.U)
                img.MoveU();
            if(side == MovimentsEnum.R)
                img.MoveR();
            if (side == MovimentsEnum.D)
                img.MoveD();
            if (side == MovimentsEnum.L)
                img.MoveL();
        }
        public static void MoveR(this int[][] img)
        {
            foreach (var x in img)
            {
                for (var i = x.Length - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        x[i] = 0;
                    }
                    else
                    {
                        x[i] = x[i - 1];
                    }
                }
            }
        }
        public static void MoveL(this int[][] img)
        {
            foreach (var x in img)
            {
                for (var i = 0; i <= x.Length - 1; i++)
                {
                    if (i == x.Length - 1)
                    {
                        x[i] = 0;
                    }
                    else
                    {
                        x[i] = x[i + 1];
                    }
                }
            }
        }
        public static void MoveU(this int[][] img)
        {
            for (var y = 0; y <= img.Length - 1; y++)
            {
                for (var x = 0; x <= img.Length - 1; x++)
                {
                    if (x == img.Length - 1)
                    {
                        img[x][y] = 0;
                    }
                    else
                    {
                        img[x][y] = img[x + 1][y];
                    }
                }
            }
        }
        public static void MoveD(this int[][] img)
        {
            for (var y = 0; y <= img.Length - 1; y++)
            {
                for (var x = img.Length - 1; x >= 0; x--)
                {
                    if (x == 0)
                    {
                        img[x][y] = 0;
                    }
                    else
                    {
                        img[x][y] = img[x - 1][y];
                    }
                }
            }
        }

        public static int[][] DeepCopy(this int[][] img)
        {
            return img.Select(a => (int[])a.Clone()).ToArray();
        }
    }
    enum MovimentsEnum
    {
        U,
        R,
        D,
        L
    }
    internal static class PrintMatrixExtensions
    {
        public static void PrintMatrix(this ITestOutputHelper testOutputHelper, int[][] img, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
                testOutputHelper.WriteLine(msg);

            foreach (var xs in img)
            {
                var line = "";
                for (var x = 0; x <= xs.Length - 1; x++)
                {
                    line += xs[x] + " ";
                }

                testOutputHelper.WriteLine(line);
            }
            testOutputHelper.WriteLine("");
        }
    }
}