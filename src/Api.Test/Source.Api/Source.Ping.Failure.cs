using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiSource
{
    public static TheoryData<HttpSendFailure, Failure<Unit>> FailurePingTestData
        =>
        new()
        {
            {
                default,
                Failure.Create("An unexpected http failure occured when trying to ping pdf builder: 0.")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.NotFound,
                    Body = new()
                    {
                        Content = BinaryData.FromString("Some failure message")
                    }
                },
                Failure.Create("An unexpected http failure occured when trying to ping pdf builder: 404.\nSome failure message")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.InternalServerError,
                    ReasonPhrase = "Some reason",
                    Body = new()
                    {
                        Content = BinaryData.FromString("Some error text.")
                    }
                },
                Failure.Create("An unexpected http failure occured when trying to ping pdf builder: 500 Some reason.\nSome error text.")
            }
        };
}