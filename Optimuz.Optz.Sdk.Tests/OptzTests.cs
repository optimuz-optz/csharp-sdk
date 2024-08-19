using Optimuz.Optz.Sdk.Exceptions;

namespace Optimuz.Optz.Sdk.Tests;

public class Tests
{
    [Test]
    public void GetInstance_ShouldReturnOptzInstance()
    {
        var optz = Optz.GetInstance("*.host.com", "account", "username", "password");
        Assert.That(optz, Is.InstanceOf<Optz>());
    }

    [Test]
    public void GetInstance_ShouldReturnSameInstanceForSameParameters()
    {
        var optz1 = Optz.GetInstance("*.host.com", "account", "username", "password");
        var optz2 = Optz.GetInstance("*.host.com", "account", "username", "password");
        Assert.That(optz1, Is.SameAs(optz2));
    }

    [Test]
    public void GetInstance_ShouldReturnSameInstanceForSameParameters_WhenCalledConcurrently()
    {
        var instances = new List<Optz>();

        Task.WaitAll([
            Task.Run(() => instances.Add(Optz.GetInstance("*.host.com", "account", "username", "password"))),
            Task.Run(() => instances.Add(Optz.GetInstance("*.host.com", "account", "username", "password"))),
        ]);

        Assert.That(instances[0], Is.SameAs(instances[1]));
    }

    [Test]
    public void GetInstance_ShouldReturnDifferentInstanceForDifferentParameters()
    {
        var optz1 = Optz.GetInstance("*.host.com", "account", "username", "password");
        var optz2 = Optz.GetInstance("*.host.com", "other_account", "other_username", "other_password");
        Assert.That(optz1, Is.Not.SameAs(optz2));
    }

    [Test]
    public void GetInstance_ShouldReturnDifferentInstanceForDifferentParameters_WhenCalledConcurrently()
    {
        var instances = new List<Optz>();

        Task.WaitAll([
            Task.Run(() => instances.Add(Optz.GetInstance("*.host.com", "account", "username", "password"))),
            Task.Run(() => instances.Add(Optz.GetInstance("*.host.com", "other_account", "other_username", "other_password"))),
        ]);

        Assert.That(instances[0], Is.Not.SameAs(instances[1]));
    }

    [Test]
    public void GetInstance_ShouldThrowInvalidHostException_WhenHostIsNotWildcard()
    {
        Assert.Throws<InvalidHostException>(() => Optz.GetInstance("host.com", "account", "username", "password"));
    }
}