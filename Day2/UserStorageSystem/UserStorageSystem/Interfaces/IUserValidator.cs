using UserStorageSystem.Entities;

namespace UserStorageSystem.Interfaces
{
    /// <summary>
    ///     Used for user entity validation.
    /// </summary>
    public interface IUserValidator
    {
        /// <summary>
        ///     Validates user entity.
        /// </summary>
        /// <param name="user">
        ///     User entity, that will be validated
        /// </param>
        /// <returns>
        ///     true if user is valid, false otherwise.
        /// </returns>
        bool UserIsValid(User user);
    }
}
