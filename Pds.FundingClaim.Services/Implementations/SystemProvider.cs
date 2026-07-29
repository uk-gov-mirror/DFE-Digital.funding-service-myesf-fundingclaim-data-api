using Pds.FundingClaim.Services.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Pds.FundingClaim.Services.Implementations
{
    /// <inheritdoc cref="ISystemProvider"/>
    public class SystemProvider : ISystemProvider
    {
        /// <inheritdoc/>
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }

        /// <inheritdoc/>
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        /// <inheritdoc/>
        public byte[] GetXmlSerializedStreamArray(object item)
        {
            if (item != null)
            {
                var messageType = item.GetType();
                var serializer = new DataContractSerializer(messageType);
                var stream = new MemoryStream();
                var binaryDictionaryWriter = XmlDictionaryWriter.CreateBinaryWriter(stream);

                serializer.WriteObject(binaryDictionaryWriter, item);

                binaryDictionaryWriter.Flush();
                return stream.ToArray();
            }

            return null;
        }
    }
}