using System;
using System.Collections;
using System.Collections.Generic;
using IdGen;


namespace UrlShortener.Utils
{

    public class Snowflake41BitIdGenerator : IIdGenerator<long>
    {
        private readonly IdGenerator _innerGenerator;

        public Snowflake41BitIdGenerator(int generatorId, DateTime? fixedEpoch = null, ITimeSource? customTimeSource = null)
        {
            if (generatorId < 0 || generatorId > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(generatorId));
            }

            var epoch = fixedEpoch ?? DateTime.UtcNow;
            var timeSource = customTimeSource ?? new DefaultTimeSource(epoch);

            var structure = new IdStructure(39, 23, 1);
            var options = new IdGeneratorOptions(
                idStructure: structure,
                timeSource: timeSource,
                sequenceOverflowStrategy: SequenceOverflowStrategy.SpinWait
            );

            _innerGenerator = new IdGenerator(generatorId, options);
        }

        public long CreateId()
        {
            long rawId = _innerGenerator.CreateId();
            return ((rawId >> 24) << 2) | (rawId & 3L);
        }
        public IEnumerator<long> GetEnumerator()
        {
            while (true)
            {
                yield return CreateId();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}

