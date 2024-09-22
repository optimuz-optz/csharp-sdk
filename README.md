# Optz SDK

The **Optz SDK** provides a simple interface for interacting with the Optz services.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Authentication](#authentication)
- [Available Methods](#available-methods)
  - [Mail](#mail)
    - [QueueEmail](#queueemail)
    - [QueueSms](#queuesms)
    - [QueueWhatsApp](#queuewhatsapp)
- [Results](#results)
- [License](#license)

## Installation

To install the **Optz SDK**, add the following package to your project:

```bash
dotnet add package Optimuz.Optz.Sdk
```

## Usage

Here is an example of how to use the **Optz SDK**:

```csharp
using Optimuz.Optz.Sdk;

public class Program
{
    public static async Task Main(string[] args)
    {
        var optz = Optz.GetInstance("*.optz.com.br", "account", "username", "password");

        var result = await optz.SomeRequest();
    }
}
```

**GetInstance** method is thread-safe and guarantees that it returns the same instance for the same parameters.

## Authentication

The **Optz SDK** handles authentication internally.

When you call any method that requires authentication, the SDK will automatically authenticate using the provided credentials.

## Available Methods

### Mail

#### QueueEmail

Queues an email for sending. Returns a result indicating success or failure.

```csharp
Task<OneOf<ItemResult<Mail.Email.Queue.Response>, ErrorResult>> QueueEmail(Mail.Email.Queue.Request request, CancellationToken cancellationToken = default);
```

#### QueueSms

Queues an SMS for sending. Returns a result indicating success or failure.

```csharp
Task<OneOf<ItemResult<Mail.Sms.Queue.Response>, ErrorResult>> QueueSms(Mail.Sms.Queue.Request request, CancellationToken cancellationToken = default);
```

#### QueueWhatsApp

Queues an WhatsApp message for sending. Returns a result indicating success or failure.

```csharp
Task<OneOf<ItemResult<Mail.WhatsApp.Queue.Response>, ErrorResult>> QueueWhatsApp(Mail.WhatsApp.Queue.Request request, CancellationToken cancellationToken = default);
```

## Results

The methods in the **Optz SDK** return a [OneOf](https://github.com/mcintyre321/OneOf) type, which can represent either a successful result or an error result.

### ItemResult/ListResult

When a method call is successful, it returns either a **ItemResult** or a **ListResult** object. These objects contains the data related to the successful operation.

### ErrorResult

When a method call fails, it returns an **ErrorResult** object. This object contains a list of errors and warnings related to the failured operation.

## License

This project is licensed under the terms described in the LICENSE file.
