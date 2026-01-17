using System;
using System.Threading.Tasks;

namespace Payments.Core.Domain.Interfaces
{
    public interface IPublisher
    {
            Task Publish<T>(T content, Uri queueAddress);
    }
}
