using System.Diagnostics;

namespace D3{
    public static class Program 
    {
        delegate bool CheckIsPrimeNumbers (int number);
        static async Task Main(string[] args)
        {
            // int min = 0, max = 100;
            // int min = 0, max = 1000;
            // int min = 0, max = 10000;
            // int min = 0, max = 100000;
            // int min = 0, max = 1000000;
            int min = 0, max = 10000000;
            // var result2 = await GetPrimeNumbersAsync(min, max, IsPrimeNumberBasic);
            // Console.WriteLine($"Total of Prime Numbers: [{result2.Count}]");
            int n = 6;

            Task<List<int>>[] tasks = new Task<List<int>>[n];
            tasks[0] = GetPrimeNumbersAsync(min, max / n, IsPrimeNumberOptimal, 1);
            for(int i = 1; i <= n - 1; i++)
            {
                tasks[i] = GetPrimeNumbersAsync(max / (i+1), max / i, IsPrimeNumberOptimal, i+1);
            }

            var result = await Task.WhenAll(tasks);
            Console.WriteLine($"Total numbers: {result.Sum(x => x.Count)}");

            var result1 = await GetPrimeNumbersAsync(min, max, IsPrimeNumberOptimal, 7);
            Console.WriteLine($"Total of Prime Numbers: [{result1.Count}]");
            
            Console.WriteLine("Main Done");
        }

        static async Task<List<int>> GetPrimeNumbersAsync (int min, int max, CheckIsPrimeNumbers checker, int? index = null)
        {
            var sw = new Stopwatch();
            sw.Start();

            var list = new List<int>();
            var result = await Task.Factory.StartNew(() => {
                for(var i = min; i <= max; i++)
                {
                    // if(IsPrimeNumberOptimal(i))
                    if(checker(i)){
                        list.Add(i);
                    }
                }
                return list;
            });
            Console.WriteLine($"Done [{index}]:[{sw.ElapsedMilliseconds}]!");
            return result;
        } 
        static void PrintNumbers(List<int> numbers)
        {
            foreach(var number in numbers)
            {
                Console.Write($"{number} ");
            }
        }
        static List<int> GetPrimeNumbers(int min, int max)
        {
            var result = new List<int>();
            for(var i = min; i <= max; i++)
            {
                if(IsPrimeNumberOptimal(i))
                {
                    result.Add(i);
                }
            }
            return result;
        }
        static bool IsPrimeNumberBasic(int number)
        {
            int i;
            for(i = 2; i <= number - 1; i++)
            {
                if(number % i == 0){
                    return false;
                }
            }
            if(number == i){
                return true;
            }
            return false;
        }
        static bool IsPrimeNumberOptimal(int number)
        {
            int i;

            if(number < 2) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));
            for(i = 2; i <= boundary; i++)
            {
                if(number % i == 0){
                    return false;
                }
            }
            return true;
        }
    }
}

