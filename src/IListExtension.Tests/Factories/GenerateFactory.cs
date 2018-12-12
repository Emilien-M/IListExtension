using System;
using System.Collections.Generic;
using System.Linq;

namespace IListExtension.Tests.Factories
{
    public class GenerateFactory
    {
        private Random Random { get; }

        public GenerateFactory()
        {
            Random = new Random();
        }
        
        public IEnumerable<T> Generator<T>(Func<T> generatorFunc)
        {
            while (true)
            {
                yield return generatorFunc();
            }
        }

        public IEnumerable<string> GenerateRandomString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

            return Generator(() => new string(Generator(() => chars[Random.Next(chars.Length)])
                .Take(length)
                .ToArray()));
        }

        public IEnumerable<int> GenerateRandomInt(int min, int max)
        {
            return Generator(() => Random.Next(min, max));
        }

        public IEnumerable<float> GenerateRandomFloat(float min, float max)
        {
            return Generator(() => (float)(Random.NextDouble() * (max - min) + min));
        }

        public IEnumerable<bool> GenerateRandomBool()
        {
            return Generator(() => Random.NextDouble() > 0.5);
        }

        public IEnumerable<DateTime> GenerateRandomDate(DateTime min, DateTime max)
        {
            return Generator(() => new DateTime((long) GenerateRandomFloat(min.Ticks, max.Ticks).First()));
        }
    }
}