using System;
using Xeptions;

namespace DMX.Core.Api.Models.Foundations.LabCommandEvents.Exceptions
{
    public class FailedLabCommandEventDependencyException : Xeption
    {
        public FailedLabCommandEventDependencyException(Exception innerException)
            : base(message: "Failed lab command event dependency error occurred, please contact support.",
                  innerException)
        { }
    }
}
