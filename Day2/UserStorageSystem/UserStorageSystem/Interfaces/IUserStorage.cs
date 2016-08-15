using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    /// <summary>
    ///     Used for storing user information.
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        ///     Stores user information
        /// </summary>
        /// <param name="storageInfo">
        ///     Contains all necessary information for user service.
        /// </param>
        void SaveUsers(StorageInfo storageInfo);

        /// <summary>
        ///     Loads user information
        /// </summary>
        /// <returns>
        ///     Object containing all necessary information for restoring user service state.
        /// </returns>
        StorageInfo LoadUsers();     
    }
}
