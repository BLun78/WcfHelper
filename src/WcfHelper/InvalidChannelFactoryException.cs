namespace WcfHelper
{
    using System;
    using System.Runtime.Serialization;

    using JetBrains.Annotations;

    [Serializable]
    public sealed class InvalidChannelFactoryException : Exception, ISerializable
    {
        public InvalidOperationException ConfigurationChannelFactoryException => this.configurationChannelFactoryException;
        private readonly InvalidOperationException configurationChannelFactoryException;

        public InvalidOperationException ChannelFactoryException => this.channelFactoryException;
        private readonly InvalidOperationException channelFactoryException;

        public InvalidChannelFactoryException() : base()
        {
            this.configurationChannelFactoryException = null;
            this.channelFactoryException = null;
        }

        public InvalidChannelFactoryException([NotNull] string message) : base(message)
        {
            this.configurationChannelFactoryException = null;
            this.channelFactoryException = null;
        }

        public InvalidChannelFactoryException([NotNull] string message,
                                              [NotNull] Exception innerException)
            : base(message, innerException)
        {
            this.configurationChannelFactoryException = null;
            this.channelFactoryException = null;
        }

        public InvalidChannelFactoryException([NotNull] InvalidOperationException configurationChannelFactoryException,
                                              [NotNull] InvalidOperationException channelFactoryException) : base()
        {
            this.configurationChannelFactoryException = configurationChannelFactoryException;
            this.channelFactoryException = channelFactoryException;
        }

        public InvalidChannelFactoryException([NotNull] string message,
                                              [NotNull] InvalidOperationException configurationChannelFactoryException,
                                              [NotNull] InvalidOperationException channelFactoryException)
            : base(message)
        {
            this.configurationChannelFactoryException = configurationChannelFactoryException;
            this.channelFactoryException = channelFactoryException;
        }

        public InvalidChannelFactoryException([NotNull] string message,
                                              [NotNull] Exception inner,
                                              [NotNull] InvalidOperationException configurationChannelFactoryException,
                                              [NotNull] InvalidOperationException channelFactoryException)
            : base(message, inner)
        {
            this.configurationChannelFactoryException = configurationChannelFactoryException;
            this.channelFactoryException = channelFactoryException;
        }

        private InvalidChannelFactoryException([NotNull] SerializationInfo serializationInfo,
                                               [NotNull] StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public override void GetObjectData([NotNull]SerializationInfo info,
                                           [NotNull] StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
