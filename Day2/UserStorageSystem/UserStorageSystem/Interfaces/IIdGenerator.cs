using System.Collections.Generic;

namespace UserStorageSystem.Interfaces
{
    /// <summary>
    ///     Implemented by classes that generate user ids
    /// </summary>
    public interface IIdGenerator : IEnumerator<int>
    {
        /// <summary>
        ///     Stores generator call number 
        /// </summary>
        int CallNumber { get; }

        /// <summary>
        ///     Restores generator's state.
        /// </summary>
        /// <param name="lastId">
        ///     Last user id. Generation starts from the next id.
        /// </param>
        /// <param name="callAttemptCount">
        ///     Used to prevent overflow of the generator.
        /// </param>
        void RestoreGeneratorState(string lastId, int callAttemptCount);
    }
}
