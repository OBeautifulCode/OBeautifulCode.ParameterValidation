﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Verifications.Workflow.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Assertion.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Assertion.Recipes
{
    using System;
    using System.Collections;
    using System.Linq;

    using OBeautifulCode.Type.Recipes;

    using static System.FormattableString;

#if !OBeautifulCodeAssertionRecipesProject
    internal
#else
    public
#endif
        static partial class Verifications
    {
#pragma warning disable SA1201

        private static void ExecuteVerification(
            this AssertionTracker assertionTracker,
            Verification verification)
        {
            assertionTracker.ThrowImproperUseOfFrameworkIfDetected(AssertionTrackerShould.Exist, AssertionTrackerShould.BeMusted);

            var hasBeenEached = assertionTracker.Actions.HasFlag(Actions.Eached);

            if (hasBeenEached)
            {
                // check that the subject is an IEnumerable and not null
                if (!assertionTracker.Actions.HasFlag(Actions.VerifiedAtLeastOnce))
                {
                    var eachVerification = new Verification
                    {
                        Name = nameof(WorkflowExtensions.Each),
                    };

                    var eachVerifiableItem = new VerifiableItem
                    {
                        Value = assertionTracker.SubjectValue,
                        Type = assertionTracker.SubjectType,
                        IsElementInEnumerable = false,
                    };

                    ThrowIfNotAssignableToType(eachVerification, eachVerifiableItem, MustBeEnumerableTypeValidations.Single());

                    NotBeNullInternal(assertionTracker, eachVerification, eachVerifiableItem);
                }

                var valueAsEnumerable = (IEnumerable)assertionTracker.SubjectValue;

                var enumerableElementType = assertionTracker.SubjectType.GetClosedEnumerableElementType();

                var verifiableItem = new VerifiableItem
                {
                    IsElementInEnumerable = true,
                    Type = enumerableElementType,
                };

                foreach (var typeValidation in verification.TypeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.Handler(verification, verifiableItem, typeValidation);
                }

                foreach (var element in valueAsEnumerable)
                {
                    verifiableItem.Value = element;

                    verification.Handler(assertionTracker, verification, verifiableItem);
                }
            }
            else
            {
                var verifiableItem = new VerifiableItem
                {
                    IsElementInEnumerable = false,
                    Value = assertionTracker.SubjectValue,
                    Type = assertionTracker.SubjectType,
                };

                foreach (var typeValidation in verification.TypeValidations ?? new TypeValidation[] { })
                {
                    typeValidation.Handler(verification, verifiableItem, typeValidation);
                }

                verification.Handler(assertionTracker, verification, verifiableItem);
            }

            assertionTracker.Actions |= Actions.VerifiedAtLeastOnce;
        }

        private static string BuildVerificationFailedExceptionMessage(
            AssertionTracker assertionTracker,
            Verification verification,
            VerifiableItem verifiableItem,
            string exceptionMessageSuffix,
            Include include = Include.None,
            string methodologyInfo = null,
            string contextualInfo = null)
        {
            if (verification.ApplyBecause == ApplyBecause.InLieuOfDefaultMessage)
            {
                // we force to empty string if null because otherwise when the exception is
                // constructed the framework chooses some generic message like 'An exception of type ArgumentException was thrown'
                return verification.Because ?? string.Empty;
            }

            var subjectNameQualifier = assertionTracker.SubjectName == null ? string.Empty : Invariant($" (name: '{assertionTracker.SubjectName}')");

            var enumerableQualifier = verifiableItem.IsElementInEnumerable ? " contains an element that" : string.Empty;

            var methodologyInfoQualifier = methodologyInfo == null ? null : " " + methodologyInfo;

            var contextualInfoQualifier = contextualInfo == null ? null : "  " + contextualInfo;

            var failingValueQualifier = include.HasFlag(Include.FailingValue) ? (verifiableItem.IsElementInEnumerable ? "  Element value" : "  Provided value") + Invariant($" is {verifiableItem.Value.ToStringInErrorMessage()}.") : string.Empty;

            var verificationParametersQualifier = verification.VerificationParameters == null || !verification.VerificationParameters.Any() ? string.Empty : string.Join(string.Empty, verification.VerificationParameters.Select(_ => _.ToStringInErrorMessage()));

            var result = Invariant($"Provided value{subjectNameQualifier}{enumerableQualifier} {exceptionMessageSuffix}{methodologyInfoQualifier}.{contextualInfoQualifier}{failingValueQualifier}{verificationParametersQualifier}");

            if (verification.ApplyBecause == ApplyBecause.PrefixedToDefaultMessage)
            {
                if (!string.IsNullOrWhiteSpace(verification.Because))
                {
                    result = verification.Because + "  " + result;
                }
            }
            else if (verification.ApplyBecause == ApplyBecause.SuffixedToDefaultMessage)
            {
                if (!string.IsNullOrWhiteSpace(verification.Because))
                {
                    result = result + "  " + verification.Because;
                }
            }
            else
            {
                throw new NotSupportedException(Invariant($"This {nameof(ApplyBecause)} is not supported: {verification.ApplyBecause}"));
            }

            return result;
        }

        private static string ToStringInErrorMessage(
            this VerificationParameter verificationParameter)
        {
            var result = Invariant($"  Specified '{verificationParameter.Name}' is");

            result = verificationParameter.ValueToStringFunc == null
                ? Invariant($"{result} {verificationParameter.Value.ToStringInErrorMessage()}")
                : Invariant($"{result} {verificationParameter.ValueToStringFunc()}");

            result = Invariant($"{result}.");

            return result;
        }

        private static string ToStringInErrorMessage(
            this object value)
        {
            var result = value == null
                ? NullValueToString
                : Invariant($"'{value}'");

            return result;
        }

        private static Exception AddData(
            this Exception exception,
            IDictionary data)
        {
            if (data != null)
            {
                // because the caller is creating a new exception, we know that Data is empty
                // and we don't have to check for key conflicts (same key exists in both exception.Data and in data)
                foreach (var dataKey in data.Keys)
                {
                    exception.Data[dataKey] = data[dataKey];
                }
            }

            return exception;
        }

        private static Exception BuildException(
            AssertionTracker assertionTracker,
            Verification verification,
            string exceptionMessage,
            ArgumentExceptionKind argumentExceptionKind)
        {
            Exception result;

            switch (assertionTracker.AssertionKind)
            {
                case AssertionKind.Unknown:
                    result = new AssertionVerificationFailedException(exceptionMessage);
                    break;
                case AssertionKind.Argument:
                    switch (argumentExceptionKind)
                    {
                        case ArgumentExceptionKind.ArgumentException:
                            result = new ArgumentException(exceptionMessage);
                            break;
                        case ArgumentExceptionKind.ArgumentNullException:
                            result = new ArgumentNullException(null, exceptionMessage);
                            break;
                        case ArgumentExceptionKind.ArgumentOutOfRangeException:
                            result = new ArgumentOutOfRangeException(exceptionMessage, (Exception)null);
                            break;
                        default:
                            throw new NotSupportedException(Invariant($"This {nameof(ArgumentExceptionKind)} is not supported: {argumentExceptionKind}."));
                    }

                    break;
                case AssertionKind.Operation:
                    result = new InvalidOperationException(exceptionMessage);
                    break;
                case AssertionKind.Test:
                    result = new TestAssertionVerificationFailedException(exceptionMessage);
                    break;
                default:
                    throw new NotSupportedException(Invariant($"This {nameof(AssertionKind)} is not supported: {assertionTracker.AssertionKind}."));
            }

            result.AddData(verification.Data);

            return result;
        }

        private enum ArgumentExceptionKind
        {
            ArgumentException,

            ArgumentNullException,

            ArgumentOutOfRangeException,
        }

        [Flags]
        private enum Include
        {
            None = 0,

            FailingValue = 1,
        }
#pragma warning restore SA1201
    }
}
