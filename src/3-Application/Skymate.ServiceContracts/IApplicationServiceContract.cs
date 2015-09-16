
namespace Skymate.ServiceContracts
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// 表示实现该接口的类型为Application Service Contract。
    /// </summary>
    public interface IWcfServiceContract : IDisposable
    {
    }

    /// <summary>
    /// The LocationService interface.
    /// 定位服务
    /// </summary>
    [ServiceContract(Namespace = "http://www.skymate.com")]
    public interface ILocationService : Skymate.ServiceContracts.IWcfServiceContract
    {
        #region Methods
       
        [OperationContract]       
        void Locate(double x,double y);
        #endregion
    }

}
