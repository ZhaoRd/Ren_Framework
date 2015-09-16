namespace Skymate.Communication
{
    using System.ServiceModel;

    using Skymate;

    /// <summary>
    /// 表示用于调用WCF服务的客户端服务代理类型。
    /// </summary>
    /// <typeparam name="T">需要调用的服务契约类型。</typeparam>
    public sealed class ServiceProxy<T> : DisposableObject
        where T : class
    {
        #region Private Fields

        /// <summary>
        /// The client.
        /// </summary>
        private T client = null;

        /// <summary>
        /// The sync.
        /// </summary>
        private static readonly object sync = new object();
        #endregion

        #region Public Properties
        /// <summary>
        /// 获取调用WCF服务的通道。
        /// </summary>
        public T Channel
        {
            get
            {
                lock (sync)
                {
                    if (this.client != null)
                    {
                        var state = (this.client as IClientChannel).State;
                        if (state == CommunicationState.Closed)
                            this.client = null;
                        else
                            return this.client;
                    }
                    var factory = ChannelFactoryManager.Instance.GetFactory<T>();
                    this.client = factory.CreateChannel();
                    (this.client as IClientChannel).Open();
                    return this.client;
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 关闭并断开客户端通道（Client Channel）。
        /// </summary>
        /// <remarks>
        /// 如果使用using语句对ServiceProxy进行了包裹，那么当程序执行点离开using的
        /// 覆盖范围时，Close方法会被自动调用，此时客户端无需显式调用Close方法。反之
        /// 如果没有使用using语句，那么则需要显式调用Close方法。
        /// </remarks>
        public void Close()
        {
            if (this.client != null)
                ((IClientChannel)this.client).Close();
        }
        #endregion

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
            lock (sync)
                {
                    this.Close();
                }
            }

        }
    }
}
