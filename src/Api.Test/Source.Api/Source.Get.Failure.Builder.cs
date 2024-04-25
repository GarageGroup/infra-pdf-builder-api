using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiSource
{
    public static TheoryData<HttpSendFailure, Failure<Unit>> FailureHttpBuilderPdfGetTestData
        =>
        new()
        {
            {
                default,
                Failure.Create("An unexpected http failure occured when trying to build pdf document: 0.")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.NotFound,
                    Body = new()
                    {
                        Type = new("application/json"),
                        Content = BinaryData.FromString("Some failure message")
                    }
                },
                Failure.Create("An unexpected http failure occured when trying to build pdf document: 404.\nSome failure message")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.InternalServerError,
                    ReasonPhrase = "Some reason",
                    Headers =
                    [
                        new("SomeHeader", "Some value")
                    ],
                    Body = new()
                    {
                        Content = BinaryData.FromString("Some error text.")
                    }
                },
                Failure.Create("An unexpected http failure occured when trying to build pdf document: 500 Some reason.\nSome error text.")
            }
        };
}