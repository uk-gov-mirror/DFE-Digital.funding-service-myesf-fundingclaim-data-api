using System;

namespace Pds.FundingClaim.Services.Interfaces
{
    /// <summary>
    /// The datetime provider to help with datetime operations.
    /// </summary>
    public interface ISystemProvider
    {
        /// <summary>
        /// Gets the current UTC datetime.
        /// </summary>
        /// <returns>Returns the current UTC datetime.</returns>
        DateTime UtcNow();

        /// <summary>
        /// Gets a new Guid.
        /// </summary>
        /// <returns>The generated Guid.</returns>
        Guid NewGuid();

        /// <summary>
        /// Get the stream array of the object serialized to xml.
        /// </summary>
        /// <param name="item">The object to work on.</param>
        /// <returns>The stream byte array.</returns>
        byte[] GetXmlSerializedStreamArray(object item);
    }
}