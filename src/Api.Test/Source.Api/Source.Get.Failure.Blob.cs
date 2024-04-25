using System;
using GarageGroup.Infra;
using Xunit;

namespace GarageGroup.Platform.Pdf.Api.Test;

partial class PdfApiSource
{
    public static TheoryData<HttpSendFailure, Failure<Unit>> UnexpectedFailureHttpBlobPdfGetTestData
        =>
        new()
        {
            {
                default,
                Failure.Create("An unexpected http failure occured when trying to check if file exists: 0.")
            },
            {
                new()
                {
                    StatusCode = HttpFailureCode.BadRequest,
                    Body = new()
                    {
                        Type = new("application/json"),
                        Content = BinaryData.FromString("Some failure message")
                    }
                },
                Failure.Create("An unexpected http failure occured when trying to check if file exists: 400.\nSome failure message")
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
                Failure.Create("An unexpected http failure occured when trying to check if file exists: 500 Some reason.\nSome error text.")
            }
        };
}